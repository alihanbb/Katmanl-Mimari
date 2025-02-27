using AppService.Products;

namespace AppService.Category
{
    public record CategoryWithProductDto (int CategoryId, string CategoryName,List<ProductDto> Products);
  
}
