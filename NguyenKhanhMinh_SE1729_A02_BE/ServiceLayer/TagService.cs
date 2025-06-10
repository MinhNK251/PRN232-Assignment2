using BusinessObjectsLayer.Entity;
using RepositoriesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class TagService : ITagService
    {
        private readonly ITagRepo _repo;

        public TagService(ITagRepo repo)
        {
            _repo = repo;
        }

        public void AddTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            _repo.AddTag(tag);
        }

        public Tag GetTagById(int id)
        {
            return _repo.GetTagById(id);
        }

        public List<Tag> GetTags()
        {
            return _repo.GetTags();
        }

        public List<Tag> GetTagsByIds(List<int> tagIds)
        {
            return _repo.GetTagsByIds(tagIds);
        }

        public List<Tag> GetTagsByNewsArticleId(string newsArticleId)
        {
            return _repo.GetTagsByNewsArticleId(newsArticleId);
        }

        public void RemoveArticlesByTagId(int tagId)
        {
            _repo.RemoveArticlesByTagId(tagId);
        }

        public void RemoveTag(int id)
        {
            _repo.RemoveTag(id);
        }

        public void UpdateTag(Tag tag)
        {
            _repo.UpdateTag(tag);
        }
    }
}
