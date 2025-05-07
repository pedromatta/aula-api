using System.ComponentModel.DataAnnotations;
public class Funcionario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Usuario { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string Senha { get; set; } = string.Empty;

    public ICollection<Pedido>? Pedidos { get; set; }
}