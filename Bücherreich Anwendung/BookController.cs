using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Bücherreich_Anwendung
{
    public class BookController
    {
        private List<Book> bookList;
        private JsonStorage<Book> storage;

        public BookController()
        {
            storage = new JsonStorage<Book>("books.json");
            bookList = storage.LoadFromFile();

            // Nach dem Laden aller Bücher müssen die Referenzen wiederhergestellt werden
            foreach (var book in bookList)
            {
                book.ReconstructReferences(new CustomerController()); // Verweis auf den CustomerController
            }
        }

        public void CreateBook(string title)
        {
            var book = new Book(bookList.Count + 1, title);
            bookList.Add(book);
            SaveToFile();
        }

        public Book ReadBook(int id)
        {
            return bookList.Find(b => b.Id == id);
        }

        public void UpdateBook(int id, string newTitle)
        {
            var book = ReadBook(id);
            if (book != null)
            {
                book.Title = newTitle;
                SaveToFile();
            }
        }

        public void DeleteBook(int id)
        {
            var book = ReadBook(id);
            if (book != null)
            {
                bookList.Remove(book);
                SaveToFile();
            }
        }


        public List<Book> GetAllBooks()
        {
            return bookList;
        }

        public void SaveToFile()
        {
            storage.SaveToFile(bookList);
        }
    }
}