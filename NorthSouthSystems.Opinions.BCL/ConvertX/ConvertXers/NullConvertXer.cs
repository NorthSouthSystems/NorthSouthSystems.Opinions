namespace NorthSouthSystems;

internal class NullConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        if (Throw.IfNull(request).Value is null && request.ConversionTypeAllowsNull)
            request.Converted(null);
    }
}
