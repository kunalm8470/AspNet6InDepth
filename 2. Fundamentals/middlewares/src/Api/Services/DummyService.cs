using Api.Interfaces;

namespace Api.Services
{
    public class DummyService : IDummyService
    {
        public int ReturnInt()
        {
            return 5;
        }
    }
}
