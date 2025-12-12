using Kawerk.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Kawerk.Infastructure.DTOs.Vehicle;
using System.ComponentModel.DataAnnotations;
using Kawerk.Infastructure.Context;
using Kawerk.Infastructure.DTOs.Customer;
using Kawerk.Infastructure.ResponseClasses;
using Kawerk.Application.Interfaces;

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
        public async Task<SettersResponse> BuyVehicle(Guid customerID, Guid vehicleID)
        {
            if (customerID == Guid.Empty || vehicleID == Guid.Empty)
                return new SettersResponse { status = 0, msg = "Invalid identifiers." };

            // Load buyer and vehicle with seller/manufacturer navigations
            var buyer = await _db.Customers.FindAsync(customerID);
            if (buyer == null)
                return new SettersResponse { status = 0, msg = "Buyer not found." };

            var vehicle = await _db.Vehicles
                .Include(v => v.Seller)
                .Include(v => v.Manufacturer)
                .FirstOrDefaultAsync(v => v.VehicleID == vehicleID);

            if (vehicle == null)
                return new SettersResponse { status = 0, msg = "Vehicle not found." };

            if (vehicle.BuyerID != null)
                return new SettersResponse { status = 0, msg = "Vehicle already sold." };

            if (vehicle.SellerID == customerID)
                return new SettersResponse { status = 0, msg = "Seller trying to buy his own car" };

            // Use a DB transaction to avoid races
            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // Re-check inside transaction in case of concurrent changes
                var currentVehicle = await _db.Vehicles
                    .AsTracking()
                    .FirstOrDefaultAsync(v => v.VehicleID == vehicleID);

                if (currentVehicle == null)
                    return new SettersResponse { status = 0, msg = "Vehicle not found." };

                if (currentVehicle.BuyerID != null)
                    return new SettersResponse { status = 0, msg = "Vehicle already sold." };

                // Create transaction record
                var transaction = new Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    Amount = currentVehicle.Price,
                    CreatedDate = DateTime.UtcNow,
                    Buyer = buyer,
                    BuyerID = buyer.CustomerID,
                    Vehicle = currentVehicle,
                    VehicleID = currentVehicle.VehicleID,
                    SellerCustomer = currentVehicle.Seller,
                    SellerCustomerID = currentVehicle.SellerID,
                    SellerManufacturer = null,
                    SellerManufacturerID = null
                };
                
                // Update vehicle state
                currentVehicle.BuyerID = buyer.CustomerID;
                currentVehicle.Buyer = buyer;
                currentVehicle.Status = "Sold";
                currentVehicle.Transaction = transaction;

                _db.Transactions.Add(transaction);
                _db.Vehicles.Update(currentVehicle);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return new SettersResponse { status = 1, msg = "Purchase completed successfully." };
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return new SettersResponse { status = 0, msg = $"Purchase failed: {ex.Message}" };
            }
        }
        public async Task<SettersResponse> SellVehicle(Guid sellerID, Guid vehicleID)
        {
            if (sellerID == Guid.Empty || vehicleID == Guid.Empty)
                return new SettersResponse { status = 0, msg = "Invalid identifiers." };

            var seller = await _db.Customers.FindAsync(sellerID);
            if (seller == null)
                return new SettersResponse { status = 0, msg = "Seller not found." };

            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.VehicleID == vehicleID);
            if (vehicle == null)
                return new SettersResponse { status = 0, msg = "Vehicle not found." };

            if (vehicle.BuyerID != null)
                return new SettersResponse { status = 0, msg = "Vehicle already sold; cannot list." };

            // If already listed by same seller, nothing to change
            if (vehicle.SellerID == sellerID && vehicle.Status == "Available")
                return new SettersResponse { status = 0, msg = "Vehicle is already listed by this seller." };

            // Assign seller and mark as available
            vehicle.SellerID = seller.CustomerID;
            vehicle.Seller = seller;
            vehicle.Status = "Available";
            vehicle.Transaction = null;
            //Checking if the car being sold was brought by the seller
            if (seller.VehiclesBought.Any(v=>v.VehicleID == vehicle.VehicleID))
                seller.VehiclesBought.Remove(vehicle);

            _db.Customers.Update(seller);
            _db.Vehicles.Update(vehicle);
            await _db.SaveChangesAsync();

            return new SettersResponse { status = 1, msg = "Vehicle listed for sale successfully." };
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
        public async Task<List<VehicleViewDTO>?> GetBoughtVehicles(Guid customerID)
        {
            var vehicles = await (from v in _db.Vehicles
                                  where v.BuyerID == customerID
                                  select new VehicleViewDTO
                                  {
                                      VehicleID = v.VehicleID,
                                      Name = v.Name,
                                      Description = v.Description,
                                      Price = v.Price,
                                      Type = v.Type,
                                      EngineCapacity = v.EngineCapacity,
                                      FuelType = v.FuelType,
                                      Images = v.Images,
                                      SeatingCapacity = v.SeatingCapacity,
                                      Status = v.Status,
                                      Transmission = v.Transmission,
                                      Year = v.Year
                                  }).ToListAsync();
            return vehicles;
        }
        public async Task<List<VehicleSellerViewDTO>?> GetSoldVehicles(Guid customerID)
        {
            var vehicles = await (from v in _db.Vehicles
                                  where v.SellerID == customerID
                                  select new VehicleSellerViewDTO
                                  {
                                      SellerID = v.SellerID,
                                      VehicleID = v.VehicleID,
                                      Name = v.Name,
                                      Description = v.Description,
                                      Price = v.Price,
                                      Type = v.Type,
                                      EngineCapacity = v.EngineCapacity,
                                      FuelType = v.FuelType,
                                      Images = v.Images,
                                      SeatingCapacity = v.SeatingCapacity,
                                      Status = v.Status,
                                  }).ToListAsync();
            return vehicles;
        }
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
