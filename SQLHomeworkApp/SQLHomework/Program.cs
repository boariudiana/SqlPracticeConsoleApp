using DataAccesLibrary;
using DataAccesLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlCrud sql = new SqlCrud(GetConnectionString());
             GetFullPersonInfo(sql,3);
            // AddFullContact(sql);
            // UpdateContact(sql);
           // DeleteAdress(sql, 2, 1);
            Console.WriteLine("Done processing SQL server");
            Console.ReadLine();

        }

        private static string GetConnectionString(string ConnectionStringName = "Default")
        {
            string output = "";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(ConnectionStringName);

            return output;
        }
        private static void GetFullPersonInfo(SqlCrud sql, int id)
        {
            var person = sql.FullPersonInfo(id);
            Console.WriteLine($"{person.BasicInfo.FirstName} {person.BasicInfo.LastName} ");
            foreach (var adress in person.adresses)
            {
                Console.WriteLine(adress.Adress);
            }

        }

        private static void AddFullContact(SqlCrud sql)
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

            sql.CreateFullContact(fullContact);
        }
        private static void UpdateContact(SqlCrud sql)
        {
            PeopleModel contact = new PeopleModel
            {
                Id = 1,
                FirstName = "Doina",
                LastName = "Stef"
            };
            sql.UpdateContactName(contact);

        }

        private static void DeleteAdress(SqlCrud sql, int personId, int adressId)
        {
            sql.DeleteAdress(personId, adressId);
        }
    }
}
