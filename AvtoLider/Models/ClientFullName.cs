using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoLider.Models
{
    partial class Clients
    {
        public string FullName
        {
            get
            {
                var ClientFullName = $"{FirstName} {LastName}";
                return ClientFullName;
            }
        }
    }
}
