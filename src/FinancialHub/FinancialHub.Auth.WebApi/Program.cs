using FinancialHub.Auth.Services.Extensions;
using FinancialHub.Auth.Resources.Extensions;
using FinancialHub.Auth.Infra.Extensions;
using FinancialHub.Auth.Infra.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAuthServices(builder.Configuration)
    .AddAuthResources()
    .AddAuthProviders(builder.Configuration)
    .AddAuthRepositories(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
