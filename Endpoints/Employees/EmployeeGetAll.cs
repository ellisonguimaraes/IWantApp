using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromQuery] int page, [FromQuery] int rows, [FromServices] IConfiguration configuration)
    {
        var db = new SqlConnection(configuration.GetConnectionString("SqlServerConnectionString"));

        var query = "SELECT Email, ClaimValue as Name FROM AspNetUsers u INNER JOIN AspNetUserClaims c " +
                    "ON u.Id = c.UserId and ClaimType = 'Name' " +
                    "ORDER BY Name " +
                    "OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        var employees = db.Query<EmployeeResponse>(query, new { page, rows });
          
        return Results.Ok(employees);
    }
}
