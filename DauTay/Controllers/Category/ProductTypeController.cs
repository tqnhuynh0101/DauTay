using AutoMapper;
using DatPhat.API.Controllers;
using DauTay.Common;
using DauTay.DTO.Category.ProductType;
using DauTay.Entities.Category;
using DauTay.Entities.Paging;
using DauTay.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DauTay.Controllers.Category;

[Route("api/[controller]")]
[ApiController]
public class ProductTypeController(IUnitOfWork unitOfWork, IMapper mapper) : HelpsController(unitOfWork, mapper)
{
    [HttpGet]
    [SwaggerOperation("Danh sách loại sản phẩm")]
    public async Task<IActionResult> Get()
    {
        List<ProductTypeDTO> res = [];
        List<ProductType> allCars = await _unitOfWork.Repository<ProductType>().GetAllAsync();
        res = _mapper.Map<List<ProductTypeDTO>>(allCars);
        return Ok(new Response<List<ProductTypeDTO>>(res, StaticMessageResponse.SUCCESS));
    }


    [HttpPost]
    [SwaggerOperation("Tạo mới loại sản phẩm")]
    public async Task<IActionResult> Create([FromBody] CreateProductTypeDTO productTypeCreate)
    {
        ProductType productType = _mapper.Map<ProductType>(productTypeCreate);

        _unitOfWork.Repository<ProductType>().Insert(productType);
        await _unitOfWork.CommitAsync();
        ProductTypeDTO res = _mapper.Map<ProductTypeDTO>(productType);
        return Ok(new Response<ProductTypeDTO>(res, StaticMessageResponse.SUCCESS));
    }


    [HttpPut("{id}")]
    [SwaggerOperation("Cập nhật loại sản phẩm")]
    public async Task<IActionResult> Update(string id, [FromBody] CreateProductTypeDTO productTypeCreate)
    {
        ProductType productType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
        if (productType == null)
        {
            return NotFound();
        }
        productType= _mapper.Map<ProductType>(productTypeCreate);
        _unitOfWork.Repository<ProductType>().Update(id, productType);
        await _unitOfWork.CommitAsync();
        ProductTypeDTO res = _mapper.Map<ProductTypeDTO>(productType);
        return Ok(new Response<ProductTypeDTO>(res, StaticMessageResponse.SUCCESS));
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Xóa loại sản phẩm")]
    public async Task<IActionResult> Delete(string id)
    {
        ProductType productType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
        if (productType == null)
        {
            return NotFound();
        }
        _unitOfWork.Repository<ProductType>().Delete(id);
        await _unitOfWork.CommitAsync();
        return Ok(new Response<string>(StaticMessageResponse.SUCCESS, $"Đã xóa loại sản phẩm {productType.Code}"));
    }
}
