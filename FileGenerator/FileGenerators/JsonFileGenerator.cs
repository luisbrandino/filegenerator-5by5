using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FileGenerator.FileGenerators
{
    internal class DefaultClassPropertyName : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            foreach (JsonProperty prop in list)
                prop.PropertyName = prop.UnderlyingName;

            return list;
        }
    }

    internal class JsonFileGenerator : IFileGenerator
    {
        private string _finalPath;

        public JsonFileGenerator(string path, string file)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            _finalPath = path + file;

            if (!File.Exists(_finalPath))
                File.Create(_finalPath).Close();
        }

        public void Generate<T>(List<T> entities)
        {
            string data = JsonConvert.SerializeObject(entities, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultClassPropertyName()
            });

            using (StreamWriter writer = new StreamWriter(_finalPath))
                writer.Write(data);
        }

    }
}
