using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StockMarket.DataContext.DataContext;
using StockMarket.DataContext.Repository.Implementations;
using StockMarket.DataContext.Repository.Interfaces;
using StockMarket.Models.Users;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/* Registering Data Context */
builder.Services.AddDbContext<ApplicationDbContext>(item =>
{
    item.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

/* Registering Identity Services */
builder.Services.AddIdentity<UsersModel, IdentityRole>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

/* Registering Authentication */
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    /* This line will set all the above vales to default (JwtBearerDefaults.AuthenticationScheme) */
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[key: "JWT:SigningKey"]))
    };
    option.SaveToken = true;
});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

/* Swagger .NET Core Web API JWT Setup */
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

/* Registering NewtonsoftJSON */
builder.Services.AddControllers().AddNewtonsoftJson(item =>
{
    item.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

/* Registering Repositories */
builder.Services.AddScoped<IStockRepositoryServices, StockRepository>();
builder.Services.AddScoped<ICommentRepositoryService, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenRepository>();
builder.Services.AddScoped<IPortfolioRepositoryServices, PortfolioRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
