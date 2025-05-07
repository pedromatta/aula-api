using Microsoft.AspNetCore.Components;

[Route("api/[controller]")]
public class ExtrasController : Controller<Extra>
{
    public ExtrasController(AppDbContext context) : base(context) {}
}