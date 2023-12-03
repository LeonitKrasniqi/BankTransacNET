using AccountManagmentSystemAPI.Controllers;
using AccountManagmentSystemAPI.Data;
using AccountManagmentSystemAPI.Mappings;
using AccountManagmentSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding DbContext to running project file
builder.Services.AddDbContext<FinancialDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("AMSConnectionString")));

//Adding repos and logic classes using DI
builder.Services.AddScoped<IAccountRepository, SQLAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, SQLTransactionRepository>();


//Adding automapper in project to map fields between model domain and dto model
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

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
