using Kawerk.Infastructure.DTOs.Transaction;
using Kawerk.Infastructure.ResponseClasses;

namespace Kawerk.Application.Interfaces
{
    public interface ITransactionService
    {
        public Task<SettersResponse> CreateTransaction(TransactionCreationDTO transaction);
        public Task<SettersResponse> DeleteTransaction(Guid transactionID);
        public Task<TransactionViewDTO?> GetTransaction(Guid transactionID);
        public Task<List<TransactionViewDTO>?> GetTransactions();

    }
}
