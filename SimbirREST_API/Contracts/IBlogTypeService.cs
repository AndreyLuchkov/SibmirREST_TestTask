using EFDataAccess.Models;
using SimbirREST_API.Models;

namespace SimbirREST_API.Contracts
{
    public interface IBlogTypeService
    {
        public Task<int?> GetIdByNameAsync(string name);
    }
}
