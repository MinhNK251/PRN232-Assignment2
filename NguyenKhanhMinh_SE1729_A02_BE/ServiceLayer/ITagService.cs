using BusinessObjectsLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface ITagService
    {
        List<Tag> GetTags();
        Tag GetTagById(int id);
        List<Tag> GetTagsByIds(List<int> tagIds);
        List<Tag> GetTagsByNewsArticleId(string newsArticleId);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void RemoveTag(int id);
        void RemoveArticlesByTagId(int tagId);
    }
}
