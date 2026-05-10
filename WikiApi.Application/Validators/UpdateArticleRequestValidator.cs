using FluentValidation;
using WikiApi.Application.Dtos;

namespace WikiApi.Application.Validators;

public class UpdateArticleRequestValidator : AbstractValidator<UpdateArticleRequest>
{
    public UpdateArticleRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O Id deve ser maior que 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(100).WithMessage("O título não deve exceder 100 caracteres.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("O conteúdo é obrigatório.")
            .MaximumLength(5000).WithMessage("O conteúdo não deve exceder 5000 caracteres.");

        RuleFor(x => x.Tags)
            .MaximumLength(200).WithMessage("As tags não devem exceder 200 caracteres.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("A categoria é obrigatória.")
            .MaximumLength(50).WithMessage("A categoria não deve exceder 50 caracteres.");
    }
}