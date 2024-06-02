using System.Reflection;

namespace FileGenerator.FileGenerators
{
    internal class CsvFileGenerator : IFileGenerator
    {
        private string _finalPath;

        public CsvFileGenerator(string path, string file)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            _finalPath = path + file;

            if (!File.Exists(_finalPath))
                File.Create(_finalPath).Close();
        }

        public void Generate<T>(List<T> entities)
        {
            using (StreamWriter sw = new StreamWriter(_finalPath))
            {
                List<PropertyInfo> properties = typeof(T).GetProperties().ToList();
                List<string> columns = properties.Select(property => property.Name).ToList();

                string names = string.Join(",", columns);

                sw.WriteLine(names);

                foreach (var entity in entities)
                {
                    string values = string.Join(",", properties.Select(property => property.GetValue(entity)?.ToString()?.Replace(',', '.')).ToList());

                    sw.WriteLine(values);
                }
            }
        }
    }
}
