namespace TestGenerator;

public static class TestGenerator
{
    public static void Generate(string className, string modelInterface, string modelClass, Param[] p)
    {
        int n = p.Length;
        int total = 1 << n;

        const string I1 = "    ";
        const string I2 = "        ";

        // --- 1. From model ---
        Console.WriteLine("[Fact]");
        Console.WriteLine("public void ProduceCorrectHashFromModel()");
        Console.WriteLine("{");

        foreach (var param in p)
            Console.WriteLine($"{I1}{param.Type} {param.Name} = {param.Init};");

        Console.WriteLine();

        Console.WriteLine($"{I1}{modelInterface} model = new {modelClass}(");
        for (int i = 0; i < n; i++)
        {
            string comma = i < n - 1 ? "," : "";
            Console.WriteLine($"{I2}{p[i].Name}{comma}");
        }
        Console.WriteLine($"{I1});");
        Console.WriteLine();

        Console.WriteLine($"{I1}{className} expected = new {className}(model);");
        Console.WriteLine($"{I1}{className} actual = new {className}(model);");
        Console.WriteLine();
        Console.WriteLine($"{I1}Assert.True(expected.SequenceEqual(actual));");
        Console.WriteLine("}");
        Console.WriteLine();

        // --- 2. All combinations ---
        for (int mask = 0; mask < total; mask++)
        {
            Console.WriteLine("[Fact]");
            Console.WriteLine($"public void ProduceCorrectHashFrom{MaskName(mask, p)}()");
            Console.WriteLine("{");

            foreach (var param in p)
                Console.WriteLine($"{I1}{param.Type} {param.Name} = {param.Init};");

            Console.WriteLine();

            Console.WriteLine($"{I1}{modelInterface} model = new {modelClass}(");
            for (int i = 0; i < n; i++)
            {
                string comma = i < n - 1 ? "," : "";
                Console.WriteLine($"{I2}{p[i].Name}{comma}");
            }
            Console.WriteLine($"{I1});");

            Console.WriteLine();

            Console.WriteLine($"{I1}{className} expected = new {className}(model);");

            Console.WriteLine($"{I1}{className} actual = new {className}(");
            for (int i = 0; i < n; i++)
            {
                bool isHash = (mask & (1 << i)) != 0;
                string value = isHash ? p[i].HashExpr : p[i].Name;

                string comma = i < n - 1 ? "," : "";
                Console.WriteLine($"{I2}{value}{comma}");
            }
            Console.WriteLine($"{I1});");

            Console.WriteLine();
            Console.WriteLine($"{I1}Assert.True(expected.SequenceEqual(actual));");
            Console.WriteLine("}");
            Console.WriteLine();
        }
    }

    static string MaskName(int mask, Param[] p)
    {
        if (mask == (1 << p.Length) - 1)
            return "Hashes";

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
}