public abstract class T_ConvertXer<T>
    where T : IConvertXer, new()
{
    private readonly T _converter = new();

    protected ConvertXRequest Convert(object value, Type conversionType, CultureInfo culture = null)
    {
        var request = new ConvertXRequest(value, conversionType, culture ?? CurrentCulture);
        _converter.Convert(request);

        return request;
    }
}
