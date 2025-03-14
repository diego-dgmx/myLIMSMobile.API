using System.Text;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;

var builder = WebApplication.CreateBuilder(args);

LogConfiguration.AddSerilogLabsoftApplicationLog(
    builder.Services,
    builder.Configuration.GetSection("ApplicationLog"),
    builder.Environment.EnvironmentName);

builder.Services.AddControllers();
builder.Services.AddHttpClient(); // Registrar HttpClient
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Labsoft myLIMS Mobile API",
        Version = "v1",
        Description = "myLIMS data services"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();
    c.OperationFilter<SwaggerFileOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
    };
});



//Configure API env settings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add external services scopes
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<ISamplesServices, SamplesServices>();
builder.Services.AddScoped<ISampleAnalysisServices, SampleAnalysisServices>();
builder.Services.AddScoped<IMethodsServices, MethodsServices>();
builder.Services.AddScoped<IConsumablesServices, ConsumablesServices>();
builder.Services.AddScoped<IConsumableTypesServices, ConsumableTypesServices>();
builder.Services.AddScoped<IEquipmentsServices, EquipmentsServices>();
builder.Services.AddScoped<IEquipmentTypesServices, EquipmentTypesServices>();
builder.Services.AddScoped<IQCTestsServices, QCTestsServices>();
builder.Services.AddScoped<IMessagesServices, MessagesServices>();
builder.Services.AddScoped<IFilesServices, FilesServices>();
builder.Services.AddScoped<IMeasurementUnitsServices, MeasurementUnitsServices>();
builder.Services.AddScoped<ISystemConfigsServices, SystemConfigsServices>();
builder.Services.AddScoped<IScheduleInterventionsServices, ScheduleInterventionsServices>();
builder.Services.AddScoped<IServiceAreasServices, ServiceAreasServices>();
builder.Services.AddScoped<IServiceCentersServices, ServiceCentersServices>();
builder.Services.AddScoped<IInfosServices, InfosServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Labsoft myLIMS Mobile API");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
