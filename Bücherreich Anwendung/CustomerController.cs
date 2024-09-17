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

        public CustomerController()
        {
            storage = new JsonStorage<Customer>("customers.json");
            customerList = storage.LoadFromFile();
        }

        public void CreateCustomer(string Namen)
        {
            var customer = new Customer(customerList.Count + 1, Namen);
            customerList.Add(customer);
            SaveToFile();
        }

        public Customer ReadCustomer(int id)
        {
            return customerList.Find(c => c.Id == id);
        }

        public void UpdateCustomer(int id, string newName)
        {
            var customer = ReadCustomer(id);
            if (customer != null)
            {
                customer.Name = newName;
                SaveToFile();
            }
        }

        public void DeleteCustomer(int id)
        {
            var customer = ReadCustomer(id);
            if (customer != null)
            {
                customerList.Remove(customer);
                SaveToFile();
            }
        }

        public List<Customer> GetAllCustomers()
        {
            return customerList;
        }

        private void SaveToFile()
        {
            storage.SaveToFile(customerList);
        }
    }
}