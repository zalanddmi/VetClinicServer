using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VetClinicServer.Data;
using VetClinicServer.Repositories;
using VetClinicServer.Services;
using VetClinicServer.Settings;
using VetClinicServer.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
      "Development",
      builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});
builder.Services.AddMvc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<Context>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = JwtOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = JwtOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
      "v1",
      new OpenApiInfo { Title = "Pbk service", Version = "v1" }
    );
    options.AddSecurityDefinition(
      "Bearer",
      new OpenApiSecurityScheme
      {
          In = ParameterLocation.Header,
          Description = "Please enter token",
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          BearerFormat = "JWT",
          Scheme = "bearer"
      }
    );
    options.AddSecurityRequirement(
      new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
      }
    );
});


builder.Services.AddTransient<DrugRepository>();
builder.Services.AddTransient<RoleRepository>();
builder.Services.AddTransient<SpeciesRepository>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<DrugService>();
builder.Services.AddTransient<RoleService>();
builder.Services.AddTransient<SpeciesService>();
builder.Services.AddTransient<JwtGenerator>();
builder.Services.AddTransient<PasswordHasher>();
builder.Services.AddTransient<ExcelConverter>();

var app = builder.Build();
app.UseCors("Development");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
