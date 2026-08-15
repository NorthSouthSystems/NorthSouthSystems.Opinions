public class T_ConvertX_ArgumentExceptions
{
    [Fact]
    public void ArgumentExceptions()
    {
        Action act;

        act = () => new ConvertX((IEnumerable<IConvertXer>)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new ConvertX(new FromStringEmptyConvertXer(), null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => ConvertX.Default.ConvertType("", null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => ConvertX.Default.TryConvertType("", null, out _);
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}
