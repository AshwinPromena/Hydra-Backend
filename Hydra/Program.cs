using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.BusinessLayer.Concrete.IService.ISettingsService;
using Hydra.BusinessLayer.Concrete.IService.IStaffService;
using Hydra.BusinessLayer.Concrete.IService.IUniversityService;
using Hydra.BusinessLayer.Concrete.Service.BadgeService;
using Hydra.BusinessLayer.Concrete.Service.SettingsService;
using Hydra.BusinessLayer.Concrete.Service.StaffService;
using Hydra.BusinessLayer.Concrete.Service.UniversityService;
using Hydra.BusinessLayer.Repository.IService.IAccountService;
using Hydra.BusinessLayer.Repository.IService.IDropDownService;
using Hydra.BusinessLayer.Repository.IService.ILearnerService;
using Hydra.BusinessLayer.Repository.Service.AccountService;
using Hydra.BusinessLayer.Repository.Service.DropDownService;
using Hydra.BusinessLayer.Repository.Service.LearnerService;
using Hydra.Common.Repository.IService;
using Hydra.Common.Repository.Service;
using Hydra.Common.Swagger;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Hydra.DatbaseLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HydraContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.Parse("8.0.28"))
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors();
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Hydra",
    new OpenApiInfo
    {
        Title = "Hydra",
        Version = "1.0",
        Description = "Hydra using ASP.NET CORE 7",
        Contact = new OpenApiContact
        {
            Name = "",
            Email = "",
        },
    });
    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });
    c.OperationFilter<AuthOperationFilter>();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("JwtOptions:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("JwtOptions:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtOptions:SecurityKey")))
    };
});

builder.Services.AddControllers();


builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(
        "https://localhost:3000",
        "http://localhost:3000",
        "https://localhost:3001",
        "http://localhost:3001",
        "https://hydra-react.vercel.app",
        "http://hydra-react.vercel.app",
        "https://bfactory-react.vercel.app",
        "http://bfactory-react.vercel.app")
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowCredentials();
}));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDropDownService, DropDownService>();
builder.Services.AddScoped<IStorageService, StorageServices>();
builder.Services.AddScoped<ILearnerManagmentService, LearnerManagmentService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IBadgeSequenceService, BadgeSequenceService>();
builder.Services.AddScoped<IBadgeSequenceService, BadgeSequenceService>();
builder.Services.AddScoped<IStaffService, StaffServices>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IUniversityService, UniversityService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseDeveloperExceptionPage();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("Hydra/swagger.json", "Hydra");
    c.DisplayRequestDuration();
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.DefaultModelExpandDepth(-1);
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

