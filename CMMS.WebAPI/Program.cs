using CMMS.BLL.Configuration;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Realtime;
using CMMS.BLL.Services;
using CMMS.DAL.DBContext;
using CMMS.DAL.Interfaces;
using CMMS.DAL.Repositories;
using CMMS.WebAPI.Hubs;
using CMMS.WebAPI.Hubs.Publishers;
using CMMS.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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
builder.Services.AddScoped<IHarvestRepository, HarvestRepository>();
builder.Services.AddScoped<IHarvestDetailRepository, HarvestDetailRepository>();
builder.Services.AddScoped<IIotDeviceRepository, IotDeviceRepository>();
builder.Services.AddScoped<IIotDataRepository, IotDataRepository>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();
builder.Services.AddScoped<ICropGrowthTaskRepository, CropGrowthTaskRepository>();
builder.Services.AddScoped<ISubTaskRepository, SubTaskRepository>();
builder.Services.AddScoped<IWorkerScheduleRepository, WorkerScheduleRepository>();
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
builder.Services.AddScoped<IGrowthTrackingRepository, GrowthTrackingRepository>();

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
builder.Services.AddScoped<IHarvestService, HarvestService>();
builder.Services.AddScoped<IHarvestDetailService, HarvestDetailService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IIotDeviceService, IotDeviceService>();
builder.Services.AddScoped<IIotDataService, IotDataService>();
builder.Services.AddScoped<ISensorDataService, SensorDataService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<ICropGrowthTaskService, CropGrowthTaskService>();
builder.Services.AddScoped<ISubTaskService, SubTaskService>();
builder.Services.AddScoped<IWorkerScheduleService, WorkerScheduleService>();
builder.Services.AddScoped<ICropGrowthStageService, CropGrowthStageService>();
builder.Services.AddScoped<ISoilCropCompatibilityService, SoilCropCompatibilityService>();
builder.Services.AddScoped<IRecommendationTaskService, RecommendationTaskService>();
builder.Services.AddScoped<IRecommendationTaskDetailService, RecommendationTaskDetailService>();
builder.Services.AddScoped<IGrowthTrackingService, GrowthTrackingService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.Configure<VNPaySettings>(builder.Configuration.GetSection("PaymentSettings:VNPay"));
builder.Services.Configure<PayOSSettings>(builder.Configuration.GetSection("PaymentSettings:PayOS"));
builder.Services.AddHttpClient<IPlantAnalysisService, PlantAnalysisService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();

builder.Services.AddScoped<VNPayService>();
builder.Services.AddHttpClient<PayOSService>();
builder.Services.AddScoped<IDiagnosisBillingService, DiagnosisBillingService>();

builder.Services.AddSignalR()
    .AddMessagePackProtocol()
    .AddJsonProtocol(opts =>
    {
        opts.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddScoped<INotificationRealtime, NotificationRealtimePublisher>();
builder.Services.AddScoped<IIotRealtime, IotRealtimePublisher>();
builder.Services.AddScoped<ITaskRealtime, TaskRealtimePublisher>();
builder.Services.AddScoped<IPaymentRealtime, PaymentRealtimePublisher>();

builder.Services.AddEndpointsApiExplorer();
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
var allowVercelPreviews = builder.Configuration.GetValue<bool>("Cors:AllowVercelPreviews");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            {
                if (allowedOrigins.Contains(origin)) return true;
                if (!allowVercelPreviews) return false;
                return Uri.TryCreate(origin, UriKind.Absolute, out var uri)
                    && uri.Host.EndsWith(".vercel.app");
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

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
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var accessToken = ctx.Request.Query["access_token"];
                var path = ctx.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    ctx.Token = accessToken;
                return Task.CompletedTask;
            }
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ResponseTimeMiddleware>();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications");
app.MapHub<IotHub>("/hubs/iot");
app.MapHub<TaskHub>("/hubs/tasks");
app.MapHub<PaymentHub>("/hubs/payments");

app.Run();