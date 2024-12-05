using Microsoft.EntityFrameworkCore;
using RRHH.Api;
using RRHH.Application.Interfaces;
using RRHH.Application.Mapper;
using RRHH.Application.Services;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
using RRHH.Infrastructure.Repositories;
using RRHH.Infrastructure.UnitOfWork;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RRHH.Domain.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura Serilog usando el archivo de configuración
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Lee la configuración de appsettings.json
    .CreateLogger();

builder.Host.UseSerilog(); // Usa Serilog para el registro de logs

builder.Services.AddDbContext<ApprrhhApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL")));

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddScoped<EmployeesMapper>();
builder.Services.AddScoped<DepartmentMapper>();
builder.Services.AddScoped<LeaveMapper>();
builder.Services.AddScoped<CheckDaysService>();

builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ILeaveService, LeavesService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmployeesRepository<Employee>, EmployeesReposity<Employee>>();
builder.Services.AddScoped<IDepartmentRepository<Department>, DepartmentRepository<Department>>();
builder.Services.AddScoped<ILeaveRepository<Leaf>, LeaveRepository<Leaf>>();


// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:ClaveSecreta"]))
    };
});

// Configura la serialización JSON para manejar referencias cíclicas
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Maneja las referencias cíclicas
    });

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
