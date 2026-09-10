using MediatR;

namespace Kovan.Application.Features.Users.Commands.UpdateProfilePicture;

public class UpdateProfilePictureCommand : IRequest<string> // Yeni resmin yolunu döndürür
{
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
    public string FileName { get; set; } = string.Empty;
}