using Kawerk.Application.Interfaces;
using Kawerk.Domain;
using Kawerk.Infastructure.Context;
using Kawerk.Infastructure.DTOs;
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
        public async Task<int> CreateBranch(BranchCreationDTO branch)//0 == Faulty DTO || 1 == name or location are already used || 2 == Successfull
        {
            //Checking DTO validation
            if (branch == null)
                return 0;

            //Checking name and location uniqueness
            if(await isNameValid(branch.Name) || await isLocationValid(branch.Location))
                return 1;

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
            return 2;
        }
        public async Task<int> UpdateBranch(Guid branchID, BranchUpdateDTO branch)//0 == Faulty DTO or ID || 1 == Branch not found || 2 == new name is in use || 3 == new location is in use || 4 == Successfull
        {
            //Checking DTO & ID validity
            if (branchID == Guid.Empty || branch == null)
                return 0;

            //Getting branch from Database
            var isBranchExisting = await (from b in _db.Branches
                                          where b.BranchID == branchID
                                          select b).FirstOrDefaultAsync();
            //If Branch not found return
            if (isBranchExisting == null)
                return 1;
            
            // --***Updating***--

            //If they want to change the name of the branch we have to check if the new name is in use or not
            if (!string.IsNullOrEmpty(branch.Name))
            {
                //if not in use update to new name
                if (!await isNameValid(branch.Name))
                    isBranchExisting.Name = branch.Name;
                //if in use return
                else
                    return 2;
            }
            //If they want to change the location of the branch we have to check if the new location is in use or not
            if (!string.IsNullOrEmpty(branch.Location))
            {
                //if not in use update to new location
                if (!await isLocationValid(branch.Location))
                    isBranchExisting.Location = branch.Location;
                //if in use return
                else
                    return 3;
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
            return 4;

        }
        public async Task<int> DeleteBranch(Guid branchID)//0 == Faulty ID || 1 == Branch not found || 2 == Successful
        {
            //Checking ID validity
            if (branchID == Guid.Empty)
                return 0;

            //Getting branch from Database
            var isBranchExisting = await (from b in _db.Branches
                                          where b.BranchID == branchID
                                          select b).FirstOrDefaultAsync();
            //if branch not found return
            if(isBranchExisting == null) 
                return 1;

            //Saving to Database
            _db.Branches.Remove(isBranchExisting);
            await _db.SaveChangesAsync();
            return 2;
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

        public async Task<BranchDTO?> GetBranch(Guid branchID)
        {
            //Checking ID validity
            if (branchID == Guid.Empty)
                return null;

            //Getting branch from Database and projecting to BranchDTO 
            var isBranchExisting = await (from b in _db.Branches
                                          where b.BranchID == branchID
                                          select new BranchDTO
                                          {
                                              BranchID = b.BranchID,
                                              Name = b.Name,
                                              Description = b.Description,
                                              Location = b.Location,
                                          }).FirstOrDefaultAsync();
            //returning result
            return isBranchExisting;
        }

        public async Task<List<BranchDTO>?> GetBranchs()
        {
            //Getting branches from Database and projecting to BranchDTO
            var branchQuery = await(from b in _db.Branches
                                         select new BranchDTO
                                         {
                                             BranchID = b.BranchID,
                                             Name = b.Name,
                                             Description = b.Description,
                                             Location = b.Location,
                                         }).ToListAsync();
            //returning result
            return branchQuery;
        }
    }
}
