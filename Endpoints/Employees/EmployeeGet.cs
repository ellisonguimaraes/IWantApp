using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGet
{
    public static string Template => "/employees/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => ActionAsync;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> ActionAsync([FromRoute] string id, [FromServices] IConfiguration configuration)
    {
        var db = new SqlConnection(configuration.GetConnectionString("SqlServerConnectionString"));

        var query = "SELECT u.Id as Id, Email, ClaimValue as Name FROM AspNetUsers u INNER JOIN AspNetUserClaims c " +
                    "ON u.Id = @id";

        var employee = (await db.QueryAsync<EmployeeResponse>(query, new { id })).FirstOrDefault();

        if (employee == null)
            return Results.NotFound();

        return Results.Ok(employee);
    }
}
