using Microsoft.AspNetCore.Components;

[Route("api/[controller]")]
public class ItensPedidoController : Controller<ItemPedido>
{
    public ItensPedidoController(AppDbContext context) : base(context) {}
}