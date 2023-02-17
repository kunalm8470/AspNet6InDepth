namespace Api.Interfaces
{
    public interface IRandomValueService
    {
        Task<int> GetRandomIntegerAsync();
    }
}
