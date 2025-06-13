using BusinessObjectsLayer.Entity;
using DAOsLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using RepositoriesLayer;
using ServiceLayer;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddDbContext<FunewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.Configure<AdminAccountSettings>(builder.Configuration.GetSection("AdminAccount"));
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Category>("Categories");
modelBuilder.EntitySet<NewsArticle>("NewsArticles");
modelBuilder.EntitySet<SystemAccount>("SystemAccounts");
modelBuilder.EntitySet<Tag>("Tags");

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
})
.AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents("odata", modelBuilder.GetEdmModel()));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5153") // FE application URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true).Build();

// -- SystemAccount
builder.Services.AddScoped<ISystemAccountRepo, SystemAccountRepo>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();

// -- NewsArticle
builder.Services.AddScoped<INewsArticleRepo, NewsArticleRepo>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();

// -- Category
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// -- Tag
builder.Services.AddSingleton<ITagRepo, TagRepo>();
builder.Services.AddSingleton<ITagService, TagService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services
.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidIssuer = configuration["JWT:Issuer"],
        ValidAudience = configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
    };
});
// Add Swagger JWT configuration
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "JWT Authentication for Cosmetics Management",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                };
    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "Role") &&
            context.User.FindFirst(claim => claim.Type == "Role").Value == "0"));

    options.AddPolicy("StaffOnly",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "Role") &&
            context.User.FindFirst(claim => claim.Type == "Role").Value == "1"));

    options.AddPolicy("AdminOrStaff",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "Role")
            && (context.User.FindFirst(claim => claim.Type == "Role").Value == "0"
            || context.User.FindFirst(claim => claim.Type == "Role").Value == "1")));

    options.AddPolicy("AdminOrStaffOrLecturer",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "Role")
            && (context.User.FindFirst(claim => claim.Type == "Role").Value == "0"
            || context.User.FindFirst(claim => claim.Type == "Role").Value == "1"
            || context.User.FindFirst(claim => claim.Type == "Role").Value == "2")));
});

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();


app.MapControllers();

app.Run();
