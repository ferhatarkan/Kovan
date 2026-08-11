namespace Kovan.Application.Features.Products.Queries.GetProductLabel;

public class ProductLabelDto
{
    public byte[] FileContents { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/pdf";
    public string FileName { get; set; } = "label.pdf";
}