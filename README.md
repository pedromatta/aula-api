# Introdução

Vamos criar uma WebAPI com ASP.NET Core, conectada ao MySQL usando o Entity Framework Core. Vamos mapear nosso banco de dados relacional com migrations e fazer endpoints para testes básicos.

## Estrutura de Dados

Antes de começarmos, temos que ter nossa estrutura do banco de dados pronta, o banco de dados não deve estar implementado, mas toda a estrutura deve estar completa, para sabermos como as tabelas devem ficar no banco de dados final:

```sql
CREATE TABLE Categorias (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL
);

CREATE TABLE Produtos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Descricao VARCHAR(255),
    Preco DECIMAL(10,2) NOT NULL,
    Foto VARCHAR(255),
    CategoriaId INT NOT NULL,
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);

CREATE TABLE Extras (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    PrecoAdicional DECIMAL(10,2) NOT NULL,
    ProdutoId INT NOT NULL,
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id)
);

CREATE TABLE Funcionarios (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Usuario VARCHAR(50) UNIQUE NOT NULL,
    Senha VARCHAR(60) NOT NULL
);

CREATE TABLE Mesas (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(50) NOT NULL
);

CREATE TABLE Pedidos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    MesaId INT NOT NULL,
    FuncionarioId INT NOT NULL,
    DataHoraInicio DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DataHoraFim DATETIME,
    FOREIGN KEY (MesaId) REFERENCES Mesas(Id),
    FOREIGN KEY (FuncionarioId) REFERENCES Funcionarios(Id)
);

CREATE TABLE ItensPedido (
    PRIMARY KEY (PedidoId, ProdutoId),
    PedidoId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL DEFAULT 1,
    PrecoUnitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id),
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id)
);
```

Relações;

- Categorias -> Produtos (1:N)
- Produtos -> Extras (1:N)
- Funcionários e Mesas -> Pedidos (1:N)
- Pedidos e Produtos -> ItensPedido(N:M)

## Passos

1. Iremos criar nosso projeto e instalar os pacotes necessários para a comunicação com o banco de dados e os testes da API.
2. Vamos mapear as nossas entidades do banco de dados para classes correspondentes às tabelas e marcá-las com o uso de DataAnnotations.
3. Vamos criar o nosso DbContext, para traduzir o contexto da aplicação para o banco de dados e vice-versa.
4. Vamos criar e aplicar migrations, scripts do EF Core que nos permitem guardar o histórico da criação e alterações do nosso banco de dados.
5. Vamos criar Controllers para mapear nossos endpoints e permitir a comunicação com o front-end
6. Vamos testar nossa aplicação com o Swagger para verificar que as requisições serão respondidas da maneira correta.


# Preparando o Ambiente

## Criando o Projeto

Para criar nosso projeto, podemos utilizar qualquer editor de código, a melhor opção é o Visual Studio, a versão community pode ser baixada gratuitamente pelo link: [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/). Eu usarei o VS Code e instalarei os pacotes pela CLI, para que as instruções sejam aplicáveis a qualquer ambiente, mas utilizar o Visual Studio pode facilitar muito processo de criação do projeto e instalação dos pacotes necessários.

Começamos criando a aplicação. A versão do .NET Core 8.0 é a LTS mais recente, para não enfrentarmos problemas com incompatibilidade de versão, é melhor usarmos ela, se a versão que você tiver instalada no seu sistema for diferente, você pode instalar a 8.0 pelo link [Download do .NET](https://dotnet.microsoft.com/pt-br/download). No Visual Studio, basta escolher a opção ASP.NET Core Web API no tipo de solução ao criar o projeto, pela CLI, utilizamos o comando:

```sh
dotnet new webapi -f "net8.0" -n NomeDaAPI 
```

Movemos para o diretório da nossa solução:

```sh
cd NomeDaAPI
```

## Instalando os Pacotes

No diretório da API podemos instalar todos os pacotes que serão necessários:

```sh
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.3 # Se estiver utilizando outro banco de dados, coloque o pacote EntityFrameworkCore correspondente.
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.13
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.13
dotnet add package Swashbuckle.AspNetCore
```

## Organizando o projeto

Para organizar melhor nosso projeto, podemos já criar os diretórios que serão necessários:

```sh
mkdir Models
mkdir Data 
mkdir Controllers
```

# Criando os Models

Agora nós já podemos começar a criar nossa solução, o primeiro passo é criar os modelos das classes, cada modelo é correspondente a uma tabela de nosso banco de dados:

```cs
// Models/Produto.cs
using System.ComponentModel.DataAnnotations; // Não é estritamente necessário mapear cada informação, mas pode ser útil para prevenir erros
using System.ComponentModel.DataAnnotations.Schema;

public class Produto
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
    public decimal Preco { get; set; }

    [MaxLength(255)]
    public string? Foto { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public Categoria? Categoria { get; set; } // A referencia da ForeignKey é feito com uma propriedade de navegação, criando um objeto da classe um na classe muitos.

    public ICollection<Extra>? Extras { get; set ; }
}

```cs
// Models/Categoria.cs
using System.ComponentModel.DataAnnotations;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public ICollection<Produto>? Produtos { get; set; } // Em uma relação um pra muitos, uma coleção de objetos da entidade muitos é criada na entidade um.
}

// Cada entidade do banco de dados deve ser representada como uma classe da aplicação.
```

# Criando o DbContext

Para mapear as classes do código para entidades de bancos de dados, temos que dar um contexto para a aplicação, e é exatamente isso que precisamos criar um DbContext, o DbContext é uma classe do .NET que é usada para mapear as entidades e fazer o processo de comunicação do banco de dados para a aplicação e vice-versa. Então, no nosso diretório Data/ criaremos o nosso contexto da aplicação o AppDbContext, nesse contexto iremos settar no banco de dados a nossa estrutura de classe com o tipo DbSet<T>, para facilitar a conversão dos dados, podemos criar regras para os relacionamentos com a função OnModelCreating():

```cs
// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Extra> Extras { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Mesa> Mesas { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }

    // A função OnModelCreating do DbContext pode definir regras extras para a modelagem das tabelas, como, nesse caso, a aplicação 
    // das foreign keys como primary key de uma tabela
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ItemPedido>().HasKey(ip => new {ip.ProdutoId, ip.PedidoId});
    }
}
```

## Criando a connection string

Precisamos conectar nossa aplicação ao nosso banco de dados, para isso, precisamos de uma conection string, que podemos construir com informações que sabemos sobre nosso servidor de banco de dados, precisamos do servidor em que o banco está hospedado, no caso dessa aula ele está no localhost, precisamos da porta em que o servidor está hospedada, no meu caso 3304, precisamos definir o nome do banco de dados, nesse caso "restaurante" e do nome de usuário e senha de um usuário que tenha as permissões necessárias no banco de dados. Agora vamos para o appsettings.json aplicar essa connection string:

!Importante: esse é um caso de teste, nunca armazene as credenciais no appsettings.json em cenários de produção, para cenários de produção, o recomendado é usar variáveis de ambiente no servidor em que sua aplicação estará hospedada, você pode ver mais detalhes sobre como armazenar a connection string na [Azure](https://learn.microsoft.com/en-us/azure/app-service/configure-common?tabs=portal#configure-connection-strings) ou como configurar variáveis de ambiente na [AWS](https://docs.aws.amazon.com/elasticbeanstalk/latest/dg/environments-cfg-softwaresettings.html).

```json
// appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
        "DefaultConnection" : "Server=localhost;port=3306;database=restaurante;User=api_user;Password=insira-senha-forte;"
    }
}
```

## Adicionando o contexto no Program.cs

Agora podemos ir no Program.cs para adicionar o contexto:

```cs
// Program.cs
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

app.Run();
```

# Criando as Migrations

No EF Core temos as Migrations, elas são scripts que permitem que nós façamos alterações na estrutura do banco de dados de forma sistemática e reproduzível, elas permitem que nós modifiquemos nossas tabelas, colunas e chaves de forma a manter o histórico de alterações e permitir que o banco seja completamente reconstruído mesmo se tivermos que mudar seu endereço, então, se formos mudar nosso banco de dados do nosso localhost para a nuvem da AWS por exemplo, não precisamos remontar todo o banco de dados e ainda teremos nosso histórico de alterações. Para aplicar as Migrations, basta usarmos alguns comandos no terminal:

```sh
dotnet ef migrations add InitialCreate # cada vez que criamos uma Migration, estamos pegando a situação atual do nosso banco de dados e alterações e salvando nesse script, nossa primeira migration pode ser a de criação do banco de dados.
dotnet ef database update # sempre que criamos uma nova Migration, temos que atualizr o banco de dados para que as mudanças sejam aplicadas.
```

## Configurando o OnModelCreating

As migrations irão criar nosso banco de dados da forma que definirmos em nosso contexto, então, no nosso caso atual, elas irão estruturar as tabelas de acordo com os nomes e tipos do nosso DbSet<T>, mas a criação de tabelas não é a única coisa que pode ser controlada pelas migrations, podemos também definir regras para a criação das tabelas, como povoar certas tabelas com informações que queremos que sejam restauradas em qualquer situação que vincularmos a aplicação a outro banco de dados, ou definir a primary key de uma tabela muitos para muitos como a junção das foreign keys. Essa configuração pode ser feita por uma função base do `DbContext`, a `OnModelCreating(ModelBuilder modelBuilder)`, a documentação oficial com os métodos do modelBuilder estão disponíveis no link: [ModelBuilder Class](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.modelbuilder?view=efcore-8.0).

Por exemplo, se eu quiser que toda vez que eu criar um novo banco de dados restaurante a tabela ItemPedido tenha as chaves de ProdutoId e PedidoId como primary key, e que o banco já esteja povoado com certas categorias, mesas e produtos, eu posso adicionar o seguinte em meu `AppDbContext`:

```cs
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    //
    // Os DbSets criados anteriormente estão aqui
    //

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
```

## Aplicando nova Migration

Para aplicarmos essas alterações, podemos criar uma nova migration e atualizar a database:

```sh
dotnet ef migrations add SeedInitial 
dotnet ef database update 
```

# Criando os Controllers

Para finalizarmos nossa aplicação, precisamos criar os nossos Controllers, são eles que nos permitirão responder às requisições Http feitas pelo front-end, para facilitar a criação, podemos criar um Controller genérico que será referenciado pelas outras classes:

```cs
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
        return prop?.GetValue(entity);
    }

    public Controller(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TEntity>>> GetAll() 
    {
        var entities = await _dbSet.ToListAsync();
        if(entities == null) return NotFound();
        return Ok(entities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TEntity>> Get(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if(entity == null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<TEntity>> Post(TEntity entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = GetEntityId(entity)}, entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, TEntity entity)
    {
        if(!id.Equals(GetEntityId(entity)))
            return BadRequest();

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return NotFound();

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
```

Cada entidade precisará de um controller próprio que use esse Controller genérico aplicando o tipo da entidade na herança:

```cs
using Microsoft.AspNetCore.Components;

[Route("api/[controller]")]
public class ProdutosController : Controller<Produto>
{
    public ProdutosController(AppDbContext context) : base(context) {}
}
```

Caso você tenha alguma entidade que não precisa, não deve fazer um CRUD completo, ou precisa de funções diferentes. Você deve criar um Controller inteiro novo para ela, e fazer as funções das requisições que você quer que ela receba, basta escolher o método Http para cada função. Por exemplo, eu não quero que os pedidos sejam acessados por completo, quero que apenas os pedidos em Aberto sejam alteráveis e também quero filtrar os pedidos por datas para gerar os relatórios sem a necessidade de aplicar essa lógica no front-end, para isso, eu preciso que meu PedidosController, tenha um comportamento diferente das demais entidades:

```cs
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
    public async Task<IActionResult> AtualizarPedido(int id, Pedido pedidoAtualizado)
    {
        if(!id.Equals(pedidoAtualizado.Id) || pedidoAtualizado.DataHoraFim != null)
            return BadRequest();

        if(pedidoAtualizado == null || pedidoAtualizado.DataHoraFim != null)
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
        return NoContent();
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
```

# Testando os Endpoints

Nossa aplicação está quase completa, só precisamos finalizar o Program.cs para que possamos testar os nossos endpoints:

```cs
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adiciona o DbContext com MySQL (Pomelo)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Habilita CORS para qualquer origem (útil para testes)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Adiciona serviços de controller
builder.Services.AddControllers();

// Adiciona e configura Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });
});

var app = builder.Build();

// Ativa o Swagger em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

## Testando no Swagger

Para rodarmos nossa aplicação, iremos executá-la da mesma forma que executamos uma aplicação de console:

```sh
dotnet run
```

Ao executarmos esse comando, nossa aplicação irá ser executada, nosso servidor então estará sendo servido na porta 5174, ou em outra, se essa estiver em uso. Você pode verificar em qual porta ele está sendo executado pelo output do `dotnet run`:

```sh
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5174
```

O endereço da aplicação está, então, no `localhost:5174`, se colocarmos esse endereço no navegador e adicionarmos o `/swagger/` poderemos acessar o SwaggerUI e testar os endpoints da nossa API por ele.
