using System.ComponentModel.DataAnnotations;

namespace Serverside.DTOs;

public class PutPointDto {
	[Range(0, long.MaxValue, ErrorMessage = "Amount must be 0 or above.")]
	public long Total { get; set; }
}