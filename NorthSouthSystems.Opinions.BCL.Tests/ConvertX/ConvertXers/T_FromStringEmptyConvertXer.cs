using static T_ConvertXer;

public class T_FromStringEmptyConvertXer
{
    private static readonly FromStringEmptyConvertXer Converter = new();

    [Fact]
    public void IsConvertedTrueNoOp()
    {
        var request = Convert(Converter, string.Empty, typeof(string));

        request.IsConverted.Should().BeTrue();
        request.ConvertedValue.As<string>().Should().BeEmpty();
    }

    [Theory]
    [InlineData(typeof(int?))]
    [InlineData(typeof(DayOfWeek?))]
    [InlineData(typeof(IConvertXer))]
    public void IsConvertedTrueToNull(Type conversionType)
    {
        var request = Convert(Converter, string.Empty, conversionType);

        request.IsConverted.Should().BeTrue();
        request.ConvertedValue.Should().BeNull();
    }

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(DayOfWeek))]
    public void IsConvertedFalse(Type conversionType)
    {
        var request = Convert(Converter, null, conversionType);

        request.IsConverted.Should().BeFalse();
        request.ConvertedValue.Should().BeNull();
    }
}
