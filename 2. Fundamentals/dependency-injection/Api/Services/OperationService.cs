using Api.Interfaces;

namespace Api.Services
{
    public class OperationService : IOperation, ISingletonOperation, IScopedOperation, ITransitiveOperation
    {
        private Guid _operationId;

        public OperationService(Guid operationId) 
        {
            _operationId = operationId;
            // 2.
        }

        public OperationService() : this(Guid.NewGuid())
        {
            // 1.
        }

        public Guid OperationId
        {
            get
            {
                // 3.
                return _operationId;
            }
            set
            {
                 _operationId = value;
            }
        }
    }
}
