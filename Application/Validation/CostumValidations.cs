namespace Application.Validation;

public class CostumValidations
{
    public static bool IsValueDefault<T>(T value) where T : IComparable
    {
        return !Equals(value, default(T));
    }
}