using Kawerk.Application.Interfaces;
using Kawerk.Domain;
using Kawerk.Infastructure.Context;
using Kawerk.Infastructure.DTOs.Salesman;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kawerk.Application.Services
{
    public class SalesmanService : ISalesmanService
    {
        private readonly DbBase _db;
        public SalesmanService(DbBase db)
        {
            _db = db;
        }

        //        *********** Setters ***********
        public async Task<int> CreateSalesman(SalesmanCreationDTO salesman)
        {
            //Checking DTO validity
            if (salesman == null)
                return 0;

            //Checking email validity
            if (!IsEmailValid(salesman.Email))
                return 1;

            //Checking Password validity
            if(!await IsPasswordValid(salesman.Password))
                return 2;

            //Getting branch from Database
            var isBranchExists = await (from b in _db.Branches
                                        where b.BranchID == salesman.branchID
                                        select b).FirstOrDefaultAsync();
            //If branch not found return
            if (isBranchExists == null)
                return 3;

            //Making the new salesman
            Salesman newSalesman = new Salesman
            {
                SalesmanID = Guid.NewGuid(),
                Name = salesman.Name,
                Email = salesman.Email,
                Password = new PasswordHasher<Salesman>().HashPassword(null, salesman.Password),
                CreatedAt = DateTime.UtcNow,
                Branch = isBranchExists // The branch the salesman will be working in
            };

            //Saving to Database
            await _db.Salesman.AddAsync(newSalesman);
            await _db.SaveChangesAsync();
            return 4;
        }
        public async Task<int> UpdateSalesman(Guid salesmanID, SalesmanUpdateDTO salesman)
        {
            //Checking DTO validity
            if (salesman == null)
                return 0;

            //Getting salesman from Database
            var isSalesmanExists = await (from s in _db.Salesman
                                          where s.SalesmanID == salesmanID
                                          select s).FirstOrDefaultAsync();
            //If not found return
            if (isSalesmanExists == null)
                return 1;

            //Updating name
            if(!string.IsNullOrEmpty(salesman.Name))
                isSalesmanExists.Name = salesman.Name;
            //Updating Phone
            if(!string.IsNullOrEmpty(salesman.Phone))
                isSalesmanExists.Phone = salesman.Phone;
            //Updating Address
            if(!string.IsNullOrEmpty(salesman.Address))
                isSalesmanExists.Address = salesman.Address;
            //Updating City
            if(!string.IsNullOrEmpty(salesman.City))
                isSalesmanExists.City = salesman.City;
            //Updating Country
            if(!string.IsNullOrEmpty(salesman.Country))
                isSalesmanExists.Country = salesman.Country;
            //Updating Salary
            if(salesman.Salary != 0)
                isSalesmanExists.Salary = salesman.Salary;

            //Savint to Database
            _db.Salesman.Update(isSalesmanExists);
            await _db.SaveChangesAsync();
            return 2;
        }
        public async Task<int> DeleteSalesman(Guid salesmanID)
        {
            //Checking ID validity
            if (salesmanID == Guid.Empty)
                return 0;

            //Getting salesman from Database
            var isSalesmanExists = await (from s in _db.Salesman
                                          where s.SalesmanID == salesmanID
                                          select s).FirstOrDefaultAsync();
            //If not found return
            if (isSalesmanExists == null)
                return 1;

            //Saving to Database
            _db.Salesman.Remove(isSalesmanExists);
            await _db.SaveChangesAsync();
            return 2;
        }
        //-----------------------------------------------------------------------


        //        *********** Extra Validation Function ***********

        public bool IsEmailValid(string email)
        {
            if (new EmailAddressAttribute().IsValid(email) && email != null)
            {
                return true;
            }
            else return false;
        }
        public async Task<bool> IsPasswordValid(string password)
        {
            var PasswordPolicy = new Microsoft.AspNet.Identity.PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var result = await PasswordPolicy.ValidateAsync(password);
            if (result.Succeeded)
                return true;
            else
                return false;
        }

        //-----------------------------------------------------------------------

        //        *********** Getters ***********
        public async Task<SalesmanViewDTO?> GetSalesman(Guid salesmanID)
        {
            if (salesmanID == Guid.Empty)
                return null;

            var isSalesmanExist = await (from s in _db.Salesman
                                         where s.SalesmanID == salesmanID
                                         select new SalesmanViewDTO
                                         {
                                             SalesmanID = salesmanID,
                                             Email = s.Email,
                                             Phone = s.Phone,
                                             Address = s.Address,
                                             City = s.City,
                                             Country = s.Country,
                                             Salary = s.Salary,
                                             CreatedAt = s.CreatedAt,
                                         }).FirstOrDefaultAsync();
            return isSalesmanExist;
        }

        public async Task<List<SalesmanViewDTO>> GetSalesmen()
        {
            var salesmenQuery = await(from s in _db.Salesman
                                        select new SalesmanViewDTO
                                        {
                                            SalesmanID = s.SalesmanID,
                                            Email = s.Email,
                                            Phone = s.Phone,
                                            Address = s.Address,
                                            City = s.City,
                                            Country = s.Country,
                                            Salary = s.Salary,
                                            CreatedAt = s.CreatedAt,
                                        }).ToListAsync();
            return salesmenQuery;
        }

    }
}
