using Microsoft.AspNetCore.Authentication.Negotiate;
using PSE.Dictionary;
using PSE.WebApi.ApplicationSettings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = ConfigurationReader.ReadConfiguration();
var appSettings = configuration.ReadAppSettings();
builder.Services.AddSingleton(appSettings);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddSingleton<IPSEDictionaryService>(sp => {
    var logger = sp.GetRequiredService<ILogger<PSEDictionaryService>>();
    return new PSEDictionaryService(appSettings.DictionariesPath, logger);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
