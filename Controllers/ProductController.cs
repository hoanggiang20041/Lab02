using Lab0002.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public IActionResult Add()
    {
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Product product, IFormFile image)
    {
        if (ModelState.IsValid)
        {
            if (image != null && image.Length > 0)
            {
                product.ImageUrl = await SaveImage(image);
            }

            await _productRepository.AddAsync(product);
            return RedirectToAction("Index");
        }

        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View(product);
    }

    public IActionResult Index()
    {
        var products = _productRepository.GetAll();
        return View(products);
    }

    public IActionResult Update(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Product product, IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        var existingProduct = _productRepository.GetById(product.Id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        existingProduct.Name = product.Name ?? existingProduct.Name;
        existingProduct.Price = product.Price != 0 ? product.Price : existingProduct.Price;
        existingProduct.Description = product.Description ?? existingProduct.Description;
        existingProduct.CategoryId = product.CategoryId != 0 ? product.CategoryId : existingProduct.CategoryId;

        if (image != null && image.Length > 0)
        {
            existingProduct.ImageUrl = await SaveImage(image);
        }

        await _productRepository.UpdateAsync(existingProduct);
        return RedirectToAction("Index");
    }

    public IActionResult Display(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product); 
    }

    public IActionResult Delete(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product); 
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var existingProduct = _productRepository.GetById(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        _productRepository.Delete(id);
        return RedirectToAction("Index");
    }

    private async Task<string> SaveImage(IFormFile image)
    {
        var filePath = Path.Combine("wwwroot/images", image.FileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        return $"/images/{image.FileName}";
    }
}