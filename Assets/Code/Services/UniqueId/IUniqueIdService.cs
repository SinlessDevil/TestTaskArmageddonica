namespace Code.Services.UniqueId
{
    public interface IUniqueIdService
    {
        string GenerateUniqueId();
        string GenerateUniqueId(string prefix);
        string GenerateUniqueId(string prefix, int length);
    }
}
