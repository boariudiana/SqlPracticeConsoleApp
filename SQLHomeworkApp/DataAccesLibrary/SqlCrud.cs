using DataAccesLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccesLibrary
{
    public class SqlCrud
    {
        private readonly string connectionString;

        SQLDataAccess db = new SQLDataAccess();

         public SqlCrud(string connectionString)
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
            string sql= "Select * from dbo.People p " +
                                  " where p.id = @Id ;";
            FullContactModel output = new FullContactModel();

            output.BasicInfo = db.LoadData<PeopleModel, dynamic>(sql, new { Id = id }, connectionString).FirstOrDefault();

            if (output.BasicInfo == null)
            {
                return null;
            }
            sql = "Select Adress from Adresses a inner join " +
                 "PeopleAdresses pa on a.Id = pa.AdressesID where pa.PeopleID = @Id; ";
            output.adresses = db.LoadData<AdressesModel, dynamic>(sql, new { Id = id }, connectionString);
            return output;

        }
        public void CreateFullContact(FullContactModel contact)
        {
            //insert into table People values

           string sql = "Insert into dbo.People (FirstName, LastName) values (@FirstName, @LastName);";
            db.SaveData(sql, new { FirstName = contact.BasicInfo.FirstName, LastName = contact.BasicInfo.LastName }, connectionString);

            //get the id of inserted contact

            sql = "Select Id from dbo.People where  FirstName = @FirstName and LastName = @LastName;";
            int contactId = db.LoadData<IdLookUpModel, dynamic>(sql,
                          new { FirstName = contact.BasicInfo.FirstName, LastName = contact.BasicInfo.LastName }, 
                          connectionString).First().Id;

            //verify all contact adresses in order to see if they already exist in the adresses table, if not add them to adresses table

            foreach (var adress in contact.adresses)
            {
                if (adress.Id == 0)
                {
                    //add adress to adresses table
                    sql = "Insert into dbo.Adresses (Adress) values (@Adress);";
                    db.SaveData(sql, new { Adress = adress.Adress }, connectionString);


                    // get the id of inserted adress
                    sql = "Select Id from dbo.Adresses where  Adress = @Adress;";
                    adress.Id = db.LoadData<IdLookUpModel, dynamic>(sql,
                                                                       new { Adress = adress.Adress },
                                                                       connectionString).First().Id;
                }
                //add link in the PeopleAdresses table
                sql = "Insert into dbo.PeopleAdresses (PeopleId, AdressesId) values (@PeopleId, @AdressesId);";
                db.SaveData(sql, new { PeopleId = contactId, AdressesId = adress.Id }, connectionString);
                
            }
        }

        public void UpdateContactName(PeopleModel contact)
        {
            string sql = "Update dbo.People set  FirstName = @FirstName , LastName = @LastName where id = @Id";
            db.SaveData(sql, contact, connectionString);
        }

        public void DeleteAdress( int personId, int adressId )
        {
            string sql = "Select Id, PeopleID, AdressesID from dbo.PeopleAdresses where AdressesID = @AdressesID;";
            var links = db.LoadData<PeopleAdressesModel, dynamic>(sql, new { AdressesID = adressId }, connectionString);

            if (links.Count == 1)
            {
                sql = "Delete from dbo.Adresses where Id =@AdressesID;";
                db.SaveData(sql, new { AdressesID = adressId }, connectionString);
            }

            sql = "Delete from dbo.PeopleAdresses  where AdressesID = @AdressesID and PeopleID = @PeopleID;";
            db.SaveData(sql, new { AdressesID = adressId, PeopleID = personId }, connectionString);
        }
    }
}
