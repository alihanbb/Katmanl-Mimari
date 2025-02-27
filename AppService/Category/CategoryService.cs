using System.Net;
using AppRepository.Categories;
using AppRepository.Products;
using AppRepository.UnitOfWorks;
using AppService;
using AppService.Category.Create;
using AppService.Category.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppService.Category
{
    public class CategoryService (ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapping) : ICategoryService
    {
        public async Task<ServiceResult<int>> Create(CreateCategoryRequest categoryRequest)
        {
            var anyCategory = await categoryRepository.where(x => x.CategoryName == categoryRequest.CategoryName).AnyAsync();
            if (anyCategory)
            {
                return ServiceResult<int>.Faild("Category already exists", HttpStatusCode.NotFound);
            }

            var newCategory = new AppRepository.Categories.Category { CategoryName = categoryRequest.CategoryName };
           
            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<int>.Succes(newCategory.CategoryId);  
        }

        public async Task<ServiceResult> Update(UpdateCategoryRequest categoryRequest)
        {
            var anyCategory = await categoryRepository.GetByIdAsync(categoryRequest.CategoryId);
            if (anyCategory is null)
            {
                return ServiceResult.Faild("Category not found", HttpStatusCode.NotFound);
            }
           
            anyCategory.CategoryName = categoryRequest.CategoryName;
            categoryRepository.Update(anyCategory);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Succes(HttpStatusCode.NoContent);
        }

    }
}

