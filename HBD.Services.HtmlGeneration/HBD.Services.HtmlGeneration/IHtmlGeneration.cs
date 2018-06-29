namespace HBD.Services.HtmlGeneration
{
    public interface IHtmlGeneration
    {
        string Generate();

        string ToClipboardFormat();
    }
}