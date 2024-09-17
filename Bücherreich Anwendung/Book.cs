using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bücherreich_Anwendung
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; set; }

        // Statt des gesamten Customer-Objekts speichern wir nur die CustomerId
        public int? CurrentBorrowerId { get; private set; }

        [NonSerialized]
        public Customer CurrentBorrower; // Diese Referenz wird nach dem Laden wiederhergestellt

        // Konstruktor für die Erstellung eines neuen Buches
        public Book(int id, string title)
        {
            Id = id;
            Title = title;
            CurrentBorrowerId = null;
            CurrentBorrower = null;
        }

        // Konstruktor für die Deserialisierung
        [JsonConstructor]
        public Book(int id, string Title, int? CurrentBorrowerId)
        {
            this.Id = id;
            this.Title = Title;
            this.CurrentBorrowerId = CurrentBorrowerId;
        }

        // Methode zum Hinzufügen eines Ausleihers
        public void AddBorrower(Customer customer)
        {
            if (customer != null) // Überprüfen, ob der Kunde nicht null ist
            {
                CurrentBorrower = customer;
                CurrentBorrowerId = customer.Id; // Speichern der ID
            }
        }

        // Methode zum Zurückgeben des Buches
        public void ReturnBook()
        {
            CurrentBorrower = null;
            CurrentBorrowerId = null;
        }

        // Methode, um zu prüfen, ob das Buch verfügbar ist
        public bool IsAvailable()
        {
            return CurrentBorrower == null;
        }

        // Methode zur Wiederherstellung der Referenzen nach der Deserialisierung
        public void ReconstructReferences(CustomerController customerController)
        {
            if (CurrentBorrowerId.HasValue)
            {
                CurrentBorrower = customerController.ReadCustomer(CurrentBorrowerId.Value);
            }
        }
    }
}
