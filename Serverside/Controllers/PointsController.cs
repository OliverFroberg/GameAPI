using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Serverside.DTOs;
using Serverside.Services;

namespace Serverside.Controllers;

public static class PointsController {
	public static void MapPointsEndpoints(this WebApplication app) {
		app.MapGet("/user/points", (ClaimsPrincipal principal, PointsService pointsService) => {
				var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return Results.Unauthorized();

				var dto = pointsService.GetUserPoints(userId);
				return Results.Ok(dto);
			})
			.RequireAuthorization()
			.WithName("GetMyPoints")
			.WithTags("Points");

		app.MapPost("/user/points",
				([FromBody] AddPointsDto? dto, ClaimsPrincipal principal, PointsService pointsService) => {
					var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
					if (string.IsNullOrEmpty(userId))
						return Results.Unauthorized();
					
					var result = pointsService.AddPoints(userId, dto?.Source ?? "unknown", dto?.Amount ?? 1);
					return Results.Ok(result);
				})
			.RequireAuthorization()
			.WithName("AddPoints")
			.WithTags("Points");

		app.MapGet("/user/points/all", (PointsService pointsService) => {
				var list = pointsService.GetAllUserPoints();
				return Results.Ok(list);
			})
			.RequireAuthorization()
			.WithName("GetAllPoints")
			.WithTags("Points");
	}
}