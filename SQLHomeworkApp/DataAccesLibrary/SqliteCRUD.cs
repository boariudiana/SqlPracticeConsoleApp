using DataAccesLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccesLibrary
{
    public class SqliteCRUD
    {
        private readonly string connectionString;

        SQLiteDataAcces db = new SQLiteDataAcces();
        public SqliteCRUD(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<PeopleModel> GetAllPeople()
        {
            string sqlStatement = "select id, FirstName, LastName from dbo.People";
            return db.LoadData<PeopleModel, dynamic>(sqlStatement, new { }, connectionString);
        }

        public FullContactModel FullPersonInfo(int id)
        {
            string sqlite = "Select * from People p " +
                                  " where p.id = @Id ;";
            FullContactModel output = new FullContactModel();

            output.BasicInfo = db.LoadData<PeopleModel, dynamic>(sqlite, new { Id = id }, connectionString).FirstOrDefault();

            if (output.BasicInfo == null)
            {
                return null;
            }
            sqlite = "Select Adress from Adresses a inner join " +
                 "PeopleAdresses pa on a.Id = pa.AdressesID where pa.PeopleID = @Id; ";
            output.adresses = db.LoadData<AdressesModel, dynamic>(sqlite, new { Id = id }, connectionString);
            return output;

        }
        public void CreateFullContact(FullContactModel contact)
        {
            //insert into table People values

            string sqlite = "Insert into People (FirstName, LastName) values (@FirstName, @LastName);";
            db.SaveData(sqlite, new { FirstName = contact.BasicInfo.FirstName, LastName = contact.BasicInfo.LastName }, connectionString);

            //get the id of inserted contact

            sqlite = "Select Id from People where  FirstName = @FirstName and LastName = @LastName;";
            int contactId = db.LoadData<IdLookUpModel, dynamic>(sqlite,
                          new { FirstName = contact.BasicInfo.FirstName, LastName = contact.BasicInfo.LastName },
                          connectionString).First().Id;

            //verify all contact adresses in order to see if they already exist in the adresses table, if not add them to adresses table

            foreach (var adress in contact.adresses)
            {
                if (adress.Id == 0)
                {
                    //add adress to adresses table
                    sqlite = "Insert into Adresses (Adress) values (@Adress);";
                    db.SaveData(sqlite, new { Adress = adress.Adress }, connectionString);


                    // get the id of inserted adress
                    sqlite = "Select Id from Adresses where  Adress = @Adress;";
                    adress.Id = db.LoadData<IdLookUpModel, dynamic>(sqlite,
                                                                       new { Adress = adress.Adress },
                                                                       connectionString).First().Id;
                }
                //add link in the PeopleAdresses table
                sqlite = "Insert into PeopleAdresses (PeopleId, AdressesId) values (@PeopleId, @AdressesId);";
                db.SaveData(sqlite, new { PeopleId = contactId, AdressesId = adress.Id }, connectionString);

            }
        }

        public void UpdateContactName(PeopleModel contact)
        {
            string sqlite = "Update People set  FirstName = @FirstName , LastName = @LastName where id = @Id";
            db.SaveData(sqlite, contact, connectionString);
        }

        public void DeleteAdress(int personId, int adressId)
        {
            string sqlite = "Select Id, PeopleID, AdressesID from dbo.PeopleAdresses where AdressesID = @AdressesID;";
            var links = db.LoadData<PeopleAdressesModel, dynamic>(sqlite, new { AdressesID = adressId }, connectionString);

            if (links.Count == 1)
            {
                sqlite = "Delete from Adresses where Id =@AdressesID;";
                db.SaveData(sqlite, new { AdressesID = adressId }, connectionString);
            }

            sqlite = "Delete from PeopleAdresses  where AdressesID = @AdressesID and PeopleID = @PeopleID;";
            db.SaveData(sqlite, new { AdressesID = adressId, PeopleID = personId }, connectionString);
        }

    }
}
