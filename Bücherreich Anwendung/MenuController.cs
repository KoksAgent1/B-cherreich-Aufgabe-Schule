﻿using System;
using System.Net;
using System.Runtime.Remoting.Messaging;

namespace Bücherreich_Anwendung
{
    public class MenuController
    {
        static CustomerController customerController = new CustomerController();
        static BookController bookController = new BookController();
        static BorrowTransaction borrowTransaction = new BorrowTransaction(customerController, bookController);
        public void MainMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Header();
                Console.WriteLine("\nBibliotheksystem:");
                Console.WriteLine("1. Kunde erstellen");
                Console.WriteLine("2. Buch erstellen");
                Console.WriteLine("3. Buch ausleihen");
                Console.WriteLine("4. Buch zurückgeben");
                Console.WriteLine("5. Zeige ausgeliehene Bücher eines Kunden");
                Console.WriteLine("6. Zeige Ausleiher eines Buches");
                Console.WriteLine("7. Kunde bearbeiten/löschen");
                Console.WriteLine("8. Buch bearbeiten/löschen");
                Console.WriteLine("9. Alle Kunden anzeigen");
                Console.WriteLine("10. Alle Bücher anzeigen");
                Console.WriteLine("11. Beenden");
                Console.Write("Bitte wähle eine Option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CreateCustomer();
                        break;
                    case "2":
                        CreateBook();
                        break;
                    case "3":
                        BorrowBook();
                        break;
                    case "4":
                        ReturnBook();
                        break;
                    case "5":
                        ShowBorrowedBooks();
                        break;
                    case "6":
                        ShowBookBorrowers();
                        break;
                    case "7":
                        ManageCustomer();
                        break;
                    case "8":
                        ManageBook();
                        break;
                    case "9":
                        ListAllCustomers();
                        break;
                    case "10":
                        ListAllBooks();
                        break;
                    case "11":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe, versuche es erneut.");
                        break;
                }
            }
        }

        private void Header()
        {
            Console.Clear();
            Console.WriteLine("System von Florian Wielga. Version 1.0");
            Console.WriteLine("-------------------------------------------------------------");
        }
        private void CreateCustomer()
        {
            Console.Clear();
            Console.Write("Kunden Name: ");
            string name = Console.ReadLine();

            customerController.CreateCustomer(name);
            Console.WriteLine("Kunde erstellt.");
            Console.ReadKey();
        }

        private void CreateBook()
        {
            Console.Clear();
            Console.Write("Buchtitel: ");
            string title = Console.ReadLine();
            bookController.CreateBook(title);
            Console.WriteLine("Buch erstellt.");
            Console.ReadKey();
        }

        private void BorrowBook()
        {
            Console.Clear();
            Console.Write("Kunden ID: ");
            int customerId = int.Parse(Console.ReadLine());
            Console.Write("Buch ID: ");
            int bookId = int.Parse(Console.ReadLine());

            borrowTransaction.BorrowBook(customerId, bookId);
            Console.ReadKey();
        }

        private void ReturnBook()
        {
            Console.Clear();
            Console.Write("Kunden ID: ");
            int customerId = int.Parse(Console.ReadLine());
            Console.Write("Buch ID: ");
            int bookId = int.Parse(Console.ReadLine());

            borrowTransaction.ReturnBook(customerId, bookId);
            Console.ReadKey();
        }

        private void ShowBorrowedBooks()
        {
            Console.Clear();
            Console.Write("Kunden ID: ");
            int customerId = int.Parse(Console.ReadLine());

            var books = borrowTransaction.GetBooksBorrowedByCustomer(customerId);
            Console.WriteLine(books.Count);
            if (books.Count > 0)
            {
                Console.WriteLine("\nAusgeliehene Bücher:");
                foreach (var book in books)
                {
                    Console.WriteLine($"- {book.Title}");
                }
            }
            else
            {
                Console.WriteLine("Keine ausgeliehenen Bücher.");
            }
            Console.ReadKey();
        }
        private void ShowBookBorrowers()
        {
            Console.Clear();
            Console.Write("Buch ID: ");
            int bookId = int.Parse(Console.ReadLine());

            var borrowHistory = borrowTransaction.GetBorrowHistory(bookId);
            if (borrowHistory.Count > 0)
            {
                Console.WriteLine("\nBisherige Ausleiher:");
                foreach (var record in borrowHistory)
                {
                    Console.WriteLine($"- {record.Customer.Name} am {record.BorrowDate}");
                }
            }
            else
            {
                Console.WriteLine("Es gibt keine Ausleiher für dieses Buch.");
            }
            Console.ReadKey();
        }

        private void ManageCustomer()
        {
            Console.Clear();
            Console.Write("Kunden ID: ");
            int customerId = int.Parse(Console.ReadLine());

            Customer customer = customerController.ReadCustomer(customerId);
            if (customer == null)
            {
                Console.WriteLine("Kunde nicht gefunden.");
                return;
            }

            Console.WriteLine("1. Kunde bearbeiten");
            Console.WriteLine("2. Kunde löschen");
            Console.Write("Bitte wähle eine Option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Neuer Name: ");
                    string newName = Console.ReadLine();
                    customerController.UpdateCustomer(customerId, newName);
                    Console.WriteLine("Kunde aktualisiert.");
                    break;
                case "2":
                    customerController.DeleteCustomer(customerId);
                    Console.WriteLine("Kunde gelöscht.");
                    break;
                default:
                    Console.WriteLine("Ungültige Eingabe.");
                    break;
            }
        }

        private void ManageBook()
        {
            Console.Clear();
            Console.Write("Buch ID: ");
            int bookId = int.Parse(Console.ReadLine());

            Book book = bookController.ReadBook(bookId);
            if (book == null)
            {
                Console.WriteLine("Buch nicht gefunden.");
                return;
            }

            Console.WriteLine("1. Buch bearbeiten");
            Console.WriteLine("2. Buch löschen");
            Console.Write("Bitte wähle eine Option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Neuer Titel: ");
                    string newTitle = Console.ReadLine();
                    bookController.UpdateBook(bookId, newTitle);
                    Console.WriteLine("Buch aktualisiert.");
                    break;
                case "2":
                    bookController.DeleteBook(bookId);
                    Console.WriteLine("Buch gelöscht.");
                    break;
                default:
                    Console.WriteLine("Ungültige Eingabe.");
                    break;
            }
        }

        private void ListAllCustomers()
        {
            var customers = customerController.GetAllCustomers();
            Console.Clear();
            if (customers != null && customers.Count > 0)
            {
                Console.WriteLine("\nListe aller Kunden:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.Id}, Name: {customer.Name}");
                }
            }
            else
            {
                Console.WriteLine("Keine Kunden vorhanden.");
            }
            Console.ReadKey();
        }

        private void ListAllBooks()
        {
            var books = bookController.GetAllBooks();
            Console.Clear();
            if (books != null && books.Count > 0)
            {
                Console.WriteLine("\nListe aller Bücher:");
                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.Id}, Titel: {book.Title}, Verfügbar: {book.IsAvailable()}");
                }
            }
            else
            {
                Console.WriteLine("Keine Bücher vorhanden.");
            }
            Console.ReadKey();
        }
    }
}