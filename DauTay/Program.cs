using DauTay.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCors(); // AllowMyOrigin
builder.Configuration.ConfigureDbContext(); //dùng lưu file cấu hình					
builder.Services.ConfigureRepositoryWrapper(); // rrepository partern
builder.Services.AddAutoMapper(typeof(Program)); //mapper profile

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Tùy chọn: Đặt Swagger UI ở trang gốc
    });
}

app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(
    c =>
    {
        c.InjectStylesheet("/css/swagger-ui/ui.css");// css path
        c.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Configuration["RedisSettings:InstanceName"]);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DisplayRequestDuration(); // test time response with cache :)
    }
);
app.MapControllers();
app.Run();
