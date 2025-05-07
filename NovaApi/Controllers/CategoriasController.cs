using Microsoft.AspNetCore.Components;

[Route("api/[controller]")]
public class CategoriasController : Controller<Categoria>
{
    public CategoriasController(AppDbContext context) : base(context) {}
}