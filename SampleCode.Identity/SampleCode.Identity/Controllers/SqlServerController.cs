using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SampleCode.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SqlServerController(IConfiguration configuration, ILogger<WeatherForecastController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var conStr = configuration["SqlServer"];
            IDbConnection connection = new SqlConnection(conStr);
            var query = """
                        Select PersonnelCode From Users Where UserId = 27
                        """;

            var personnelCode = connection.Query<string>(query).SingleOrDefault();

            if (personnelCode == "980923")
            {
                return Ok("Pong from SQL-Server");
            }

            return Ok("Something is wrong. Couldn't correctly connect to DB");
        }
    }
}
