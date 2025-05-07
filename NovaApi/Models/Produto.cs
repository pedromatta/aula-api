using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = "";

    [StringLength(255)]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [StringLength(255)]
    public string? Foto { get; set; }

    [Required]
    public int CategoriaId {get; set; }

    [ForeignKey("CategoriaId")]
    public Categoria? Categoria { get; set; }

    public ICollection<Extra>? Extras { get; set; }
    public ICollection<ItemPedido>? ItensPedido { get; set; }
}