using System.Collections.Generic;
using System.Threading.Tasks;
using Lab0002.Models; 

public interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Product GetById(int id);
    Task AddAsync(Product product); 
    Task UpdateAsync(Product product);
    void Delete(int id);
}