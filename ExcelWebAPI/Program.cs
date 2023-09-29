using ExcelWebAPI;
using ExcelWebAPI.Managers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExcelWebApiContext>(options=>
        options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));
builder.Services.AddScoped<IDocumentManager, DocumentManager>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHsts();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
