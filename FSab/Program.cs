using FSab.Data;
using FSab.Services;
using FSab.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services
    .AddAuthentication()
    .AddMicrosoftIdentityWebApi(
        builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

builder.Services.AddScoped<IFaultReportService, FaultReportService>();
builder.Services.AddScoped<ISMSNotifyer, SMSNotifyer>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
