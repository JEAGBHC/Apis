//using System.Text;
//using System.Text.Json.Serialization;
//using BibliotecaAPI.Entidades;
//using BibliotecaAPI.Servicios;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using webApi;
//using webApi.Controllers;
//using webApi.Datos;
//using webApi.Entidades;

//var builder = WebApplication.CreateBuilder(args);

//// Configuración de AutoMapper
//builder.Services.AddAutoMapper(typeof(Program));

//// Configuración de los controladores
//builder.Services.AddControllers().AddNewtonsoftJson();

//// Registro de servicios personalizados
//builder.Services.AddTransient<IServiciosUsuarios, ServiciosUsuarios>();

//// Configuración del DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Configuración de Identity con Usuario personalizado
//builder.Services.AddIdentity<Usuario, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

//// Registro de UserManager y SignInManager con Usuario personalizado
//builder.Services.AddScoped<UserManager<Usuario>>();
//builder.Services.AddScoped<SignInManager<Usuario>>();

//// Agregar HttpContextAccessor para acceder al contexto HTTP
//builder.Services.AddHttpContextAccessor();

//// Configuración de autenticación con JWT
//builder.Services.AddAuthentication().AddJwtBearer(options =>
//{
//    options.MapInboundClaims = false;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"])),
//        ClockSkew = TimeSpan.Zero,
//    };
//});
//builder.Services.AddAuthorization(opciones =>
//{
//    opciones.AddPolicy("esadmin", politica => politica.RequireClaim("esadmin"));
//});


//// Construcción de la aplicación
//var app = builder.Build();

//// Configuración de los middlewares
//app.MapControllers();

//// Ejecución de la aplicación
//app.Run();















using System.Text;
using System.Text.Json.Serialization;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Servicios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webApi;
using webApi.Controllers;
using webApi.Datos;
using webApi.Entidades;

var builder = WebApplication.CreateBuilder(args);

//Inicio del area de servios
//configuracion de autoMaper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<SignInManager<Usuario>>();
builder.Services.AddScoped<UserManager<Usuario>>();
builder.Services.AddTransient<IServiciosUsuarios, ServiciosUsuarios>();
//al utilizar las dtos ya no necesitamos el evitar comportamientos ciclicosl
//    AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler
//= ReferenceHandler.IgnoreCycles);


builder.Services.AddDbContext<ApplicationDbContext>(opciones
=> opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddSingleton<IRepositorioValores, RepositorioValoresOracle>();
//builder.Services.AddTransient< IRepositorioValores, RepositorioValores>();

//configurando el sistema de entity personalizada
builder.Services.AddIdentity< Usuario, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//configurando el manejadr de usuarios 
builder.Services.AddScoped<UserManager<Usuario>>();
//Autenticar usuarios 
builder.Services.AddScoped<SignInManager<Usuario>>();
//nos permitira acceder al contexto http desde cualquier clase 
builder.Services.AddHttpContextAccessor();

//manejar el tokken 
builder.Services.AddAuthentication().AddJwtBearer(opciones =>

{
    opciones.MapInboundClaims = false;
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        //validar el emisor del tokken 
        ValidateIssuer = false,
        //validar la audencia 
        ValidateAudience = false,
        //tiempo de vida del tokken
        ValidateLifetime = true,
        //validar la llave secreta 
        ValidateIssuerSigningKey = true,
        //configurar llave secreta 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"]!)),
        // configuracion de los valores de tiempo para no tener discrepancia en la validacion
        ClockSkew = TimeSpan.Zero,
    };
});

//Fin del area de servios
var app = builder.Build();
//Inicio del area de los middlewares
//app.MapGet("/", () => "Adios Mundo!");
app.MapControllers();


//Fin del area de los middlewares

app.Run();


