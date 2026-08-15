namespace NorthSouthSystems;

public class IdentityConvertXer : IConvertXer
{
    public void Convert(ConvertXRequest request)
    {
        // All Nullable<T> instances box as their UnderlyingType.
        if (Throw.IfNull(request).Value?.GetType() == request.ConversionType.FlattenGenericNullable())
            request.Converted(request.Value);
    }
}
