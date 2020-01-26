namespace AlphaDev.Security
{
    public interface IJwtTokenBuilderFactory
    {
        IJwtTokenBuilder Create(TokenSettings settings);
    }
}