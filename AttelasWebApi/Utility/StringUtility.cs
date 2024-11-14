namespace Attelas.Utility;

public static class StringUtility
{
    public static int CompareIgnoreCase(string a, string b)
    {
        return string.Compare(a, b, StringComparison.InvariantCultureIgnoreCase);
    }
    
    public static int CompareGuid(Guid value1, Guid value2)
    {
        return value1.ToString().CompareTo(value2.ToString());
    }
}