using System.Text.Json;
using Serverside.Models;

namespace Serverside.Services;

public class DataService {
	private readonly string _dataFilePath;
	private readonly JsonSerializerOptions _jsonOptions;
	private readonly ILogger<DataService> _logger;

	public DataService(ILogger<DataService> logger) {
		_logger = logger;

		var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");

		if (!Directory.Exists(dataDirectory)) {
			Directory.CreateDirectory(dataDirectory);
		}

		_dataFilePath = Path.Combine(dataDirectory, "users.json");

		_jsonOptions = new JsonSerializerOptions {
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
		};
	}

	public async Task<List<User>> LoadUsersAsync() {
		if (!File.Exists(_dataFilePath)) {
			return new List<User>();
		}

		try {
			var json = await File.ReadAllTextAsync(_dataFilePath);
			if (string.IsNullOrWhiteSpace(json)) {
				return new List<User>();
			}

			var users = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions);
			return users ?? new List<User>();
		} catch (Exception ex) {
			_logger.LogWarning(ex, "Fejl ved læsning af data fra {Path}", _dataFilePath);
			return new List<User>();
		}
	}

	public async Task SaveUsersAsync(List<User> users) {
		try {
			var json = JsonSerializer.Serialize(users, _jsonOptions);
			await File.WriteAllTextAsync(_dataFilePath, json);
		} catch (Exception ex) {
			_logger.LogError(ex, "Fejl ved gemning af data til {Path}", _dataFilePath);
			throw new Exception("Kunne ikke gemme data", ex);
		}
	}
}