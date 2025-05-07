using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NovaAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Pizzas Salgadas" },
                    { 2, "Pizzas Doces" },
                    { 3, "Entradas e Petiscos" },
                    { 4, "Refrigerantes" },
                    { 5, "Sucos Naturais" },
                    { 6, "Água" },
                    { 7, "Cervejas" },
                    { 8, "Vinhos" },
                    { 9, "Sobremesas" }
                });

            migrationBuilder.InsertData(
                table: "Mesas",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Mesa 01" },
                    { 2, "Mesa 02" },
                    { 3, "Mesa 03" },
                    { 4, "Mesa 04" },
                    { 5, "Mesa 05" },
                    { 6, "Mesa 06" },
                    { 7, "Mesa 07" }
                });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "CategoriaId", "Descricao", "Foto", "Nome", "Preco" },
                values: new object[,]
                {
                    { 1, 1, "Molho de tomate, mussarela, rodelas de calabresa de primeira qualidade e cebola fatiada", "./imgs/pizza_calabresa.jpg", "Calabresa", 30.00m },
                    { 2, 1, "Molho de tomate, mussarela, rodelas de tomate fresco, manjericão fresco e um toque de parmesão", "./imgs/pizza-marguerita.jpg", "Marguerita", 32.00m },
                    { 3, 1, "Molho de tomate, mussarela, presunto, ovos cozidos, cebola, azeitonas pretas e orégano", "./imgs/pizza-portuguesa.jpg", "Portuguesa", 35.00m },
                    { 4, 2, "Delicioso chocolate ao leite derretido (opcional: granulado)", "./imgs/pizza-chocolate.jpg", "Chocolate Preto", 30.00m },
                    { 5, 2, "Chocolate branco derretido com morangos frescos fatiados", "./imgs/pizza-choco-morango.jpg", "Chocolate Branco com Morango", 35.00m },
                    { 6, 3, "Pão baguete com pasta de alho caseira, gratinado com queijo (Unidade)", "./imgs/pao-alho.jpg", "Pão de Alho Tradicional", 8.00m },
                    { 7, 3, "Porção de calabresa fatiada e salteada com cebola. Acompanha pão.", "./imgs/calabresa-acebolada.jpg", "Calabresa Acebolada", 38.00m },
                    { 8, 4, "Lata 350ml", "./imgs/coca-cola-lata.jpg", "Coca-Cola", 6.00m },
                    { 9, 4, "Lata 350ml", "./imgs/guarana-lata.jpg", "Guaraná Antarctica", 6.00m },
                    { 10, 5, "Natural - Copo 400ml", "./imgs/suco-laranja.jpg", "Suco de Laranja", 9.00m },
                    { 11, 5, "Polpa/Natural - Copo 400ml", "./imgs/suco-abacaxi.jpg", "Suco de Abacaxi", 9.00m },
                    { 12, 6, "Garrafa 500ml", "./imgs/agua-sem-gas.jpg", "Água Mineral Sem Gás", 4.00m },
                    { 13, 6, "Garrafa 500ml", "./imgs/agua-com-gas.jpg", "Água Mineral Com Gás", 4.50m },
                    { 14, 7, "Lata 350ml", "./imgs/cerveja-skol.jpg", "Skol", 7.00m },
                    { 15, 7, "Lata 350ml", "./imgs/cerveja-brahma.jpg", "Brahma", 7.00m },
                    { 16, 8, "Taça - Cabernet Sauvignon ou Merlot", "./imgs/vinho-tinto-taca.jpg", "Vinho Tinto da Casa", 20.00m },
                    { 17, 8, "Taça - Sauvignon Blanc", "./imgs/vinho-branco-taca.jpg", "Vinho Branco da Casa", 20.00m },
                    { 18, 9, "Mousse de maracujá com açúcar", "./imgs/mousse-maracuja.jpg", "Mousse de Maracujá", 12.00m },
                    { 19, 9, "300ml - Açaí com granola e banana", "./imgs/acai-tigela.jpg", "Açaí na Tigela", 22.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Mesas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mesas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Mesas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Mesas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Mesas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Mesas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Mesas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
