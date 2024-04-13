using HomeLink.Management.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HomeLink.Management.WebApi.Models;

public static class EntityResponseExtensions
{
    public static IActionResult ToActionResult(this EntityResult response)
    {
        return response.IsValid ? new OkObjectResult(response) : new BadRequestObjectResult(response);
    }
}