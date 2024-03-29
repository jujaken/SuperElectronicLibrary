﻿using SuperElectronicLibrary.Models;

namespace SuperElectronicLibrary.Repositories
{
    public class BookRepository
    {
        public List<Book>? GetAll()
        {
            using var db = new AppDbContext();

            return db.Books?.ToList();
        }

        public Book? GetById(int id)
        {
            using var db = new AppDbContext();

            return db.Books?.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"> Вызывается, если не был найден жанр </exception>
        public List<Book>? GetByGenreId(int genreId)
        {
            using var db = new AppDbContext();

            var genre = db.Genres?.FirstOrDefault(g => g.Id == genreId);

            if (genre is null)
                throw new ArgumentException();

            return db.Books?.Where(b => b.Genres.Contains(genre)).ToList();
        }

        public List<Book>? GetByDates(DateTime firstDate, DateTime lastDate)
        {
            if (firstDate > lastDate)
                throw new ArgumentException();

            using var db = new AppDbContext();

            return db.Books?.Where(b => firstDate < b.YearOfPublication && b.YearOfPublication < lastDate).ToList();
        }

        public void Add(params Book[] Book)
        {
            using var db = new AppDbContext();

            db.Books?.AddRange(Book);
            db.SaveChanges();
        }

        public void Remove(params Book[] Book)
        {
            using var db = new AppDbContext();

            db.Books?.RemoveRange(Book);
            db.SaveChanges();
        }

        public void UpdateYearOfPublicationById(int id, DateTime newDate)
        {
            using var db = new AppDbContext();

            var book = db.Books?.FirstOrDefault(b => b.Id == id);

            if (book is null)
                throw new Exception();

            book.YearOfPublication = newDate;

            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameBook"></param>
        /// <param name="auhtorId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"> Вызывается при отсутствующем авторе </exception>
        public bool ContainsByNameAndAuthor(string nameBook, int auhtorId)
        {
            using var db = new AppDbContext();

            var author = new AuthorRepository().GetById(auhtorId);

            if (author is null)
                throw new ArgumentException();

            return db.Books?.Select(b => b.Tittle == nameBook && b.Authors.Contains(author)).Count() > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"> вызывается при отсутствующем пользователе </exception>
        public bool ContainsByUser(int userId)
        {
            using var db = new AppDbContext();

            var user = new UserRepository().GetById(userId);

            if (user is null)
                throw new ArgumentException();

            return db.Books?.Select(b => b.CurrentUser == user).Count() > 0;
        }

        public List<Book>? GetAllWithNameFilter()
        {
            using var db = new AppDbContext();

            var books = GetAll();

            return books?.OrderBy(b => b.Tittle).ToList();
        }

        public List<Book>? GetAllWithYearFilter()
        {
            using var db = new AppDbContext();

            var books = GetAll();

            return books?.OrderByDescending(b => b.YearOfPublication).ToList();
        }
    }
}