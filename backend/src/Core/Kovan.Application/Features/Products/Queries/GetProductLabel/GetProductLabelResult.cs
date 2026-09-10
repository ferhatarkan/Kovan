namespace Kovan.Application.Features.Products.Queries.GetProductLabel;

public class GetProductLabelResult
{
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/pdf";
    public string FileName { get; set; } = "label.pdf";
}