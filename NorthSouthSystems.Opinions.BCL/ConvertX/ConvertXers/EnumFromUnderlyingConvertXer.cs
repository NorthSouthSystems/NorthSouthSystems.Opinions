namespace NorthSouthSystems;

public class EnumFromUnderlyingConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        if (Throw.IfNull(request).Value is null)
            return;

        if (request.Value!.GetType().CanBeEnumUnderlyingType() && request.ConversionTypeFlattened.IsEnum)
            request.Converted(Enum.ToObject(request.ConversionTypeFlattened, request.Value));
    }
}
