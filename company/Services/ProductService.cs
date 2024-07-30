using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService
{
    private readonly ProductDAO _productDAO;

    public ProductService(ProductDAO productDAO)
    {
        _productDAO = productDAO;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productDAO.GetAllProductsAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _productDAO.GetProductByIdAsync(id);
    }

    public async Task<Product> InsertProductAsync(Product product)
    {
        return await _productDAO.InsertProductAsync(product);
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        return await _productDAO.UpdateProductAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productDAO.DeleteProductAsync(id);
    }
}