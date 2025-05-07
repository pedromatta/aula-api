using System.ComponentModel.DataAnnotations;

public class Mesa
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nome { get; set; } = string.Empty;

    public ICollection<Pedido>? Pedidos { get; set; }
}
