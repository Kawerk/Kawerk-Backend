using Kawerk.Application.Interfaces;
using Kawerk.Domain;
using Kawerk.Infastructure.Context;
using Kawerk.Infastructure.DTOs.Transaction;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Kawerk.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DbBase _db;
        public TransactionService(DbBase db)
        {
            _db = db;
        }
        public async Task<int> CreateTransaction(TransactionCreationDTO transaction)
        {
            if (transaction == null)
                return 0;

            if (transaction.SellerCustomerID != Guid.Empty)//0 == Faulty DTO || 1 == emtpy SellerID and ManufacturerID || 2 == Seller not found || 3 == Manufacturer not found || 4 == Customer not found || 5 == Vehicle not found || 6 == Successful
            {
                var isSellerExists = await (from c in _db.Customers
                                              where c.CustomerID == transaction.SellerCustomerID
                                              select c).FirstOrDefaultAsync();
                if (isSellerExists == null)
                    return 2;
                var isCustomerExists = await (from c in _db.Customers
                                                where c.CustomerID == transaction.BuyerID
                                                select c).FirstOrDefaultAsync();
                if(isCustomerExists == null)
                    return 4;

                var isVehicleExists = await (from v in _db.Vehicles
                                             where v.VehicleID == transaction.VehicleID
                                             select v).FirstOrDefaultAsync();
                if (isVehicleExists == null)
                    return 5;

                Domain.Transaction newTransaction = new Domain.Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    Buyer = isCustomerExists,
                    SellerCustomer = isSellerExists,
                    Amount = transaction.Amount,
                    CreatedDate = DateTime.Now,
                    Vehicle = isVehicleExists
                };
                await _db.Transactions.AddAsync(newTransaction);
                await _db.SaveChangesAsync();
                return 6;
            }
            else if (transaction.SellerManufacturerID != Guid.Empty)
            {
                var isManufacturerExists = await (from m in _db.Manufacturers
                                            where m.ManufacturerID == transaction.SellerManufacturerID
                                            select m).FirstOrDefaultAsync();
                if (isManufacturerExists == null)
                    return 3;

                var isCustomerExists = await (from c in _db.Customers
                                              where c.CustomerID == transaction.BuyerID
                                              select c).FirstOrDefaultAsync();
                if (isCustomerExists == null)
                    return 4;

                var isVehicleExists = await (from v in _db.Vehicles
                                             where v.VehicleID == transaction.VehicleID
                                             select v).FirstOrDefaultAsync();
                if (isVehicleExists == null)
                    return 5;

                Domain.Transaction newTransaction = new Domain.Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    Buyer = isCustomerExists,
                    SellerManufacturer = isManufacturerExists,
                    Amount = transaction.Amount,
                    CreatedDate = DateTime.Now,
                    Vehicle = isVehicleExists
                };
                await _db.Transactions.AddAsync(newTransaction);
                await _db.SaveChangesAsync();
                return 6;
            }
            else
                return 1;
        }

        public async Task<int> DeleteTransaction(Guid transactionID)
        {
            if (transactionID == Guid.Empty)
                return 0;
            var isTransactionExists = await (from t in _db.Transactions
                                     where t.TransactionID == transactionID
                                     select t).FirstOrDefaultAsync();

            if (isTransactionExists == null)
                return 1;

            _db.Transactions.Remove(isTransactionExists);
            await _db.SaveChangesAsync();
            return 2;
        }

        public Task<TransactionViewDTO> GetTransaction(Guid transactionID)
        {
            throw new NotImplementedException();
        }

        public Task<List<TransactionViewDTO>> GetTransactions()
        {
            throw new NotImplementedException();
        }
    }
}
