public class T_ConvertX_ArgumentExceptions
{
    [Fact]
    public void ArgumentExceptions()
    {
        Action act;

        act = () => new ConvertX((IEnumerable<IConvertXer>)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new ConvertX(new IdentityConvertXer(), null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new ConvertX(Array.Empty<IConvertXer>());
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();

        act = () => new ConvertX().ConvertType("", null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new ConvertX().TryConvertType("", null, out _);
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}
