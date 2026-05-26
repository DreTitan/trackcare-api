using FluentValidation;
using TrackCare.Application.DTOs;

namespace TrackCare.Application.Validators;

public class CreateRecolhimentoValidator : AbstractValidator<CreateRecolhimentoDto>
{
    public CreateRecolhimentoValidator()
    {
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

        RuleFor(x => x.TicketHub)
            .MaximumLength(50);

        RuleFor(x => x.TicketBlip)
            .MaximumLength(50);

        RuleFor(x => x.DescricaoProblema)
            .MaximumLength(2000);

        RuleFor(x => x.RelatorioN3)
            .MaximumLength(4000);

        RuleFor(x => x.Observacoes)
            .MaximumLength(4000);
    }
}
