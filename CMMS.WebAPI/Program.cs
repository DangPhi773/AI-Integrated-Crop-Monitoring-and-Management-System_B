using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Services;
using CMMS.DAL.DBContext;
using CMMS.DAL.Interfaces;
using CMMS.DAL.Repositories;
using CMMS.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>  
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionStringDB")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskDetailRepository, TaskDetailRepository>();
builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();
builder.Services.AddScoped<IFarmRepository, FarmRepository>();
builder.Services.AddScoped<ISoilRepository, SoilRepository>();
builder.Services.AddScoped<IPlotRepository, PlotRepository>();
builder.Services.AddScoped<IBedRepository, BedRepository>();
builder.Services.AddScoped<ISeasonsDetailRepository, SeasonsDetailRepository>();
builder.Services.AddScoped<IIotDeviceRepository, IotDeviceRepository>();
builder.Services.AddScoped<IIotDataRepository, IotDataRepository>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();
builder.Services.AddScoped<ICropGrowthTaskRepository, CropGrowthTaskRepository>();
builder.Services.AddScoped<ISubTaskRepository, SubTaskRepository>();
builder.Services.AddScoped<IWorkerScheduleRepository, WorkerScheduleRepository>();
builder.Services.AddScoped<ICropBedConfigRepository, CropBedConfigRepository>();
builder.Services.AddScoped<ICropGrowthStageRepository, CropGrowthStageRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IReportAssignmentRepository, ReportAssignmentRepository>();
builder.Services.AddScoped<IDiagnosisResultRepository, DiagnosisResultRepository>();
builder.Services.AddScoped<IReportEnvironmentSnapshotRepository, ReportEnvironmentSnapshotRepository>();
builder.Services.AddScoped<ISoilCropCompatibilityRepository, SoilCropCompatibilityRepository>();
builder.Services.AddScoped<IRecommendationTaskRepository, RecommendationTaskRepository>();
builder.Services.AddScoped<IRecommendationTaskDetailRepository, RecommendationTaskDetailRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICropService, CropService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskDetailService, TaskDetailService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<IFarmService, FarmService>();
builder.Services.AddScoped<ISoilService, SoilService>();
builder.Services.AddScoped<IPlotService, PlotService>();
builder.Services.AddScoped<IBedService, BedService>();
builder.Services.AddScoped<ISeasonsDetailService, SeasonsDetailService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IIotDeviceService, IotDeviceService>();
builder.Services.AddScoped<IIotDataService, IotDataService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<ICropGrowthTaskService, CropGrowthTaskService>();
builder.Services.AddScoped<ISubTaskService, SubTaskService>();
builder.Services.AddScoped<IWorkerScheduleService, WorkerScheduleService>();
builder.Services.AddScoped<ICropBedConfigService, CropBedConfigService>();
builder.Services.AddScoped<ICropGrowthStageService, CropGrowthStageService>();
builder.Services.AddScoped<ISoilCropCompatibilityService, SoilCropCompatibilityService>();
builder.Services.AddScoped<IRecommendationTaskService, RecommendationTaskService>();
builder.Services.AddScoped<IRecommendationTaskDetailService, RecommendationTaskDetailService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.Configure<PlantNetSettings>(builder.Configuration.GetSection("PlantNet"));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddHttpClient<IPlantNetService, PlantNetService>();

builder.Services.AddEndpointsApiExplorer();


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new Exception("JWT Secret Key is missing!");
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, 
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.NameIdentifier,
            ClockSkew = TimeSpan.Zero 
        };
    });

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CMMS API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",         
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerOnly", policy => policy.RequireRole("Owner"));

    options.AddPolicy("SpecialistOnly", policy => policy.RequireRole("Owner", "Specialist"));

    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Owner", "Worker", "Specialist"));
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ResponseTimeMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();