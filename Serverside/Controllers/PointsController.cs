using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Serverside.DTOs;
using Serverside.Services;

namespace Serverside.Controllers;

public static class PointsController {
	public static void MapPointsEndpoints(this WebApplication app) {
		app.MapGet("/user/points", async (ClaimsPrincipal principal, PointsService pointsService) => {
				var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return Results.Unauthorized();

				var dto = await pointsService.GetPointsAsync(userId);
				return Results.Ok(dto);
			})
			.RequireAuthorization()
			.WithName("GetMyPoints")
			.WithTags("Points");

		app.MapPost("/user/points",
				async ([FromBody] PostPointRequestDto? dto, ClaimsPrincipal principal, PointsService pointsService) => {
					var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
					if (string.IsNullOrEmpty(userId))
						return Results.Unauthorized();

					var result = await pointsService.PostPointAsync(userId, dto?.Source ?? "unknown", dto?.Amount ?? 1);
					return Results.Ok(result);
				})
			.RequireAuthorization()
			.WithName("AddPoints")
			.WithTags("Points");

		app.MapGet("/user/points/all", async (PointsService pointsService) => {
				var list = await pointsService.GetAllPointsAsync();
				return Results.Ok(list);
			})
			.RequireAuthorization()
			.WithName("GetAllPoints")
			.WithTags("Points");

		app.MapGet("/users/{id}/points", async (string id, PointsService pointService) => {
				try {
					var dto = await pointService.GetPointsForUserAsync(id);
					return dto == null
						? Results.NotFound(new { error = $"Bruger med ID '{id}' blev ikke fundet." })
						: Results.Ok(dto);
				} catch (Exception ex) {
					return Results.Problem(detail: ex.Message, statusCode: 500,
						title: "Der opstod en fejl ved hentning af point");
				}
			})
			.RequireAuthorization("AdminOnly")
			.WithName("GetPointsById")
			.WithTags("Points");

		app.MapPut("/users/{id}/points", async (string id, [FromBody] PutPointDto? dto, PointsService pointService) => {
				if (dto == null)
					return Results.BadRequest(new { error = "Request body mangler." });
				try {
					var result = await pointService.PutPointAsync(id,
						dto.Total);
					return result == null ? Results.NotFound(new { error = $"Bruger med ID '{id}' blev ikke fundet." }) : Results.Ok(result);
				} catch (InvalidOperationException ex) {
					return Results.BadRequest(new { error = ex.Message });
				} catch (Exception ex) {
					return Results.Problem(detail: ex.Message,
						statusCode: 500, title: "Der opstod en fejl ved opdatering af point");
				}
			})
			.RequireAuthorization("AdminOnly")
			.WithName("SetPoints")
			.WithTags("Points");
	}
}