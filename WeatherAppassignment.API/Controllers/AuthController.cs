using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WeatherApp.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace WeatherApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly string _connectionString;

        public AuthController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Username and password are required");

            var hashedPassword = HashPassword(request.Password);

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_ValidateUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username", request.Username);
            command.Parameters.AddWithValue("@Password", hashedPassword);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var user = new User
                {
                    UserId = (int)reader["UserId"],
                    Username = reader["Username"].ToString()!,
                    FullName = reader["FullName"].ToString()!
                };
                return Ok(user);
            }

            return Unauthorized("Invalid username or password");
        }
    }
}