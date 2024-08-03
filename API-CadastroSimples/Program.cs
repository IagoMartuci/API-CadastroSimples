using API_CadastroSimples.Business;
using API_CadastroSimples.Business.Implementations;
using API_CadastroSimples.Data;
using API_CadastroSimples.Repository;
using API_CadastroSimples.Repository.Implementations;
using API_CadastroSimples.Service;
using API_CadastroSimples.Service.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPessoasRepository, PessoasRepository>();
//builder.Services.AddScoped<IPessoasBusiness, PessoasBusiness>();
builder.Services.AddScoped<IPessoasService, PessoasService>();

// Configurando para os endpoints no Swagger ficarem todos em minúsculo
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();

// Adiciona o Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cadastro Simples API",
        Version = "v1",
        Description = "API para cadastro simples de pessoas.",
        Contact = new OpenApiContact
        {
            Name = "Iago Martuci",
            Url = new Uri("https://github.com/IagoMartuci")
        }
    });

    // Opcional: Adiciona suporte para comentários XML (para exibir na documentação do Swagger)
    // O XML fica em: API-CadastroSimples\API-CadastroSimples\obj\Debug\net8.0\API-CadastroSimples.xml
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cadastro Simples API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();