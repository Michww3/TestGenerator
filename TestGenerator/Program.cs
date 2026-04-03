using System;

class HashTestGenerator
{
    static void Main()
    {
        string modelClass = "SeriesRelationalModel";
        string className = modelClass + "Hash";
        string modelInterface = "I"+ modelClass;

        var parameters = new[]
        {
            new Param("IGuid","id","new Guid()","new DeterminedHash(id)"),
            new Param("IGuid","chartId","new Guid()","new DeterminedHash(chartId)"),
            new Param("IString","legend","new RandomString()","new DeterminedHash(legend)"),
            new Param("IString","xAxisSource","new RandomString()","new DeterminedHash(xAxisSource)"),
            new Param("IString","yAxisSource","new RandomString()","new DeterminedHash(yAxisSource)"),
        };

        Generate(className, modelInterface, modelClass, parameters);
    }

    static void Generate(string className, string modelInterface, string modelClass, Param[] p)
    {
        int n = p.Length;
        int total = 1 << n;

        for (int mask = 1; mask < total - 1; mask++)
        {
            Console.WriteLine("[Fact]");
            Console.WriteLine($"public void ProduceCorrectHashFrom{MaskName(mask, p)}()");
            Console.WriteLine("{");

            // init parameters
            foreach (var param in p)
                Console.WriteLine($"    {param.Type} {param.Name} = {param.Init};");

            Console.WriteLine();

            // create model
            Console.WriteLine($"    {modelInterface} model = new {modelClass}(");
            for (int i = 0; i < n; i++)
                Console.WriteLine($"        {p[i].Name}{(i < n - 1 ? "," : "")}");
            Console.WriteLine("    );");

            Console.WriteLine();

            Console.WriteLine($"    {className} expected = new {className}(model);");

            Console.WriteLine($"    {className} actual = new {className}(");

            for (int i = 0; i < n; i++)
            {
                bool isHash = (mask & (1 << i)) != 0;
                string value = isHash ? p[i].HashExpr : p[i].Name;

                Console.WriteLine($"        {value}{(i < n - 1 ? "," : "")}");
            }

            Console.WriteLine("    );");
            Console.WriteLine();
            Console.WriteLine("    Assert.True(expected.SequenceEqual(actual));");
            Console.WriteLine("}");
            Console.WriteLine();
        }
    }

    static string MaskName(int mask, Param[] p)
    {
        string name = "";

        for (int i = 0; i < p.Length; i++)
        {
            if ((mask & (1 << i)) != 0)
                name += UpperFirst(p[i].Name) + "Hash";
        }

        return name == "" ? "Values" : name;
    }

    static string UpperFirst(string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    class Param
    {
        public string Type;
        public string Name;
        public string Init;
        public string HashExpr;

        public Param(string t, string n, string init, string h)
        {
            Type = t;
            Name = n;
            Init = init;
            HashExpr = h;
        }
    }
}