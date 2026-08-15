namespace NorthSouthSystems;

public interface IConvertXer
{
    void Convert(ConvertXRequest request);
}

/// <summary>
/// ConvertX is used to compose pipelines of ITypeConverters capable of converting .NET objects of one Type to any other
/// Type. The default ConvertX conversion pipeline properly handles empty strings and Nullable of T Types for which
/// System.Convert throws Exceptions.
/// </summary>
public class ConvertX
{
    // Construction

    public static IEnumerable<IConvertXer> DefaultConverters { get; } =
    [
        new IdentityConvertXer(),
        new NullConvertXer(),
        new StringEmptyConvertXer(),
        new EnumFromUnderlyingConvertXer(),
        new SystemConvertConvertXer()
    ];

    // Order is important! Must be initialized after DefaultTypeConverters because they are used in the constructor.
    public static ConvertX Default { get; } = new();

    public ConvertX()
        : this(DefaultConverters)
    { }

    public ConvertX(params IConvertXer[] converters)
        : this((IEnumerable<IConvertXer>)converters)
    { }

    public ConvertX(IEnumerable<IConvertXer> converters)
    {
        // Always "make a copy" of the enumerable in case it is a modifiable collection.
        _converters = [.. Throw.IfNull(converters)];

        if (_converters.Any(c => c is null))
            throw new ArgumentNullException(nameof(converters));

        if (!_converters.Any())
            throw new ArgumentOutOfRangeException(nameof(converters));
    }

    private readonly IReadOnlyList<IConvertXer> _converters;

    // ConvertType Generic and Object

    public TConversionType? ConvertType<TConversionType>(object? value,
        IFormatProvider? provider = null, bool throwIntermediateExceptions = false)
    {
        var request = ConvertTypeImpl(value, typeof(TConversionType), provider, throwIntermediateExceptions, false);

        return request.IsConverted ? (TConversionType?)request.ConvertedValue : throw request.ExceptionToThrow();
    }

    public object? ConvertType(object? value, Type conversionType,
        IFormatProvider? provider = null, bool throwIntermediateExceptions = false)
    {
        var request = ConvertTypeImpl(value, conversionType, provider, throwIntermediateExceptions, false);

        return request.IsConverted ? request.ConvertedValue : throw request.ExceptionToThrow();
    }

    // TryConvertType Generic

    public bool TryConvertType<TConversionType>(object? value,
        out TConversionType? convertedValue) =>
        TryConvertType(value, null, false, out convertedValue);

    public bool TryConvertType<TConversionType>(object? value,
        IFormatProvider? provider, out TConversionType? convertedValue) =>
        TryConvertType(value, provider, false, out convertedValue);

    public bool TryConvertType<TConversionType>(object? value,
        bool abortIntermediateExceptions, out TConversionType? convertedValue) =>
        TryConvertType(value, null, abortIntermediateExceptions, out convertedValue);

    public bool TryConvertType<TConversionType>(object? value,
        IFormatProvider? provider, bool abortIntermediateExceptions, out TConversionType? convertedValue)
    {
        var request = ConvertTypeImpl(value, typeof(TConversionType), provider, false, abortIntermediateExceptions);

        convertedValue = request.IsConverted ? (TConversionType?)request.ConvertedValue : default;

        return request.IsConverted;
    }

    // TryConvertType Object

    public bool TryConvertType(object? value, Type conversionType,
        out object? convertedValue) =>
        TryConvertType(value, conversionType, null, false, out convertedValue);

    public bool TryConvertType(object? value, Type conversionType,
        IFormatProvider? provider, out object? convertedValue) =>
        TryConvertType(value, conversionType, provider, false, out convertedValue);

    public bool TryConvertType(object? value, Type conversionType,
        bool abortIntermediateExceptions, out object? convertedValue) =>
        TryConvertType(value, conversionType, null, abortIntermediateExceptions, out convertedValue);

    public bool TryConvertType(object? value, Type conversionType,
        IFormatProvider? provider, bool abortIntermediateExceptions, out object? convertedValue)
    {
        var request = ConvertTypeImpl(value, conversionType, provider, false, abortIntermediateExceptions);

        // We want to have complete parity with TryConvertType<TConversionType>, so whenever conversion didn't occur,
        // we use our extension method to "default" convertedValue instead of simply always setting null.
        convertedValue = request.IsConverted ? request.ConvertedValue : conversionType.Default();

        return request.IsConverted;
    }

    // Implementation

    private ConvertXRequest ConvertTypeImpl(object? value, Type conversionType,
        IFormatProvider? provider, bool throwIntermediateExceptions, bool abortIntermediateExceptions)
    {
        var request = new ConvertXRequest(value, conversionType, provider ?? CurrentCulture);

        foreach (var converter in _converters)
        {
            try
            {
                converter.Convert(request);

                if (request.IsConverted)
                    return request;
            }
            catch (Exception exception)
            {
                if (throwIntermediateExceptions)
                    throw;

                request.Exception(exception);

                if (abortIntermediateExceptions)
                    return request;
            }
        }

        return request;
    }
}
