namespace TestGenerator;

public static class Programm
{
    static void Main()
    {
        string modelClass = "AxisRichRelationalModel";
        string className = modelClass + "Hash";
        string modelInterface = "I" + modelClass;

        var parameters = new[]
        {
            new Param("IGuid","id","new Guid()","new DeterminedHash(id)"),
            new Param("IGuid","chartId","new Guid()","new DeterminedHash(chartId)"),
            new Param("IString","legend","new RandomString()","new DeterminedHash((model as RelationalModel.Abstractions.IAxisRelationalModel).Legend)"),
        };

        Console.WriteLine(TestGenerator.Generate(className, modelInterface, modelClass, parameters));
    }
}
