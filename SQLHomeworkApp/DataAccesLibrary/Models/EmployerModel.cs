using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccesLibrary.Models
{
    public class EmployerModel
    {
        public int Id { get; set; }
        public PeopleModel Employer { get; set; }
    }
}
