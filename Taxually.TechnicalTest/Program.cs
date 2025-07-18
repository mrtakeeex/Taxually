using Microsoft.Extensions.DependencyInjection;
using Taxually.TechnicalTest;
using Taxually.TechnicalTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IVatRegistrationService, VatRegistrationService>();

builder.Services.AddSingleton<ICountryVatRegistrationService, GermanyRegistrationService>();
builder.Services.AddSingleton<ICountryVatRegistrationService, FranceRegistrationService>();
builder.Services.AddSingleton<ICountryVatRegistrationService, GreatBritainRegistrationService>();

builder.Services.AddScoped<ITaxuallyQueueClient, TaxuallyQueueClient>();
builder.Services.AddScoped<ITaxuallyHttpClient, TaxuallyHttpClient>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
