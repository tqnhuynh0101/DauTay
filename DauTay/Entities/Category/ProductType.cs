using DauTay.Entities.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DauTay.Entities.Category
{
    public class ProductType : EntityBase
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string BrandId { get; set; }

        [DisplayName("Mã")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Code { get; set; }
        [DisplayName("Tên")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }
    }

}
