using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Serverside.DTOs;
using Serverside.Services;

namespace Serverside.Controllers;

public static class UserController {
	public static void MapUserEndpoints(this WebApplication app) {
		app.MapGet("/users", async (UserService userService) => {
				var users = await userService.GetAllUsersAsync();
				return Results.Ok(users);
			})
			.WithName("GetAllUsers")
			.RequireAuthorization("AdminOnly");

		app.MapGet("/users/{id}", async (string id, ClaimsPrincipal principal, UserService userService) => {
			var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
			var isAdmin = principal.IsInRole("Admin");

			if (string.IsNullOrEmpty(userId))
				return Results.Unauthorized();

			if (!isAdmin && userId != id)
				return Results.Forbid();

			var user = await userService.GetUserByIdAsync(id);
			return user != null ? Results.Ok(user) : Results.NotFound();
		})
			.WithName("GetUserById")
			.RequireAuthorization("AdminOnly");

		app.MapGet("/users/email/{email}", async (string email, UserService userService) => {
			var user = await userService.GetUserByEmailAsync(email);
			return user == null ? Results.NotFound() : Results.Ok(user);
		}).WithName("GetUserByEmail");

		app.MapPost("/users", async ([FromBody] UserPostDto user, UserService userService) => {
			var createdUser = await userService.CreateUserAsync(user);
			return Results.Created($"/users/{createdUser.Id}", createdUser);
		}).WithName("CreateUser");

		app.MapPatch("/users/{id}", async (string id, [FromBody] UserUpdateDto user, UserService userService) => {
			var updatedUser = await userService.UpdateUserAsync(id, user);
			return updatedUser ? Results.NoContent() : Results.NotFound();
		}).WithName("UpdateUser");

		app.MapDelete("/users/{id}", async (string id, [FromBody] UserDeleteDto deleteDto, UserService userService) => {
				var deleted = await userService.DeleteUserAsync(id);
				return deleted ? Results.NoContent() : Results.NotFound();
			})
			.WithName("DeleteUser")
			.RequireAuthorization("AdminOnly");
	}
}