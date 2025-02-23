
using APIProfessor.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Agregar el DbContext para la base de datos de profesores
builder.Services.AddDbContext<ScnProfessorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProfessorConnection")));

builder.Services.AddDbContext<ScnAdminDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdminConnection")));

// Agregar el DbContext para la base de datos de estudiantes
builder.Services.AddDbContext<ScnStudentDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("StudentConnection")));

// Agregar el DbContext para la base de datos de appointments
builder.Services.AddDbContext<ScnAppointmentsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentConnection")));

builder.Services.AddControllers().AddNewtonsoftJson();
//builder.Services.AddControllers();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
