using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ItemPedido
{
    [Required]
    public int PedidoId { get; set; }
    [ForeignKey("PedidoId")]
    public Pedido Pedido { get; set; } = null!;

    [Required]
    public int ProdutoId { get; set; }
    [ForeignKey("ProdutoId")]
    public Produto Produto { get; set; } = null!;

    [Required]
    public int Quantidade { get; set; } = 1;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoUnitario { get; set; }
}
