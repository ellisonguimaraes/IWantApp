using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => ActionAsync;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> ActionAsync([FromQuery] int page, [FromQuery] int rows, [FromServices] IConfiguration configuration)
    {
        var db = new SqlConnection(configuration.GetConnectionString("SqlServerConnectionString"));

        var query = "SELECT u.Id as Id, Email, ClaimValue as Name FROM AspNetUsers u INNER JOIN AspNetUserClaims c " +
                    "ON u.Id = c.UserId and ClaimType = 'Name' " +
                    "ORDER BY Name " +
                    "OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        var employees = await db.QueryAsync<EmployeeResponse>(query, new { page, rows });

        return Results.Ok(employees);
    }
}
