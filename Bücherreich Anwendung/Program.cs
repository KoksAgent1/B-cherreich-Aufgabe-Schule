using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bücherreich_Anwendung
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Instance einer instance des MenuControllers erstellen
            MenuController menuController = new MenuController();
            menuController.MainMenu();
        }
    }
}
