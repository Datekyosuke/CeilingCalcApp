

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiDB;
using WebApiDB.Context;
using WebApiDB.Interfaces;
using WebApiDB.Mapper;
using WebApiDB.Models;
using WebApiDB.Repository;
using WebApiDB.Servics;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using CeilingCalc.Interfaces;
using CeilingCalc.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CeilingCalc.Autorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CeilingCalc.Services.Identity;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//var serverVersion = new MySqlServerVersion(new Version(5, 7, 27));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AplicationContext>(options =>
                options.UseNpgsql(connectionString));
builder.Services.AddTransient<IDealerRepository, DealerReposytory>();
builder.Services.AddTransient<IMaterialRepository, MaterialRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

#region Swagger Configuration
builder.Services.AddSwaggerGen(options =>
{
    options.DescribeAllParametersInCamelCase();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Ceiling API",
        Description = "Stretch ceiling software",
        //TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Yuri Kuzmin",
            Url = new Uri("https://t.me/DateKyosuke")
        }

    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                });

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "CeilingCalc.xml");
    options.IncludeXmlComments(filePath);
});
#endregion

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7021/swagger",
                                             "https://localhost:80/swagger",
                                             "https://localhost:5000/swagger",
                                             "http://localhost:3000",
                                             "http://localhost:8000").WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH")
                                             .WithHeaders("Content-Type", "Access-Control-Allow-Headers", "Authorization", "X-Requested-With");
                      });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});
builder.Services.AddControllers(options =>
{
options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());

});
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddValidatorsFromAssemblyContaining<OrderDTOValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.InvalidModelStateResponseFactory = actionContext =>
    {

        List<Error> error = actionContext.ModelState
                    .Where(modelError => modelError.Value.Errors.Count > 0)
                    .Select(modelError => new Error
                    {
                        ErrorField = modelError.Key,
                        ErrorDescription = modelError.Value.Errors.FirstOrDefault().ErrorMessage
                    }).ToList();

        return new BadRequestObjectResult(error);
    };
});
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
            ValidAudience = builder.Configuration["Jwt:Audience"]!,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });
builder.Services.AddAuthorization(options => options.DefaultPolicy =
    new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());
builder.Services.AddIdentity<ApplicationUser, IdentityRole<long>>()
    .AddEntityFrameworkStores<AplicationContext>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
