using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoLider.Models
{
    public partial class Configuration
    {
        public string FullPrice
        {
            get
            {
                var ConfFullPrice = Convert.ToDouble(Cars.Price) + Convert.ToDouble(TypesOfCover.Price) + Convert.ToDouble(TypesOfRims.Price) + Convert.ToDouble(TypesOfTires.Price)
                    + Convert.ToDouble(TypesOfSeatUpholstery.Price) + Convert.ToDouble(TypesOfSteeringWheelsUpholstery.Price);
                return ConfFullPrice.ToString() + " ₽";
            }
        }
    }
}
