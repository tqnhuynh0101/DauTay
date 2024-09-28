using System.ComponentModel;

namespace DauTay.DTO.Category.ProductType
{
    public class CreateProductTypeDTO
    {
        //[DisplayName("Nhãn hàng")]
        //public string BrandId { get; set; }
        [DisplayName("Mã")]
        public string Code { get; set; }
        [DisplayName("Tên")]
        public string Name { get; set; }
    }
}
