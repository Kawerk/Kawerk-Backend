using Kawerk.Infastructure.DTOs.Transaction;

namespace Kawerk.Application.Interfaces
{
    public interface ITransactionService
    {
        public Task<int> CreateTransaction(TransactionCreationDTO transaction);
        public Task<int> DeleteTransaction(Guid transactionID);
        public Task<TransactionViewDTO> GetTransaction(Guid transactionID);
        public Task<List<TransactionViewDTO>> GetTransactions();

    }
}
