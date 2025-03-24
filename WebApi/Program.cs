using Core.Domain.Interfaces;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services au conteneur.

// Enregistrer les dépôts et services
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configurer le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();