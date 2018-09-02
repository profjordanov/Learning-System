namespace LearningSystem.Services
{
    public interface IPdfGenerator : IService
    {
        byte[] GeneratePdfFromHtml(string html);
    }
}