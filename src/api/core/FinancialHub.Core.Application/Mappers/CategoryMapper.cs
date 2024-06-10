using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.Application.Mappers
{
    internal class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CreateCategoryDto, CategoryModel>();
            CreateMap<UpdateCategoryDto, CategoryModel>();
            CreateMap<CategoryDto, CategoryModel>().ReverseMap();
        }
    }
}
