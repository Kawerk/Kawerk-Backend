using Kawerk.Infastructure.DTOs;

namespace Kawerk.Application.Interfaces
{
    public interface ITransactionService
    {
        public Task<int> CreateTransaction(TransactionCreationDTO transaction);
        public Task<int> DeleteTransaction(Guid transactionID);
        public Task<TransactionDTO> GetTransaction(Guid transactionID);
        public Task<List<TransactionDTO>> GetTransactions();

    }
}
