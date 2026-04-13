namespace TestGenerator;

public class Param
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
