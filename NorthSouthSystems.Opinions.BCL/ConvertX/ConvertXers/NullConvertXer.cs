namespace NorthSouthSystems;

public class NullConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        if (Throw.IfNull(request).Value == null && request.ConversionTypeAllowsNull)
            request.Converted(null);
    }
}
