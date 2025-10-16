using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Utility
{
    internal class CarParkUtility
    {
    }



    public static class CarParkHelper
    {
        public static Dictionary<string, int> GetParkingLotUsage(this CarPark carPark)
        {
            int freeRegularParkinLots = 0;
            int freeTenantParkingLots = 0;
            int OccupiedRegularParkingLots = 0;
            int OccupiedTenantParkingLots = 0;
            int parkingLots = 0;

            foreach(var floor in carPark.Floors)
            {
                freeRegularParkinLots += floor.ParkingLots.Count(x => x.IsOccupied == false && x.IsPermanentTenant == false);

                freeTenantParkingLots +=  floor.ParkingLots.Count(x => x.IsOccupied == false && x.IsPermanentTenant == true);

                OccupiedRegularParkingLots +=  floor.ParkingLots.Count(x => x.IsOccupied == true && x.IsPermanentTenant == false);

                OccupiedTenantParkingLots += floor.ParkingLots.Count(x => x.IsOccupied == true && x.IsPermanentTenant == true);

                parkingLots += floor.ParkingLots.Count();

            }

            Dictionary<string, int> result = new Dictionary<string, int>();

            result.Add("FREE_REGULAR", freeRegularParkinLots);
            result.Add("FREE_TENANT", freeTenantParkingLots);
            result.Add("OCCUPIED_REGULAR", OccupiedRegularParkingLots);
            result.Add("OCCUPIED_TENANT", OccupiedTenantParkingLots);
            result.Add("TOTAL", parkingLots);

            return result;
        }

        

    }

}
