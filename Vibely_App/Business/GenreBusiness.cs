using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vibely_App.API;
using Vibely_App.Data;
using Vibely_App.Data.Models;

namespace Vibely_App.Business
{
    public class GenreBusiness(VibelyDbContext dbContext) : IGenreBusiness
    {
        VibelyDbContext _dbContext = dbContext;

        public List<Genre> GetAll()
        {
            return _dbContext.Genres.ToList();
        }

        public void Add(Genre genre)
        {
            if (_dbContext.Genres.Find(genre.Id) != null)
            {
                return;
            }
            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();
        }

        public Genre? Find(int id)
        {
            return _dbContext.Genres
                .FirstOrDefault(g => g.Id == id);
        }

        public Genre? FindByName(string genre)
        {
            return _dbContext.Genres
                .Where(g => g.Name == genre).FirstOrDefault();
        }

        public void Remove(int id)
        {
            Genre? genre = _dbContext.Genres.Find(id);

            if (genre != null)
            {
                _dbContext.Genres.Remove(genre);
                _dbContext.SaveChanges();
            }
        }

        public void Update(int id, Genre genre)
        {
            Genre? genreToUpdate = _dbContext.Genres.Find(id);

            if (genreToUpdate != null)
            {
                _dbContext.Entry(genreToUpdate).State = EntityState.Detached;

                genreToUpdate.Name = genre.Name;

                _dbContext.Attach(genreToUpdate);
                _dbContext.Entry(genreToUpdate).State = EntityState.Modified;

                _dbContext.SaveChanges();
            }
        }
    }
}
