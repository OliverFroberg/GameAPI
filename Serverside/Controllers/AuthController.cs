using Microsoft.AspNetCore.Mvc;
using Serverside.DTOs;
using Serverside.Services;

namespace Serverside.Controllers;

public static class AuthController {
	public static void MapAuthEndpoints(this WebApplication app) {
		app.MapPost("/auth/register", async ([FromBody] RegisterDto? dto, AuthService auth) => {
				if (dto == null)
					return Results.BadRequest(new { error = "Request body mangler." });

				var result = await auth.RegisterAsync(dto);
				if (result == null)
					return Results.BadRequest(new { error = "Email findes allerede." });

				return Results.Created("/auth/login", result);
			})
			.WithName("Register");

		app.MapPost("/auth/login", async ([FromBody] LoginDto? dto, AuthService auth) => {
				if (dto == null)
					return Results.BadRequest(new { error = "Request body mangler." });

				var result = await auth.LoginAsync(dto);
				if (result == null)
					return Results.Unauthorized();

				return Results.Ok(result);
			})
			.WithName("Login");
	}
}