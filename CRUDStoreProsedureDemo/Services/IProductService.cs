using CRUDStoreProsedureDemo.Models;

namespace CRUDStoreProsedureDemo.Services
{
    public interface IProductService
    {
        Task InsertAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<Product> GetAsync(int id);
        Task<IReadOnlyList<Product>> ListAsync();
    }
}
