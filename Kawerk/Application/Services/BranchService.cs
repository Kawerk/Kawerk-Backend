using Kawerk.Application.Interfaces;
using Kawerk.Domain;
using Kawerk.Infastructure.Context;
using Kawerk.Infastructure.DTOs.Branch;
using Kawerk.Infastructure.DTOs.Salesman;
using Kawerk.Infastructure.ResponseClasses;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kawerk.Application.Services
{
    public class BranchService : IBranchSevice
    {
        private readonly DbBase _db;
        public BranchService(DbBase db)
        {
            _db = db;
        }

        //        *********** Setters ***********
        public async Task<SettersResponse> CreateBranch(BranchCreationDTO branch)
        {
            //Checking DTO validation
            if (branch == null)
                return new SettersResponse { status = 0, msg = "Faulty DTO" };

            //Checking name and location uniqueness
            if(await isNameValid(branch.Name) || await isLocationValid(branch.Location))
                return new SettersResponse { status = 0, msg = "Name or location already in use" };

            //Creating new Branch
            Branches newBranch = new Branches
            {
                BranchID = Guid.NewGuid(),
                Name = branch.Name,
                Description = branch.Description,
                Location = branch.Location,
                CreatedAt = DateTime.UtcNow,
            };

            //Saving to Database
            await _db.Branches.AddAsync(newBranch);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Branch created successfully" };
        }
        public async Task<SettersResponse> UpdateBranch(Guid branchID, BranchUpdateDTO branch)
        {
            //Checking DTO & ID validity
            if (branchID == Guid.Empty || branch == null)
                return new SettersResponse { status = 0, msg = "Faulty DTO or ID" };

            //Getting branch from Database
            var isBranchExisting = await (from b in _db.Branches
                                          where b.BranchID == branchID
                                          select b).FirstOrDefaultAsync();
            //If Branch not found return
            if (isBranchExisting == null)
                return new SettersResponse { status = 0, msg = "Branch not found" };
            
            // --***Updating***--

            //If they want to change the name of the branch we have to check if the new name is in use or not
            if (!string.IsNullOrEmpty(branch.Name))
            {
                //if not in use update to new name
                if (!await isNameValid(branch.Name))
                    isBranchExisting.Name = branch.Name;
                //if in use return
                else
                    return new SettersResponse { status = 0, msg = "Name is already in use" };
            }
            //If they want to change the location of the branch we have to check if the new location is in use or not
            if (!string.IsNullOrEmpty(branch.Location))
            {
                //if not in use update to new location
                if (!await isLocationValid(branch.Location))
                    isBranchExisting.Location = branch.Location;
                //if in use return
                else
                    return new SettersResponse { status = 0, msg = "Location is already in use" };
            }
            //Updating Description
            if(!string.IsNullOrEmpty(branch.Description))
                isBranchExisting.Description = branch.Description;
            //Updating Warranty
            if(!string.IsNullOrEmpty(branch.Warranty))
                isBranchExisting.Warranty = branch.Warranty;

            //Saving to Database
            _db.Branches.Update(isBranchExisting);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Branch updated successfully" };

        }
        public async Task<SettersResponse> DeleteBranch(Guid branchID)
        {
            //Checking ID validity
            if (branchID == Guid.Empty)
                return new SettersResponse { status = 0, msg = "Faulty ID" };

            //Getting branch from Database
            var isBranchExisting = await (from b in _db.Branches
                                          where b.BranchID == branchID
                                          select b).FirstOrDefaultAsync();
            //if branch not found return
            if(isBranchExisting == null) 
                return new SettersResponse { status = 0, msg = "Branch not found" };

            //Saving to Database
            _db.Branches.Remove(isBranchExisting);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Branch deleted successfully" };
        }
        public async Task<SettersResponse> AddSalesman(Guid branchID, Guid salesmanID)
        {
            //Checking ID validity
            if (branchID == Guid.Empty || salesmanID == Guid.Empty)
                return new SettersResponse { status = 0, msg = "Faulty ID" };

            //Getting branch from Database
            var isBranchExisting = await (from b in _db.Branches.Include(b => b.Salesmen)
                                          where b.BranchID == branchID
                                          select b).FirstOrDefaultAsync();
            //if branch not found return
            if (isBranchExisting == null)
                return new SettersResponse { status = 0, msg = "Branch not found" };

            //Getting salesman from Database
            var isSalesmanExisting = await (from s in _db.Salesman
                                            where s.SalesmanID == salesmanID
                                            select s).FirstOrDefaultAsync();
            //if salesman not found return
            if (isSalesmanExisting == null)
                return new SettersResponse { status = 0, msg = "Salesman not found" };

            //Checking if salesman is already assigned to this branch
            if (isBranchExisting.Salesmen.Any(s => s.SalesmanID == salesmanID))
                return new SettersResponse { status = 0, msg = "Salesman already assigned to this branch" };

            //Adding salesman to branch
            isBranchExisting.Salesmen.Add(isSalesmanExisting);
            _db.Branches.Update(isBranchExisting);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Salesman added to branch successfully" };
        }
        public async Task<SettersResponse> RemoveSalesman(Guid branchID, Guid salesmanID)
        {
            //Checking ID validity
            if (branchID == Guid.Empty || salesmanID == Guid.Empty)
                return new SettersResponse { status = 0, msg = "Faulty ID" };

            //Getting branch from Database
            var isBranchExisting = await (from b in _db.Branches.Include(b => b.Salesmen)
                                          where b.BranchID == branchID
                                          select b).FirstOrDefaultAsync();
            //if branch not found return
            if (isBranchExisting == null)
                return new SettersResponse { status = 0, msg = "Branch not found" };

            //Getting salesman from Database
            var isSalesmanExisting = await (from s in _db.Salesman
                                            where s.SalesmanID == salesmanID
                                            select s).FirstOrDefaultAsync();
            //if salesman not found return
            if (isSalesmanExisting == null)
                return new SettersResponse { status = 0, msg = "Salesman not found" };

            //Checking if salesman is assigned to this branch
            if (!isBranchExisting.Salesmen.Any(s => s.SalesmanID == salesmanID))
                return new SettersResponse { status = 0, msg = "Salesman not assigned to this branch" };

            //Removing salesman from branch
            isBranchExisting.Salesmen.Remove(isSalesmanExisting);
            _db.Branches.Update(isBranchExisting);
            await _db.SaveChangesAsync();
            return new SettersResponse { status = 1, msg = "Salesman removed from branch successfully" };
        }
        //-----------------------------------------------------------------------


        //        *********** Extra Validation Function ***********

        public async Task<bool> isNameValid(string name)
        {
            var isNameExists = await _db.Branches.AnyAsync(b=>b.Name == name);
            return isNameExists;
        }
        public async Task<bool> isLocationValid(string location)
        {
            var isLocationValid = await _db.Branches.AnyAsync(b=>b.Location == location);
            return isLocationValid;
        }

        //-----------------------------------------------------------------------

        //        *********** Getters ***********

        public async Task<BranchViewDTO?> GetBranch(Guid branchID)
        {
            //Checking ID validity
            if (branchID == Guid.Empty)
                return null;

            //Getting branch from Database and projecting to BranchDTO 
            var isBranchExisting = await (from b in _db.Branches
                                          where b.BranchID == branchID
                                          select new BranchViewDTO
                                          {
                                              BranchID = b.BranchID,
                                              Name = b.Name,
                                              Description = b.Description,
                                              Location = b.Location,
                                              CreatedAt = b.CreatedAt
                                          }).FirstOrDefaultAsync();
            //returning result
            return isBranchExisting;
        }
        public async Task<List<SalesmanViewDTO>?> GetBranchSalesmen(Guid branchID)
        {
            if(branchID == Guid.Empty)
                return null;
            //Getting branch salesmen from Database and projecting to SalesmanDTO
            var salesmenQuery = await (from b in _db.Branches
                                       where b.BranchID == branchID
                                       from s in b.Salesmen
                                       select new SalesmanViewDTO
                                       {
                                           SalesmanID = s.SalesmanID,
                                           Name = s.Name,
                                           Email = s.Email,
                                           Password = s.Password,
                                           Phone = s.Phone,
                                           Salary = s.Salary,
                                           Address = s.Address,
                                           City = s.City,
                                           Country = s.Country,
                                           CreatedAt = s.CreatedAt
                                       }).ToListAsync();
            //returning result
            return salesmenQuery;
        }
        public async Task<List<BranchViewDTO>?> GetBranches()
        {
            //Getting branches from Database and projecting to BranchDTO
            var branchQuery = await(from b in _db.Branches
                                         select new BranchViewDTO
                                         {
                                             BranchID = b.BranchID,
                                             Name = b.Name,
                                             Description = b.Description,
                                             Location = b.Location,
                                             CreatedAt = b.CreatedAt
                                         }).ToListAsync();
            //returning result
            return branchQuery;
        }
    }
}
