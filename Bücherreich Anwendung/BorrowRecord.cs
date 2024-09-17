using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bücherreich_Anwendung
{
    public class BorrowRecord
    {
        public int CustomerId { get; private set; }
        public int BookId { get; private set; }
        public DateTime BorrowDate { get; private set; }

        // Diese Felder werden nicht serialisiert, sondern nur rekonstruiert
        [NonSerialized]
        public Customer Customer;
        [NonSerialized]
        public Book Book;

        public BorrowRecord(Customer customer, Book book, DateTime borrowDate)
        {
            CustomerId = customer.Id;
            BookId = book.Id;
            Customer = customer;
            Book = book;
            BorrowDate = borrowDate;
        }

        [JsonConstructor]
        public BorrowRecord(int CustomerId,int BookId, DateTime BorrowDate)
        {
            this.CustomerId = CustomerId;
            this.BookId = BookId;
            this.BorrowDate = BorrowDate;
        }

        // Diese Methode wird verwendet, um die Referenzen zu rekonstruieren
        public void ReconstructReferences(CustomerController customerController, BookController bookController)
        {
            Customer = customerController.ReadCustomer(CustomerId);
            Book = bookController.ReadBook(BookId);

            Console.WriteLine($"Kunde: {Customer.Id} {Customer.Name}");
            Console.WriteLine($"Book: {Book.Id} {Book.Title}");

            if (Customer == null)
            {
                Console.WriteLine($"Warnung: Kunde mit ID {CustomerId} wurde nicht gefunden.");
            }

            if (Book == null)
            {
                Console.WriteLine($"Warnung: Buch mit ID {BookId} wurde nicht gefunden.");
            }
        }
    }
}