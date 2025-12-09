using Kawerk.Infastructure.DTOs.Branch;
using Kawerk.Infastructure.ResponseClasses;

namespace Kawerk.Application.Interfaces
{
    public interface IBranchSevice
    {
        public Task<SettersResponse> CreateBranch(BranchCreationDTO branch);
        public Task<SettersResponse> UpdateBranch(Guid branchID,BranchUpdateDTO branch);
        public Task<SettersResponse> DeleteBranch(Guid branchID);
        public Task<BranchViewDTO?> GetBranch(Guid branchID);
        public Task<List<BranchViewDTO>?> GetBranches();
    }
}
