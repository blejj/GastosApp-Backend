using FluentValidation;
using GastosApp.Application.DTOs;

namespace GastosApp.Application.Validators;

public class GastoValidator : AbstractValidator<GastoRequest>
{
    public GastoValidator()
    {
        RuleFor(x => x.Monto)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");

        RuleFor(x => x.Fecha)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("La fecha no puede ser futura.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("Debe indicar una categoría.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(200).WithMessage("La descripción no puede superar los 200 caracteres.");
    }
}
