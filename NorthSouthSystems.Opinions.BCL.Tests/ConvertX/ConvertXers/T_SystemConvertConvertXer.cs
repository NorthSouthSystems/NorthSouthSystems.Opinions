using static T_ConvertXer;

public class T_SystemConvertConvertXer
{
    private static readonly SystemConvertConvertXer Converter = new();

    [Theory]
    [InlineData("true", typeof(bool), true)]
    [InlineData("1", typeof(int), 1)]
    [InlineData("1", typeof(double), 1.0)]
    public void IsConvertedTrue(object value, Type conversionType, object expectedConvertedValue)
    {
        var request = Convert(Converter, value, conversionType);

        request.IsConverted.Should().BeTrue();
        request.ConvertedValue.Should().Be(expectedConvertedValue);
    }
}
