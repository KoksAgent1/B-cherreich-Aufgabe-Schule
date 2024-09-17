using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bücherreich_Anwendung
{
    public class CustomerController
    {
        private List<Customer> customerList;
        private JsonStorage<Customer> storage;

        //Konstruktor
        public CustomerController()
        {
            storage = new JsonStorage<Customer>("customers.json");
            customerList = storage.LoadFromFile();
        }

        //Methode zum Erstellen eines neuen Kunden
        public void CreateCustomer(string Namen)
        {
            var customer = new Customer(customerList.Count + 1, Namen);
            customerList.Add(customer);
            SaveToFile();
        }

        //Methode zum Lesen eines Kunden
        public Customer ReadCustomer(int id)
        {
            return customerList.Find(c => c.Id == id);
        }

        //Methode zum Aktualisieren eines Kunden
        public void UpdateCustomer(int id, string newName)
        {
            var customer = ReadCustomer(id);
            if (customer != null)
            {
                customer.Name = newName;
                SaveToFile();
            }
        }

        //Methode zum Löschen eines Kunden
        public void DeleteCustomer(int id)
        {
            var customer = ReadCustomer(id);
            if (customer != null)
            {
                customerList.Remove(customer);
                SaveToFile();
            }
        }

        //Methode zum Abrufen aller Kunden
        public List<Customer> GetAllCustomers()
        {
            return customerList;
        }

        //Methode zum Speichern in eine Datei
        private void SaveToFile()
        {
            storage.SaveToFile(customerList);
        }
    }
}