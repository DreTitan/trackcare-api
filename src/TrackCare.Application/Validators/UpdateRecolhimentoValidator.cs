using FluentValidation;
using TrackCare.Application.DTOs;

namespace TrackCare.Application.Validators;

public class UpdateRecolhimentoValidator : AbstractValidator<UpdateRecolhimentoDto>
{
    public UpdateRecolhimentoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID inválido");

        RuleFor(x => x.Hgid)
            .NotEmpty().WithMessage("HGID é obrigatório")
            .MaximumLength(50);

        RuleFor(x => x.NumeroSerie)
            .NotEmpty().WithMessage("Número de série é obrigatório")
            .MaximumLength(100);

        RuleFor(x => x.ClienteNome)
            .NotEmpty().WithMessage("Nome do cliente é obrigatório")
            .MaximumLength(200);

        RuleFor(x => x.ClientePlano)
            .NotEmpty().WithMessage("Plano é obrigatório");
    }
}
