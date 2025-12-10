using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Interface;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Infrastructure.Data;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Services;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Profiles;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

Console.WriteLine($"Ambiente ativo: {builder.Environment.EnvironmentName}");

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPlanoInterface, PlanoService>();
builder.Services.AddScoped<IBeneficiarioInterface, BeneficiarioService>();

builder.Services.AddAutoMapper(typeof(PlanoProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod(); 
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
