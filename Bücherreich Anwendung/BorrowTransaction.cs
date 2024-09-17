using System;
using System.Collections.Generic;
using System.Linq;

namespace Bücherreich_Anwendung
{
    public class BorrowTransaction
    {
        private CustomerController customerController;
        private BookController bookController;
        private List<BorrowRecord> borrowHistory; // Liste der Ausleihvorgänge
        private JsonStorage<BorrowRecord> borrowHistoryStorage; // JsonStorage für die BorrowRecords

        public BorrowTransaction(CustomerController customerCtrl, BookController bookCtrl)
        {
            customerController = customerCtrl;
            bookController = bookCtrl;
            borrowHistoryStorage = new JsonStorage<BorrowRecord>("borrowHistory.json");
            borrowHistory = borrowHistoryStorage.LoadFromFile();

            // Nach dem Laden alle Referenzen rekonstruieren
            foreach (var record in borrowHistory)
            {
                record.ReconstructReferences(customerController, bookController);
            }
        }

        // Buch ausleihen
        public void BorrowBook(int customerId, int bookId)
        {
            var customer = customerController.ReadCustomer(customerId);
            var book = bookController.ReadBook(bookId);

            if (customer != null && book != null)
            {
                if (book.IsAvailable())
                {
                    book.AddBorrower(customer); // Setze den aktuellen Ausleiher
                    AddToHistory(customer, book); // Füge die Ausleihe zur Historie hinzu
                    Console.WriteLine($"{customer.Name} hat das Buch '{book.Title}' ausgeliehen.");
                    SaveChanges(); // Speichern der Änderungen in die Datei
                }
                else
                {
                    Console.WriteLine($"Das Buch '{book.Title}' ist derzeit von {book.CurrentBorrower.Name} ausgeliehen.");
                }
            }
            else
            {
                Console.WriteLine("Kunde oder Buch nicht gefunden.");
            }
        }

        // Buch zurückgeben
        public void ReturnBook(int customerId, int bookId)
        {
            var customer = customerController.ReadCustomer(customerId);
            var book = bookController.ReadBook(bookId);

            if (customer != null && book != null)
            {
                Console.WriteLine(book.CurrentBorrower.Id == customer.Id);
                if (book.CurrentBorrower.Id == customer.Id)
                {
                    book.ReturnBook(); // Buch wird zurückgegeben
                    Console.WriteLine($"{customer.Name} hat das Buch '{book.Title}' zurückgegeben.");
                    SaveChanges(); // Speichern der Änderungen
                }
                else
                {
                    Console.WriteLine($"{customer.Name} hat dieses Buch nicht ausgeliehen.");
                }
            }
            else
            {
                Console.WriteLine("Kunde oder Buch nicht gefunden.");
            }
        }

        // Methode, um die Historie zu aktualisieren
        private void AddToHistory(Customer customer, Book book)
        {
            var borrowRecord = new BorrowRecord(customer, book, DateTime.Now);
            borrowHistory.Add(borrowRecord);
            SaveChanges();
        }

        // Abrufen der Historie eines Buches
        public List<BorrowRecord> GetBorrowHistory(int bookId)
        {
            return borrowHistory.Where(record => record.BookId == bookId).ToList();
        }


        // Abrufen aller aktuell ausgeliehenen Bücher eines Kunden
        public List<Book> GetBooksBorrowedByCustomer(int customerId)
        {
            return borrowHistory
                .Where(record => record.CustomerId == customerId && record.Book.CurrentBorrower?.Id == customerId)
                .Select(record => record.Book)
                .ToList();
        }

        // Speichern aller Änderungen
        private void SaveChanges()
        {
            bookController.SaveToFile(); // Speichern der Buchdaten
            borrowHistoryStorage.SaveToFile(borrowHistory); // Speichern der Ausleihhistorie
        }
    }
}