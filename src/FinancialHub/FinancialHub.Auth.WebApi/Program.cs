using FinancialHub.Auth.Application.Extensions;

namespace FinancialHub.Auth.WebApi;

public partial class Program
{
    protected Program()
    {

    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthApplication(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddAuthDocs();

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
    }
}