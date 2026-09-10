using System.Net;
using System.Text.Json;
using Kovan.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmedik bir hata oluştu: {Message}", ex.Message);

            var response = context.Response;
            response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails();

            switch (ex)
            {
                case ValidationException validationException:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Bir veya daha fazla doğrulama hatası oluştu.";
                    problemDetails.Extensions.Add("errors", validationException.Errors);
                    break;
                case NotFoundException:
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    problemDetails.Title = "İstenen kaynak bulunamadı.";
                    problemDetails.Detail = ex.Message;
                    break;
                default:
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Title = "Sunucuda beklenmedik bir hata oluştu.";
                    problemDetails.Detail = "İsteğiniz işlenirken bir sorunla karşılaşıldı. Lütfen daha sonra tekrar deneyin.";
                    break;
            }

            var result = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsync(result);
        }
    }
}
