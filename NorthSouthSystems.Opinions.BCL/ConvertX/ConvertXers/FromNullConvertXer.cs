namespace NorthSouthSystems;

internal class FromNullConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        if (Throw.IfNull(request).Value is null && request.ConversionTypeAllowsNull)
            request.Converted(null);
    }
}
