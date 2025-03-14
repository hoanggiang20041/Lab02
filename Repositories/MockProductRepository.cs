using Lab0002.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MockProductRepository : IProductRepository
{
    private readonly List<Product> _products;

    public MockProductRepository()
    {
        _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000, Description = "A high-end laptop" },
            new Product { Id = 2, Name = "Desktop", Price = 800, Description = "A powerful desktop computer" }
        };
    }

    public IEnumerable<Product> GetAll()
    {
        return _products;
    }

    public Product GetById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return product ?? throw new InvalidOperationException("Product not found."); // Xử lý null
    }

    public async Task AddAsync(Product product)
    {
        await Task.Run(() =>
        {
            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        });
    }

    public async Task UpdateAsync(Product product)
    {
        await Task.Run(() =>
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name ?? existingProduct.Name;
                existingProduct.Price = product.Price != 0 ? product.Price : existingProduct.Price;
                existingProduct.Description = product.Description ?? existingProduct.Description;
                existingProduct.ImageUrl = product.ImageUrl ?? existingProduct.ImageUrl;
                existingProduct.CategoryId = product.CategoryId != 0 ? product.CategoryId : existingProduct.CategoryId;
            }
        });
    }


    public void Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _products.Remove(product);
        }
    }
}