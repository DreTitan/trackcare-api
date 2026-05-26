using Xunit;
using FluentValidation;
using TrackCare.Application.DTOs;
using TrackCare.Application.Validators;

namespace TrackCare.Application.Tests.Validators;

public class CreateComentarioValidatorTests
{
    private readonly CreateComentarioValidator _validator;

    public CreateComentarioValidatorTests()
    {
        _validator = new CreateComentarioValidator();
    }

    [Fact]
    public void Valid_comentario_should_pass()
    {
        var dto = new CreateComentarioDto(
            RecolhimentoId: 1,
            Texto: "Teste de comentário",
            Usuario: "admin",
            Setor: "N3"
        );
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Empty_texto_should_fail()
    {
        var dto = new CreateComentarioDto(1, "", "admin", "N3");
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Texto");
    }

    [Fact]
    public void Zero_recolhimentoId_should_fail()
    {
        var dto = new CreateComentarioDto(0, "Texto válido", "admin", "N3");
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "RecolhimentoId");
    }

    [Fact]
    public void Texto_too_long_should_fail()
    {
        var dto = new CreateComentarioDto(1, new string('A', 2001), "admin", "N3");
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Texto");
    }
}
