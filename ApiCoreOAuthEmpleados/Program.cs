using ApiCoreOAuthEmpleados.Data;
using ApiCoreOAuthEmpleados.Helpers;
using ApiCoreOAuthEmpleados.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//creamos una instancia del helper
HelperActionServicesOAuth helper =
    new HelperActionServicesOAuth(builder.Configuration);
//esta instancia del helper debemos incluirla dentro
//de nuestra app solamente una vez para que todo lo que
//hemos creado dentro no se genere de nuevo
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
//habilitamos los servicios de auth que hemos creado 
//en el helper con action<>
builder.Services.AddAuthentication
    (helper.GetAuthenticationSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());

// Add services to the container.
string connectionString =
    builder.Configuration.GetConnectionString("SQL");
builder.Services.AddTransient<RepositoryEmpleados>();
builder.Services.AddDbContext<HospitalContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Api OAuth Empleados :D",
        Description = "Api con token de seguridad",
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json"
        , name: "Api OAuth Empleados");
    options.RoutePrefix = "";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
