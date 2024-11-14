namespace Attelas.Utility;

public static class ArgumentValidator
{
    public static void RequireRange(int value, int minValue, int maxValue, string argumentName)
    {
        if (value > maxValue || value < minValue)
        {
            throw new ArgumentException(string.Format("The value must be between {0} and {1}", minValue, maxValue),
                argumentName);
        }
    }

    public static void RequireNotNullOrEmpty(string value, object argumentName)
    {
        if (argumentName is null)
        {
            throw new ArgumentException("The value can't be null or empty", value);
        }
    }
    
}