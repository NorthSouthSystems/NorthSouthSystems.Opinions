public class TNullConvertXer : T_ConvertXer<NullConvertXer>
{
    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(int?))]
    [InlineData(typeof(IConvertXer))]
    public void IsConvertedTrue(Type conversionType)
    {
        var request = Convert(null, conversionType);

        request.IsConverted.Should().BeTrue();
        request.ConvertedValue.Should().BeNull();
    }

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(DayOfWeek))]
    public void IsConvertedFalse(Type conversionType)
    {
        var request = Convert(null, conversionType);

        request.IsConverted.Should().BeFalse();
        request.ConvertedValue.Should().BeNull();
    }
}
