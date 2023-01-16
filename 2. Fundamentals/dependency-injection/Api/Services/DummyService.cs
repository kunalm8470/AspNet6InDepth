using Api.Interfaces;

namespace Api.Services
{
    public class DummyService : IDummyService
    {
        private readonly IScopedOperation _scopedOperation;
        private readonly ITransitiveOperation _transitiveOperation;
        private readonly ILogger<DummyService> _logger;

        public DummyService(
            IScopedOperation scopedOperation,
            ITransitiveOperation transitiveOperation,
            ILogger<DummyService> logger
        )
        {
            _scopedOperation = scopedOperation;
            _transitiveOperation = transitiveOperation;
            _logger = logger;
        }

        public void PrintOperationId()
        {
            _logger.LogInformation("Scoped OperationId ", _scopedOperation.OperationId);
            _logger.LogInformation("Transitive OperationId ", _transitiveOperation.OperationId);
        }
    }
}
