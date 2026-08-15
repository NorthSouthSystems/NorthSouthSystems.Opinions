namespace NorthSouthSystems;

public class ToStringConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        if (Throw.IfNull(request).ConversionTypeFlattened != typeof(string))
            return;

        if (request.Value is IFormattable formattable)
        {
            request.Converted(formattable.ToString(null, request.Culture));
            return;
        }

        request.Converted(request.Value!.ToString());
    }
}
