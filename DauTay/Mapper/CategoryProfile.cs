using AutoMapper;
using DauTay.DTO.Category.ProductType;
using DauTay.Entities.Category;

namespace DatPhat.API.Mapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        #region category
        CreateMap<ProductType, ProductTypeDTO>().ReverseMap();
        CreateMap<ProductType, CreateProductTypeDTO>().ReverseMap();
        #endregion
    }
}
