using System.ComponentModel;

namespace DauTay.DTO.Category.ProductType
{
    public class ProductTypeDTO
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        //[DisplayName("Nhãn hàng")]
        //public string BrandId { get; set; }

        [DisplayName("Mã")]
        public string Code { get; set; }
        [DisplayName("Tên")]
        public string Name { get; set; }
    }
}
