using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Books.SetBookNote
{
    public class SetBookNoteCommandValidator : AbstractValidator<SetBookNoteCommand>
    {
        public SetBookNoteCommandValidator()
        {
            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("الملاحظة أطول من اللازم (500 حرف كحد أقصى).");
        }
    }
}
