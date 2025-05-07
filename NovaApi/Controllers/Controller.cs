using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class Controller<TEntity> : ControllerBase where TEntity : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    private object GetEntityId(TEntity entity)
    {
        var prop = typeof(TEntity).GetProperty("Id");
        return prop?.GetValue(entity)!;
    }

    public Controller(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
    {
        var entities = await _dbSet.ToListAsync();
        if(entities == null) return NotFound();
        return Ok(entities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TEntity>> GetById(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if(entity == null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<TEntity>> Post([FromBody] TEntity entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TEntity>> Put(int id, [FromBody] TEntity entity)
    {
        if(!id.Equals(GetEntityId(entity)))
        {
            return BadRequest(); 
        }

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TEntity>> Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if(entity == null) return NotFound();

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}