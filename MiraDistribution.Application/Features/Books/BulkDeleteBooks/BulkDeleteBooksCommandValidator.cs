using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Books.BulkDeleteBooks
{
    public class BulkDeleteBooksCommandValidator : AbstractValidator<BulkDeleteBooksCommand>
    {
        public BulkDeleteBooksCommandValidator()
        {
            RuleFor(x => x.BookIds)
                .NotEmpty().WithMessage("لازم تحدد دفتر واحد على الأقل.")
                .Must(ids => ids.Count <= 200).WithMessage("مينفعش تمسح أكتر من 200 دفتر في المرة الواحدة.");
        }
    }
}
