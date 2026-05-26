using Xunit;
using FluentValidation;
using TrackCare.Application.DTOs;
using TrackCare.Application.Validators;

namespace TrackCare.Application.Tests.Validators;

public class CreateRecolhimentoValidatorTests
{
    private readonly CreateRecolhimentoValidator _validator;

    public CreateRecolhimentoValidatorTests()
    {
        _validator = new CreateRecolhimentoValidator();
    }

    private static CreateRecolhimentoDto MakeValid() => new(
        Hgid: "HG001",
        NumeroSerie: "SN123456",
        Modelo: "Modelo X",
        ClienteNome: "Cliente Teste",
        ClienteContato: "João Silva",
        ClienteEmail: "joao@teste.com",
        ClienteTelefone: "11999999999",
        ClientePlano: "GoPremium",
        TicketHub: "HUB-001",
        TicketBlip: "BLIP-001",
        DescricaoProblema: "Problema no sensor",
        RelatorioN3: "Análise técnica completa",
        JaRecolhido: false,
        DataPrevistaColeta: null,
        Observacoes: "Observação adicional",
        Usuario: "admin"
    );

    [Fact]
    public void Valid_dto_should_pass()
    {
        var dto = MakeValid();
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Empty_hgid_should_fail()
    {
        var dto = MakeValid() with { Hgid = "" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Hgid");
    }

    [Fact]
    public void Empty_numeroSerie_should_fail()
    {
        var dto = MakeValid() with { NumeroSerie = "" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "NumeroSerie");
    }

    [Fact]
    public void Empty_clienteNome_should_fail()
    {
        var dto = MakeValid() with { ClienteNome = "" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ClienteNome");
    }

    [Fact]
    public void Hgid_too_long_should_fail()
    {
        var dto = MakeValid() with { Hgid = new string('A', 51) };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Hgid");
    }

    [Fact]
    public void DescricaoProblema_too_long_should_fail()
    {
        var dto = MakeValid() with { DescricaoProblema = new string('A', 2001) };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "DescricaoProblema");
    }
}
