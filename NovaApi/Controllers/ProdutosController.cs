using Microsoft.AspNetCore.Components;

[Route("api/[controller]")]
public class ProdutosController : Controller<Produto>
{
    public ProdutosController(AppDbContext context) : base(context) {}
}