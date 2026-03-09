using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serverside.Controllers;
using Serverside.Data;
using Serverside.Repositories;
using Serverside.Services;

var builder = WebApplication.CreateBuilder(args);

var url = "http://0.0.0.0:5212";
if (!string.IsNullOrEmpty(url))
	builder.WebHost.UseUrls(url);

builder.Services.ConfigureHttpJsonOptions(options => {
	options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
		Description = "JWT. Only enter your token",
		Name = "Authorization",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
		Scheme = "Bearer",
	});
	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
				Reference = new Microsoft.OpenApi.Models.OpenApiReference {
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.Services.AddSingleton<DataService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IPointsRepository, PointsRepository>();
builder.Services.AddScoped<PointsService>();

builder.Services.AddCors(options => {
	options.AddDefaultPolicy(policy => {
		policy.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key mangler i appsettings.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options => {
		options.TokenValidationParameters = new TokenValidationParameters {
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
			RoleClaimType = System.Security.Claims.ClaimTypes.Role
		};
	});
builder.Services.AddAuthorization(options => { options.AddPolicy("AdminOnly", p => p.RequireRole("Admin")); });

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "gameapi.db");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? $"Data Source={dbPath}";
builder.Services.AddDbContext<DbContextGameApi>(options =>
	options.UseSqlite(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
	var db = scope.ServiceProvider.GetRequiredService<DbContextGameApi>();
	db.Database.EnsureCreated();
	// Opret Point tabellen hvis den ikke findes (ved opgradering fra in-memory)
	db.Database.ExecuteSqlRaw(
		"CREATE TABLE IF NOT EXISTS Point (UserId TEXT NOT NULL PRIMARY KEY, Total INTEGER NOT NULL DEFAULT 0)");
}

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapPointsEndpoints();

app.Run();