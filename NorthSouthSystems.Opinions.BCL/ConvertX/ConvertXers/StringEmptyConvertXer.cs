namespace NorthSouthSystems;

public class StringEmptyConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        if (Throw.IfNull(request).Value is string { Length: 0 })
        {
            if (request.ConversionTypeFlattened == typeof(string))
                request.Converted(request.Value);
            else if (request.ConversionTypeAllowsNull)
                request.Converted(null);
        }
    }
}
