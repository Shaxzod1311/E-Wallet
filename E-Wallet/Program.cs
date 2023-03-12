using E_Wallet.CustomMiddleware;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.AddScoped<IAuthorizationHandler, HmacAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Hmac", policy =>
    {
        policy.AddRequirements(new HmacAuthorizationRequirement(userId => GetSecretKey(userId)));
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomExceptionMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
