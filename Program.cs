

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiDB;
using WebApiDB.Context;
using WebApiDB.Interfaces;
using WebApiDB.Mapper;
using WebApiDB.Repository;
using WebApiDB.Servics;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


//"Database=u2028771_testbd;Data Source=server203.hosting.reg.ru;User Id=u2028771_datekyo;Password=m2jl2aoe;";

var serverVersion = new MySqlServerVersion(new Version(5, 7, 27));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DealerContextWithMigrations");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AplicationContext>(options =>
                options.UseMySql(connectionString, serverVersion));
builder.Services.AddTransient<IDealerRepository, DealerReposytory>();
builder.Services.AddTransient<IMaterialRepository, MaterialRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

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

    }

    );
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebApiDB.xml");
    options.IncludeXmlComments(filePath);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7021/swagger",
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}


app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);


app.UseAuthorization();

app.MapControllers();

app.Run();
