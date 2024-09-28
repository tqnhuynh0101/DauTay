using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DauTay.Entities.Base;

/// <summary>
/// Data cơ bản của 1 entity
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntityBase
{
    string Id { get; set; }

    bool IsActive { get; set; }
    string CreatedBy { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    DateTime CreatedOn { get; set; }
    string UpdatedBy { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    [BsonDefaultValue(null)]
    DateTime? UpdatedOn { get; set; }
    int Status { get; set; }

    void SetCreator(string creator, DateTime dateTime);

    void SetUpdater(string updater, DateTime dateTime);
}

public abstract class EntityBase : IEntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [JsonIgnore]
    public bool IsActive { get; set; } = true;

    public string CreatedBy { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? UpdatedOn { get; set; }

    public int Status { get; set; } = 0;

    public void SetCreator(string creator, DateTime dateTime)
    {
        CreatedBy = creator;
        CreatedOn = dateTime;
    }

    public void SetUpdater(string updater, DateTime dateTime)
    {
        UpdatedBy = updater;
        UpdatedOn = dateTime;
    }

}

/// <summary>
/// Các entities cần có field CompanyId
/// </summary>
public interface IEntityBaseCompany
{
    [BsonRepresentation(BsonType.ObjectId)]
    string CompanyId { get; set; }
}

public abstract class EntityBaseCompany : EntityBase, IEntityBaseCompany
{
    [DisplayName("Công ty")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CompanyId { get; set; }
}
