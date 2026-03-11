using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Serverside.DTOs;
using Serverside.Services;

namespace Serverside.Controllers;

public static class MazeController {
	public static void MapMazeEndpoints(this WebApplication app) {
		app.MapGet("/maze", async (ClaimsPrincipal principal, MazeService mazeService) => {
				var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return Results.Unauthorized();

				var dto = await mazeService.GetHighestCompletedLevelAsync(userId);
				return Results.Ok(dto);
			})
			.RequireAuthorization()
			.WithName("GetHighestLevel")
			.WithTags("Maze");

		app.MapPost("/maze",
				async ([FromBody] PostMazeLevelRequestDto? dto, ClaimsPrincipal principal, MazeService mazeService) => {
					var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
					if (string.IsNullOrEmpty(userId))
						return Results.Unauthorized();

					var result = await mazeService.PostCompletedLevelAsync(userId, dto?.Level ?? 1);
					return Results.Ok(result);
				})
			.RequireAuthorization()
			.WithName("CompletedMaze")
			.WithTags("Maze");
	}
}