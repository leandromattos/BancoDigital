namespace BancoDigitalAPI.Validators
{
    using BancoDigitalAPI.Models;
    using FluentValidation;

    public class RelatorioRequestValidator : AbstractValidator<RelatorioRequest>
    {
        public RelatorioRequestValidator()
        {
            RuleFor(x => x.DataInicio)
                .NotEmpty().WithMessage("O campo 'Data de Início' é obrigatório.");

            RuleFor(x => x.DataFim)
                .NotEmpty().WithMessage("O campo 'Data de Fim' é obrigatório.");
        }
    }
}
