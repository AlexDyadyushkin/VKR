using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoLider.Models
{
    partial class Cars
    {
        public string BrandModel
        {
            get
            {
                var CarBrandModel = $"{Brand} {Model}";
                return CarBrandModel;
            }
        }
    }
}
