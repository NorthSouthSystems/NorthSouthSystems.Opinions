public class T_ConvertXRequest
{
    [Theory]
    [InlineData(typeof(object), true)]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(bool), false)]
    [InlineData(typeof(int), false)]
    [InlineData(typeof(bool?), true)]
    [InlineData(typeof(int?), true)]
    public void ConversionTypeAllowsNull(Type conversionType, bool expectedAllowsNull) =>
        new ConvertXRequest(null, conversionType, InvariantCulture)
            .ConversionTypeAllowsNull
            .Should()
            .Be(expectedAllowsNull);

    [Fact]
    public void ExceptionToThrow()
    {
        var request = new ConvertXRequest("1", typeof(int), InvariantCulture);
        Exception exceptionToThrow;

        exceptionToThrow = request.ExceptionToThrow();
        exceptionToThrow.GetType().Should().Be<NotSupportedException>();

        var innerException1 = new ApplicationException();
        request.Exception(innerException1);

        exceptionToThrow = request.ExceptionToThrow();
        exceptionToThrow.GetType().Should().Be<InvalidCastException>();
        exceptionToThrow.InnerException.Should().Be(innerException1);

        var innerException2 = new ApplicationException();
        request.Exception(innerException2);

        exceptionToThrow = request.ExceptionToThrow();
        exceptionToThrow.GetType().Should().Be<AggregateException>();
        ((AggregateException)exceptionToThrow).InnerExceptions.Should().Equal(innerException1, innerException2);
    }

    [Fact]
    public void ArgumentExceptions()
    {
        Action act;
        var request = new ConvertXRequest("1", typeof(int), InvariantCulture);

        act = () => request.Exception(null);
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}
