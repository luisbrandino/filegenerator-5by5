using System.Xml.Serialization;

namespace FileGenerator.FileGenerators
{
    public class XmlFileGenerator : IFileGenerator
    {
        private string _finalPath;

        public XmlFileGenerator(string path, string file)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            _finalPath = path + file;

            if (!File.Exists(_finalPath))
                File.Create(_finalPath).Close();
        }

        public void Generate<T>(List<T> entities) 
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, entities);

                string data = stringWriter.ToString();

                using (StreamWriter writer = new StreamWriter(_finalPath))
                    writer.Write(data);
            }
        }

    }
}
