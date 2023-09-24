using Back.Conventions;
using Webscrapping.Services.Interfaces;
using Webscrapping.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IJobScraperService, JobScraperService>();
builder.Services.AddScoped<ISearch, LinkedInSearch>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new LowercaseControllerConvention());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
