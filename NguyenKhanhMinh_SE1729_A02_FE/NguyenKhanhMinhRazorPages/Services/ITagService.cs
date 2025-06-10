using BusinessObjectsLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenKhanhMinhRazorPages.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetTags();
        Task<Tag> GetTagById(int id);
        Task<List<Tag>> GetTagsByIds(List<int> tagIds);
        Task<List<Tag>> GetTagsByNewsArticleId(string newsArticleId);
        Task AddTag(Tag tag);
        Task UpdateTag(Tag tag);
        Task RemoveTag(int id);
        Task RemoveArticlesByTagId(int tagId);
    }
}
