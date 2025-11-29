using Kawerk.Infastructure.DTOs;

namespace Kawerk.Application.Interfaces
{
    public interface IBranchSevice
    {
        public Task<int> CreateBranch(BranchCreationDTO branch);
        public Task<int> UpdateBranch(Guid branchID,BranchUpdateDTO branch);
        public Task<int> DeleteBranch(Guid branchID);
        public Task<BranchDTO?> GetBranch(Guid branchID);
        public Task<List<BranchDTO>?> GetBranches();
    }
}
