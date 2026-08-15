namespace NorthSouthSystems;

public class ConvertXRequest
{
    internal ConvertXRequest(object? value, Type conversionType, CultureInfo culture)
    {
        Value = value;
        ConversionType = Throw.IfNull(conversionType);
        Culture = Throw.IfNull(culture);
    }

    public object? Value { get; }
    public Type ConversionType { get; }
    public CultureInfo Culture { get; }

    public bool ConversionTypeAllowsNull => !ConversionType.IsValueType || ConversionType.IsGenericNullable();

    public bool IsConverted { get; private set; }
    public object? ConvertedValue { get; private set; }

    public void Converted(object? convertedValue)
    {
        IsConverted = true;
        ConvertedValue = convertedValue;
    }

    // Don't create unneccessary garbage; instantiate when the first Exception is added.
    private List<Exception>? _exceptions;

    internal void Exception(Exception exception)
    {
        Throw.IfNull(exception);

        _exceptions ??= new();
        _exceptions.Add(exception);
    }

    internal Exception ExceptionToThrow()
    {
        string message = string.Create(InvariantCulture, $"{Value?.GetType().FullName} : {ConversionType.FullName}");

        if (_exceptions == null)
            return new NotSupportedException(message);
        else if (_exceptions.Count == 1)
            return new InvalidCastException(message, _exceptions.Single());
        else
            return new AggregateException(message, _exceptions);
    }
}
