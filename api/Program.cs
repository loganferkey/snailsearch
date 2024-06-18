using api.Data;
using api.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
// DI Configurations
builder.WebHost.UseUrls("https://localhost:7049","http://localhost:5051");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ConsoleUtil>();
builder.Services.AddScoped<IWebScraper, RelicScraper>();
builder.Services.AddScoped<IRelicUtil, RelicUtil>();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("Local"));
});

var app = builder.Build();
// Middleware Configurations

// Console commands for seeding/extra functionality
if (args.Length > 0)
{
    switch (args[0].ToLower())
    {
        case "seed":
            // Run webscraper and seed the database
            using (var scope = app.Services.CreateScope()) 
            {
                IWebScraper scraper = scope.ServiceProvider.GetRequiredService<IWebScraper>();
                scraper.Seed();
                scraper.SeedDetail();
            }
        break;
        case "purgedb":
            if (!app.Environment.IsDevelopment())
                break;
            // Delete relic database for testing purposes
        break;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
