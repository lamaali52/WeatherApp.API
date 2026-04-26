using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WeatherApp.API.Models;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly string _connectionString;

        public WeatherController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        [HttpPost("save")]
        public IActionResult SaveWeather([FromBody] SaveWeatherRequest request)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_SaveWeatherSearch", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserId", request.UserId);
            command.Parameters.AddWithValue("@CityName", request.CityName);
            command.Parameters.AddWithValue("@CountryCode", request.CountryCode);
            command.Parameters.AddWithValue("@Humidity", request.Humidity);
            command.Parameters.AddWithValue("@TempMin", request.TempMin);
            command.Parameters.AddWithValue("@TempMax", request.TempMax);

            connection.Open();
            var newId = command.ExecuteScalar();
            return Ok(new { SearchId = newId, Message = "Saved successfully" });
        }

        [HttpGet("searches")]
        public IActionResult GetSearches([FromQuery] string? cityName, [FromQuery] int? userId)
        {
            var searches = new List<WeatherSearch>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetWeatherSearches", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CityName", (object?)cityName ?? DBNull.Value);
            command.Parameters.AddWithValue("@UserId", (object?)userId ?? DBNull.Value);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                searches.Add(new WeatherSearch
                {
                    SearchId = (int)reader["SearchId"],
                    CityName = reader["CityName"].ToString()!,
                    CountryCode = reader["CountryCode"]?.ToString() ?? "",
                    Humidity = (int)reader["Humidity"],
                    TempMin = (decimal)reader["TempMin"],
                    TempMax = (decimal)reader["TempMax"],
                    SearchDate = (DateTime)reader["SearchDate"],
                    SearchedBy = reader["SearchedBy"].ToString()!
                });
            }

            return Ok(searches);
        }

        [HttpPut("update")]
        public IActionResult UpdateWeather([FromBody] UpdateWeatherRequest request)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_UpdateWeatherSearch", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SearchId", request.SearchId);
            command.Parameters.AddWithValue("@CityName", request.CityName);
            command.Parameters.AddWithValue("@Humidity", request.Humidity);
            command.Parameters.AddWithValue("@TempMin", request.TempMin);
            command.Parameters.AddWithValue("@TempMax", request.TempMax);
            command.Parameters.AddWithValue("@ChangedById", request.ChangedByUserId);

            connection.Open();
            command.ExecuteNonQuery();
            return Ok(new { Message = "Updated successfully" });
        }
    }
}