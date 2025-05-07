using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Categoria> Categorias { get; set;}
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Extra> Extras { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Mesa> Mesas { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ItemPedido>().HasKey(ip => new {ip.ProdutoId, ip.PedidoId});
        
        // Categorias
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nome = "Pizzas Salgadas" },
            new Categoria { Id = 2, Nome = "Pizzas Doces" },
            new Categoria { Id = 3, Nome = "Entradas e Petiscos" },
            new Categoria { Id = 4, Nome = "Refrigerantes" },
            new Categoria { Id = 5, Nome = "Sucos Naturais" },
            new Categoria { Id = 6, Nome = "Água" },
            new Categoria { Id = 7, Nome = "Cervejas" },
            new Categoria { Id = 8, Nome = "Vinhos" },
            new Categoria { Id = 9, Nome = "Sobremesas" }
        );

        // Mesas
        modelBuilder.Entity<Mesa>().HasData(
            new Mesa { Id = 1, Nome = "Mesa 01" },
            new Mesa { Id = 2, Nome = "Mesa 02" },
            new Mesa { Id = 3, Nome = "Mesa 03" },
            new Mesa { Id = 4, Nome = "Mesa 04" },
            new Mesa { Id = 5, Nome = "Mesa 05" },
            new Mesa { Id = 6, Nome = "Mesa 06" },
            new Mesa { Id = 7, Nome = "Mesa 07" }
        );

        modelBuilder.Entity<Produto>().HasData(

            new Produto { Id = 1, Nome = "Calabresa", Descricao = "Molho de tomate, mussarela, rodelas de calabresa de primeira qualidade e cebola fatiada", Preco = 30.00m, Foto = "./imgs/pizza_calabresa.jpg", CategoriaId = 1 },
            new Produto { Id = 2, Nome = "Marguerita", Descricao = "Molho de tomate, mussarela, rodelas de tomate fresco, manjericão fresco e um toque de parmesão", Preco = 32.00m, Foto = "./imgs/pizza-marguerita.jpg", CategoriaId = 1 },
            new Produto { Id = 3, Nome = "Portuguesa", Descricao = "Molho de tomate, mussarela, presunto, ovos cozidos, cebola, azeitonas pretas e orégano", Preco = 35.00m, Foto = "./imgs/pizza-portuguesa.jpg", CategoriaId = 1 },

            new Produto { Id = 4, Nome = "Chocolate Preto", Descricao = "Delicioso chocolate ao leite derretido (opcional: granulado)", Preco = 30.00m, Foto = "./imgs/pizza-chocolate.jpg", CategoriaId = 2 },
            new Produto { Id = 5, Nome = "Chocolate Branco com Morango", Descricao = "Chocolate branco derretido com morangos frescos fatiados", Preco = 35.00m, Foto = "./imgs/pizza-choco-morango.jpg", CategoriaId = 2 },

            new Produto { Id = 6, Nome = "Pão de Alho Tradicional", Descricao = "Pão baguete com pasta de alho caseira, gratinado com queijo (Unidade)", Preco = 8.00m, Foto = "./imgs/pao-alho.jpg", CategoriaId = 3 },
            new Produto { Id = 7, Nome = "Calabresa Acebolada", Descricao = "Porção de calabresa fatiada e salteada com cebola. Acompanha pão.", Preco = 38.00m, Foto = "./imgs/calabresa-acebolada.jpg", CategoriaId = 3 },

            new Produto { Id = 8, Nome = "Coca-Cola", Descricao = "Lata 350ml", Preco = 6.00m, Foto = "./imgs/coca-cola-lata.jpg", CategoriaId = 4 },
            new Produto { Id = 9, Nome = "Guaraná Antarctica", Descricao = "Lata 350ml", Preco = 6.00m, Foto = "./imgs/guarana-lata.jpg", CategoriaId = 4 },

            new Produto { Id = 10, Nome = "Suco de Laranja", Descricao = "Natural - Copo 400ml", Preco = 9.00m, Foto = "./imgs/suco-laranja.jpg", CategoriaId = 5 },
            new Produto { Id = 11, Nome = "Suco de Abacaxi", Descricao = "Polpa/Natural - Copo 400ml", Preco = 9.00m, Foto = "./imgs/suco-abacaxi.jpg", CategoriaId = 5 },

            new Produto { Id = 12, Nome = "Água Mineral Sem Gás", Descricao = "Garrafa 500ml", Preco = 4.00m, Foto = "./imgs/agua-sem-gas.jpg", CategoriaId = 6 },
            new Produto { Id = 13, Nome = "Água Mineral Com Gás", Descricao = "Garrafa 500ml", Preco = 4.50m, Foto = "./imgs/agua-com-gas.jpg", CategoriaId = 6 },

            new Produto { Id = 14, Nome = "Skol", Descricao = "Lata 350ml", Preco = 7.00m, Foto = "./imgs/cerveja-skol.jpg", CategoriaId = 7 },
            new Produto { Id = 15, Nome = "Brahma", Descricao = "Lata 350ml", Preco = 7.00m, Foto = "./imgs/cerveja-brahma.jpg", CategoriaId = 7 },

            new Produto { Id = 16, Nome = "Vinho Tinto da Casa", Descricao = "Taça - Cabernet Sauvignon ou Merlot", Preco = 20.00m, Foto = "./imgs/vinho-tinto-taca.jpg", CategoriaId = 8 },
            new Produto { Id = 17, Nome = "Vinho Branco da Casa", Descricao = "Taça - Sauvignon Blanc", Preco = 20.00m, Foto = "./imgs/vinho-branco-taca.jpg", CategoriaId = 8 },

            new Produto { Id = 18, Nome = "Mousse de Maracujá", Descricao = "Mousse de maracujá com açúcar", Preco = 12.00m, Foto = "./imgs/mousse-maracuja.jpg", CategoriaId = 9 },
            new Produto { Id = 19, Nome = "Açaí na Tigela", Descricao = "300ml - Açaí com granola e banana", Preco = 22.00m, Foto = "./imgs/acai-tigela.jpg", CategoriaId = 9 }
        );
    }
}