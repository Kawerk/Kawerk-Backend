using Kawerk.Application.Interfaces;
using Kawerk.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using Kawerk.Infastructure.Context;
using Kawerk.Infastructure.DTOs.Customer;
using Kawerk.Infastructure.ResponseClasses;

namespace Kawerk.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DbBase _db;

        public CustomerService(DbBase db)
        {
            _db = db;
        }


        //        *********** Setters ***********
        public async Task<SettersResponse> CreateCustomer(CustomerCreationDTO customer)//0 == Faulty DTO || 1 == Invalid Email || 2 == Invalid Password || 3 == Customer already Exists || 4 == Customer created Succesfully
        {
            //Checking customerDTO validity
            if(customer == null)
                return new SettersResponse { status = 0, msg = "Faulty DTO" };

            if (!IsEmailValid(customer.Email))
                return new SettersResponse { status = 0, msg = "Invalid Email" };

            if(!await IsPasswordValid(customer.Password))
                return new SettersResponse { status = 0, msg = "Invalid Password" };

            //Checking if User already exists
            var isCustomerExists = await _db.Customers.AnyAsync(c=>c.Username.ToLower() == customer.Username.ToLower() ||
                                                     c.Email.ToLower() == customer.Email.ToLower());
            //If User exists return
            if (isCustomerExists)
                return new SettersResponse { status = 0, msg = "Customer already exists" };

            //Creating new Customer
            Customer newCustomer = new Customer
            {
                CustomerID = Guid.NewGuid(),
                Name = customer.Name,
                Username = customer.Username,
                Email = customer.Email,
                Password = new PasswordHasher<Customer>().HashPassword(null, customer.Password),
                CreatedAt = DateTime.Now,
            };

            //Saving to Database
            await _db.Customers.AddAsync(newCustomer);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Customer created successfully" };
        }
        public async Task<SettersResponse> UpdateCustomer(Guid customerID,CustomerUpdateDTO customer)//0 == Faulty DTO || 1 == Customer does not exist  || 2 == username is already used || 3 == Updated Successful
        {
            //Checking DTO validity
            if (customer == null)
                return new SettersResponse { status = 0, msg = "Faulty DTO" };

            //Checking if User Exists
            var isCustomerExists = await (from c in _db.Customers
                                          where c.CustomerID == customerID
                                          select c).FirstOrDefaultAsync();
            //If User does not exist return
            if (isCustomerExists == null)
                return new SettersResponse { status = 0, msg = "Customer does not exist" };

            // --***Updating***--

            //If the user wants to change their username, we must first check if the username is in use
            if (!string.IsNullOrEmpty(customer.Username))
            {
                //If username is not in use then we change to it
                if (customer.Username.ToLower() == isCustomerExists.Username.ToLower() || !await isUsernameValid(customer.Username))
                    isCustomerExists.Username = customer.Username;
                //If it is in user we return
                else
                    return new SettersResponse { status = 0, msg = "Username is already used" };
            }
            //Updating Phone field
            if(!string.IsNullOrEmpty(customer.Phone))
                isCustomerExists.Phone = customer.Phone;   
            //Updating Address field
            if(!string.IsNullOrEmpty(customer.Address))
                isCustomerExists.Address = customer.Address;
            //Updating City field
            if (!string.IsNullOrEmpty(customer.City)) 
                isCustomerExists.City = customer.City;
            //Updating Country field
            if(!string.IsNullOrEmpty(customer.Country))
                isCustomerExists.Country = customer.Country;
            //Updating Profile picture
            if(!string.IsNullOrEmpty(customer.ProfileUrl))
                isCustomerExists.ProfileUrl = customer.ProfileUrl;

            //Saving to Database
            _db.Customers.Update(isCustomerExists);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Updated Successfully" };
        }
        public async Task<SettersResponse> DeleteCustomer(Guid customerID)
        {
            //Checking ID validity
            if (customerID == Guid.Empty)
                return new SettersResponse { status = 0, msg = "Faulty customerID" };

            //Getting curstomer from Database
            var isCustomerExists = await (from  c in _db.Customers
                                          where c.CustomerID == customerID
                                          select c).FirstOrDefaultAsync();
            //If customer not found return
            if (isCustomerExists == null)
                return new SettersResponse { status = 0, msg = "Customer not found" };

            //Saving to Database
            _db.Customers.Remove(isCustomerExists);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Customer Deleted Successfully" };
        }

        //-----------------------------------------------------------------------


        //        *********** Extra Validation Function ***********

        public async Task<bool> isUsernameValid(string username)
        {
            var isUsernameExists = await _db.Customers.AnyAsync(c=>c.Username == username);
            return isUsernameExists;
        } 
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
        public async Task<CustomerViewDTO?> GetCustomer(Guid customerID)
        {
            //Getting customer from Database and projecting to CustomerDTO
            var customer = await (from c in _db.Customers
                                  where c.CustomerID == customerID
                                  select new CustomerViewDTO
                                  {
                                      CustomerID = customerID,
                                      Name = c.Name,
                                      Username = c.Username,
                                      Address = c.Address,
                                      City = c.City,
                                      Country = c.Country,
                                      Phone = c.Phone,
                                      ProfileUrl = c.ProfileUrl
                                  }).FirstOrDefaultAsync();

            //Returning Customer
            return customer;
        }
        public async Task<List<CustomerViewDTO>?> GetCustomers()
        {
            //Getting customers from Database and projecting to CustomerDTO
            var customersQuery = await(from c in _db.Customers
                                 select new CustomerViewDTO
                                 {
                                     CustomerID = c.CustomerID,
                                     Name = c.Name,
                                     Username = c.Username,
                                     Address = c.Address,
                                     City = c.City,
                                     Country = c.Country,
                                     Phone = c.Phone,
                                     ProfileUrl = c.ProfileUrl
                                 }).ToListAsync();

            return customersQuery;
        }

    }
}
