using BusinessObjectsLayer.DTO;
using BusinessObjectsLayer.Entity;

namespace NguyenKhanhMinhRazorPages.Services
{
    public interface INewsArticleService
    {
        Task<List<NewsArticle>> GetNewsArticles();
        Task<List<NewsArticle>> GetNewsArticlesByCreatedBy(int createdById);
        Task<NewsArticle?> GetNewsArticleById(string articleId);
        Task<List<NewsArticle>> GetArticlesByTagId(int tagId);
        Task<List<NewsArticle>> GetActiveNewsArticles();
        Task AddNewsArticle(NewsArticleDto dto);
        Task UpdateNewsArticle(NewsArticleDto dto);
        Task RemoveNewsArticle(string articleId);
        Task RemoveTagsByArticleId(string articleId);
        Task<List<NewsArticle>> GetNewsArticlesByDateRange(DateRangeDto dateRangeDto);
    }
}
