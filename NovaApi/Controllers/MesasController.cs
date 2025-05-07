using Microsoft.AspNetCore.Components;

[Route("api/[controller]")]
public class MesasController : Controller<Mesa>
{
    public MesasController(AppDbContext context) : base(context) {}
}