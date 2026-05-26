using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TrackCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "recolhimentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Hgid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumeroSerie = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: true),
                    ClienteNome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClienteContato = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ClienteEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ClienteTelefone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ClientePlano = table.Column<int>(type: "integer", nullable: false),
                    TicketHub = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TicketBlip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DescricaoProblema = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RelatorioN3 = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    JaRecolhido = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPrevistaColeta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataColetaReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataPrevistaDevolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataDevolucaoReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    CriadoPor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recolhimentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "anexos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecolhimentoId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    NomeOriginal = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NomeArquivo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioUpload = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DataUpload = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CaminhoCompleto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anexos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_anexos_recolhimentos_RecolhimentoId",
                        column: x => x.RecolhimentoId,
                        principalTable: "recolhimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecolhimentoId = table.Column<int>(type: "integer", nullable: false),
                    Texto = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Setor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comentarios_recolhimentos_RecolhimentoId",
                        column: x => x.RecolhimentoId,
                        principalTable: "recolhimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "historico_status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecolhimentoId = table.Column<int>(type: "integer", nullable: false),
                    StatusAnterior = table.Column<int>(type: "integer", nullable: false),
                    StatusNovo = table.Column<int>(type: "integer", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico_status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_historico_status_recolhimentos_RecolhimentoId",
                        column: x => x.RecolhimentoId,
                        principalTable: "recolhimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_anexos_RecolhimentoId",
                table: "anexos",
                column: "RecolhimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_comentarios_RecolhimentoId",
                table: "comentarios",
                column: "RecolhimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_historico_status_RecolhimentoId",
                table: "historico_status",
                column: "RecolhimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_recolhimentos_ClienteNome",
                table: "recolhimentos",
                column: "ClienteNome");

            migrationBuilder.CreateIndex(
                name: "IX_recolhimentos_DataSolicitacao",
                table: "recolhimentos",
                column: "DataSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_recolhimentos_Hgid",
                table: "recolhimentos",
                column: "Hgid");

            migrationBuilder.CreateIndex(
                name: "IX_recolhimentos_NumeroSerie",
                table: "recolhimentos",
                column: "NumeroSerie");

            migrationBuilder.CreateIndex(
                name: "IX_recolhimentos_Status",
                table: "recolhimentos",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anexos");

            migrationBuilder.DropTable(
                name: "comentarios");

            migrationBuilder.DropTable(
                name: "historico_status");

            migrationBuilder.DropTable(
                name: "recolhimentos");
        }
    }
}
