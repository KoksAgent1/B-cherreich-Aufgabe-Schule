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

        // Konstruktor der BookController-Klasse
        public BookController()
        {
            storage = new JsonStorage<Book>("books.json"); // Initialisierung des JsonStorage mit dem Dateinamen
            bookList = storage.LoadFromFile(); // Laden der Bücher aus der Datei

            // Nach dem Laden aller Bücher müssen die Referenzen wiederhergestellt werden
            foreach (var book in bookList)
            {
                book.ReconstructReferences(new CustomerController()); // Verweis auf den CustomerController
            }
        }

        // Methode zum Erstellen eines neuen Buches
        public void CreateBook(string title)
        {
            var book = new Book(bookList.Count + 1, title); // Erstellen eines neuen Buches mit einer ID
            bookList.Add(book); // Hinzufügen des Buches zur Liste
            SaveToFile(); // Speichern der Liste in die Datei
        }

        // Methode zum Suchen eines Buches anhand der ID
        public Book ReadBook(int id)
        {
            return bookList.Find(b => b.Id == id);
        }

        // Methode zum Aktualisieren eines Buches
        public void UpdateBook(int id, string newTitle)
        {
            var book = ReadBook(id); // Buch anhand der ID suchen
            // Wenn das Buch gefunden wurde, aktualisiere den Titel
            if (book != null)
            {
                book.Title = newTitle;
                SaveToFile();
            }
        }

        // Methode zum Löschen eines Buches
        public void DeleteBook(int id)
        {
            var book = ReadBook(id); // Buch anhand der ID suchen
            // Wenn das Buch gefunden wurde, lösche es
            if (book != null)
            {
                bookList.Remove(book);
                SaveToFile();
            }
        }


        // Methode zum Abrufen aller Bücher
        public List<Book> GetAllBooks()
        {
            return bookList;
        }


        // Methode zum Speichern der Bücher in die Datei
        public void SaveToFile()
        {
            storage.SaveToFile(bookList);
        }
    }
}