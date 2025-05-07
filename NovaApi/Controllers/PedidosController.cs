using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly AppDbContext _context;
    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPedidosAbertos()
    {
        var pedidos = await _context.Pedidos
            .Where(p => p.DataHoraFim == null)
            .ToListAsync();
        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPedido(int id)
    {
        var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);

        if(pedido == null)
           return NotFound(); 
        if(pedido.DataHoraFim != null)
           return BadRequest();

        return Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> CriarPedido(Pedido pedido)
    {
        pedido.DataHoraInicio = DateTime.Now;
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarPedido(int id,[FromBody] Pedido pedidoAtualizado)
    {
        if(!id.Equals(pedidoAtualizado.Id) || pedidoAtualizado.DataHoraFim != null)
            return BadRequest();

        if(pedidoAtualizado == null)
            return NotFound("Pedido não encontrado.");

        _context.Entry(pedidoAtualizado).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}/fechar")]
    public async Task<IActionResult> FecharPedido(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if(pedido == null)
            return NotFound();      
        if(pedido.DataHoraFim != null)
            return BadRequest();

        pedido.DataHoraFim = DateTime.Now;

        _context.Entry(pedido).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(pedido);
    }

    [HttpGet("relatorio/periodo")]
    public async Task<IActionResult> RelatorioPorPeriodo(DateTime inicio, DateTime fim)
    {
        var pedidos = await _context.Pedidos
            .Where(p => p.DataHoraFim != null && p.DataHoraFim >= inicio && p.DataHoraFim <= fim)
            .ToListAsync();
        return Ok(pedidos);
    }

     [HttpGet("relatorio/dia")]
    public async Task<IActionResult> RelatorioPorDia(DateTime dia)
    {
        var pedidos = await _context.Pedidos
            .Where(p => p.DataHoraFim != null && p.DataHoraFim.Value.Date == dia.Date)
            .ToListAsync();
        return Ok(pedidos);
    }
}
