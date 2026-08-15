namespace NorthSouthSystems;

public class FromStringEmptyConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        if (Throw.IfNull(request).Value is string { Length: 0 })
        {
            // This is already handled by IdentityConvertXer, which is a required converter; however, it is cheap
            // to duplicate here for clarity.
            if (request.ConversionTypeFlattened == typeof(string))
                request.Converted(request.Value);
            else if (request.ConversionTypeAllowsNull)
                request.Converted(null);
        }
    }
}
