using E_Wallet.CustomMiddleware;
using E_Wallet.Data.Data;
using E_Wallet.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Debugging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomServices();

builder.WebHost.ConfigureLogging((hostingContext, logging) =>
{
    logging.ClearProviders();
    logging.AddSerilog(new LoggerConfiguration()
          .MinimumLevel.Debug()
          .WriteTo.Console()
          .CreateLogger());
});

builder.Host.UseSerilog();

builder.Services.AddDbContext<WalletDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("WalletDb"));
});

builder.Services.BuildServiceProvider().GetService<WalletDbContext>().Database.Migrate();

var app = builder.Build();


    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetRequiredService<WalletDbSeed>();

    }
    
if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


app.UseSerilogRequestLogging();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<CustomExceptionMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

