using DataAccesLibrary;
using DataAccesLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SQLITEHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            SqliteCRUD sqliteCrud = new SqliteCRUD(GetConnectionString());
             GetFullPersonInfo(sqliteCrud,3);
            // AddFullContact(sqliteCrud);
            // UpdateContact(sqliteCrud);
            // DeleteAdress(sqliteCrud, 2, 1);
            Console.WriteLine("Done processing Sqlite");
            Console.ReadLine();
        }

        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = "";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;

        }

        private static void GetFullPersonInfo(SqliteCRUD sqliteCrud, int id)
        {
            var person = sqliteCrud.FullPersonInfo(id);
            Console.WriteLine($"{person.BasicInfo.FirstName} {person.BasicInfo.LastName} ");
            foreach (var adress in person.adresses)
            {
                Console.WriteLine(adress.Adress);
            }

        }

        private static void AddFullContact(SqliteCRUD sqliteCrud)
        {
            FullContactModel fullContact = new FullContactModel
            {
                BasicInfo = new PeopleModel
                {
                    FirstName = "Alondra",
                    LastName = "Gates"
                }
            };
            fullContact.adresses.Add(new AdressesModel { Id = 1, Adress = "Ion Creanga nr 17" });
            fullContact.adresses.Add(new AdressesModel { Adress = "Ion Caragiale nr 88" });

            sqliteCrud.CreateFullContact(fullContact);
        }
        private static void UpdateContact(SqliteCRUD sqliteCrud)
        {
            PeopleModel contact = new PeopleModel
            {
                Id = 1,
                FirstName = "Doina",
                LastName = "Stef"
            };
            sqliteCrud.UpdateContactName(contact);

        }

        private static void DeleteAdress(SqliteCRUD sqliteCrud, int personId, int adressId)
        {
            sqliteCrud.DeleteAdress(personId, adressId);
        }
    }
}
