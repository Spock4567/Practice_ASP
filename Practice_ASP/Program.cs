using Microsoft.Extensions.Options;
using Practice_ASP;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Добавляем конфигурацию
builder.Services.Configure<AppConfig>(builder.Configuration);

// Регистрируем HttpClient с конфигурацией
builder.Services.AddHttpClient<StringProcessorService>((serviceProvider, client) =>
{
    var config = serviceProvider.GetRequiredService<IOptions<AppConfig>>().Value;
    client.BaseAddress = new Uri(config.RandomApi);
    client.Timeout = TimeSpan.FromSeconds(5); 
});
builder.Services.AddSingleton<StringProcessorService>();
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
