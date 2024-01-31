using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.Application.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CreateCategoryDto, CategoryModel>();
            CreateMap<UpdateCategoryDto, CategoryModel>();
            CreateMap<CategoryModel, CategoryDto>().ReverseMap();
        }
    }
}
