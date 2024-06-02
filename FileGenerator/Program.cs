using Databases.Drivers;
using Databases;
using Models;
using FileGenerator.FileGenerators;
using Repositories;
using Services;
using Controllers;

DatabaseClient sql = new(new SqlServerDriver("db_radar", "sa", "SqlServer2019!"));
DatabaseClient mongo = new(new MongoDriver("radar", "root", "Mongo@2024#"));

RecordRepository sqlRepository = new(sql);
RecordRepository mongoRepository = new(mongo);

RecordService sqlService = new(sqlRepository);
RecordService mongoService = new(mongoRepository);

RecordController sqlController = new(sqlService);
RecordController mongoController = new(mongoService);

string radarsPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\radars\";

List<IFileGenerator> generatorsUsingSqlServerData = new()
{
    new JsonFileGenerator(radarsPath + @"\sql\", "dados.json"),
    new XmlFileGenerator(radarsPath + @"\sql\", "dados.xml"),
    new CsvFileGenerator(radarsPath + @"\sql\", "dados.csv")
};

List<IFileGenerator> generatorsUsingMongoData = new()
{
    new JsonFileGenerator(radarsPath + @"\mongo\", "dados.json"),
    new XmlFileGenerator(radarsPath + @"\mongo\", "dados.xml"),
    new CsvFileGenerator(radarsPath + @"\mongo\", "dados.csv")
};

List<Record> sqlRecords = sqlController.FindAll();
List<Record> mongoRecords = mongoController.FindAll();

var tasks = new List<Task>();

generatorsUsingSqlServerData.ForEach(generator => tasks.Add(Task.Run(() => generator.Generate(sqlRecords))));
generatorsUsingMongoData.ForEach(generator => tasks.Add(Task.Run(() => generator.Generate(mongoRecords))));

await Task.WhenAll(tasks);

Console.WriteLine("Todos dados do SQLServer e MongoDB foram criados e salvos em formatos JSON, XML e CSV!");