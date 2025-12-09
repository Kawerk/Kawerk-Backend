using Kawerk.Infastructure.DTOs.Branch;

namespace Kawerk.Application.Interfaces
{
    public interface IBranchSevice
    {
        public Task<int> CreateBranch(BranchCreationDTO branch);
        public Task<int> UpdateBranch(Guid branchID,BranchUpdateDTO branch);
        public Task<int> DeleteBranch(Guid branchID);
        public Task<BranchViewDTO?> GetBranch(Guid branchID);
        public Task<List<BranchViewDTO>?> GetBranches();
    }
}
