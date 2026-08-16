using FluentValidation;

namespace Kovan.Application.Features.Reports.Queries.GetSalesSummaryReport;

public class GetSalesSummaryReportQueryValidator : AbstractValidator<GetSalesSummaryReportQuery>
{
    public GetSalesSummaryReportQueryValidator()
    {
        RuleFor(v => v.StartDate).NotEmpty().WithMessage("Başlangıç tarihi boş olamaz.");
        RuleFor(v => v.EndDate).NotEmpty().WithMessage("Bitiş tarihi boş olamaz.");
        RuleFor(v => v.EndDate).GreaterThanOrEqualTo(v => v.StartDate).WithMessage("Bitiş tarihi, başlangıç tarihinden önce olamaz.");
        RuleFor(v => v.GroupBy).IsInEnum().WithMessage("Geçersiz gruplama değeri."); // Yeni eklenen kural
    }
}