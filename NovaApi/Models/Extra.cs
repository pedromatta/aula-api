using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Extra
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Descricao { get; set;}

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoAdicional { get; set; }
    
    [Required]
    public int ProdutoId { get; set; }

    [ForeignKey("ProdutoId")]
    public Produto? Produto { get; set; }
}