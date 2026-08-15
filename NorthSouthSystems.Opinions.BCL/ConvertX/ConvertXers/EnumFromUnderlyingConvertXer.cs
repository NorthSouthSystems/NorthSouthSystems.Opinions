namespace NorthSouthSystems;

public class EnumFromUnderlyingConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        var conversionType = Throw.IfNull(request).ConversionType.FlattenGenericNullable();

        if (request.Value != null && request.Value.GetType().CanBeEnumUnderlyingType() && conversionType.IsEnum)
            request.Converted(Enum.ToObject(conversionType, request.Value));
    }
}
