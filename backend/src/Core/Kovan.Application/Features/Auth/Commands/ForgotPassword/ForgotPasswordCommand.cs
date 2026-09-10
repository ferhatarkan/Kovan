using MediatR;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommand : IRequest<string>, ITransactionalRequest // Token'ı test için döndürelim
{
    public string Email { get; set; } = string.Empty;
}
