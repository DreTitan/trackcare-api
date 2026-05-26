using FluentValidation;
using TrackCare.Application.DTOs;

namespace TrackCare.Application.Validators;

public class CreateComentarioValidator : AbstractValidator<CreateComentarioDto>
{
    public CreateComentarioValidator()
    {
        RuleFor(x => x.RecolhimentoId)
            .GreaterThan(0).WithMessage("RecolhimentoId inválido");

        RuleFor(x => x.Texto)
            .NotEmpty().WithMessage("Texto do comentário é obrigatório")
            .MaximumLength(2000);
    }
}
