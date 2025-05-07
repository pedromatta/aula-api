using Microsoft.AspNetCore.Components;

[Route("api/[controller]")]
public class FuncionariosController : Controller<Funcionario>
{
    public FuncionariosController(AppDbContext context) : base(context) {}
}