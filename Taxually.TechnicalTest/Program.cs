using Taxually.TechnicalTest;
using Taxually.TechnicalTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IVatRegistrationService, VatRegistrationService>();

builder.Services.AddScoped<ICountryVatRegistrationService, GermanyRegistrationService>();
builder.Services.AddScoped<ICountryVatRegistrationService, FranceRegistrationService>();
builder.Services.AddScoped<ICountryVatRegistrationService, GreatBritainRegistrationService>();

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
