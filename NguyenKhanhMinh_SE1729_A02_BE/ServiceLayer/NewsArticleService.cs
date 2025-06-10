using BusinessObjectsLayer.Entity;
using RepositoriesLayer;

namespace ServiceLayer
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepo _repo;

        public NewsArticleService(INewsArticleRepo repo)
        {
            _repo = repo;
        }

        public void AddNewsArticle(NewsArticle newsArticle)
        {
            if (newsArticle == null)
                throw new ArgumentNullException(nameof(newsArticle));

            _repo.AddNewsArticle(newsArticle);
        }

        public List<NewsArticle> GetActiveNewsArticles()
        {
            return _repo.GetActiveNewsArticles();
        }

        public List<NewsArticle> GetArticlesByTagId(int tagId)
        {
            return _repo.GetArticlesByTagId(tagId);
        }

        public NewsArticle? GetNewsArticleById(string articleId)
        {
            return _repo.GetNewsArticleById(articleId);
        }

        public List<NewsArticle> GetNewsArticles()
        {
            return _repo.GetNewsArticles();
        }

        public List<NewsArticle> GetNewsArticlesByCreatedBy(int createdById)
        {
            return _repo.GetNewsArticlesByCreatedBy(createdById);
        }

        public List<NewsArticle> GetNewsArticlesByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be later than end date.");

            return _repo.GetNewsArticlesByDateRange(startDate, endDate);
        }

        public void RemoveNewsArticle(string articleId)
        {
            if (string.IsNullOrWhiteSpace(articleId))
                throw new ArgumentException("Article ID cannot be null or empty.", nameof(articleId));

            _repo.RemoveNewsArticle(articleId);
        }

        public void RemoveTagsByArticleId(string articleId)
        {
            if (string.IsNullOrWhiteSpace(articleId))
                throw new ArgumentException("Article ID cannot be null or empty.", nameof(articleId));

            _repo.RemoveTagsByArticleId(articleId);
        }

        public void UpdateNewsArticle(string articleId, NewsArticle updatedArticle)
        {
            if (string.IsNullOrWhiteSpace(articleId))
                throw new ArgumentException("Article ID cannot be null or empty.", nameof(articleId));

            if (updatedArticle == null)
                throw new ArgumentNullException(nameof(updatedArticle));

            _repo.UpdateNewsArticle(articleId, updatedArticle);
        }
    }
}