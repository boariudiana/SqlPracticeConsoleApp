using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccesLibrary.Models
{
    public class FullContactModel
    {
        public PeopleModel BasicInfo { get; set; }
        public List<AdressesModel> adresses { get; set; } = new List<AdressesModel>();
    }
}
