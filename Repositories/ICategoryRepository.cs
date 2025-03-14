using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab0002.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}