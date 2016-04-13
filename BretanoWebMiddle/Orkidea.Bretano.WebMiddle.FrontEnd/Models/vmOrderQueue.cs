using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Models
{
    public class vmOrderQueue
    {
        public List<ORDRViewModel> openOrders { get; set; }
        public List<ORDRViewModel> processedOrders { get; set; }
        public List<ORDRViewModel> processedCancelations { get; set; }

        public vmOrderQueue()
        {
            openOrders = new List<ORDRViewModel>();
            processedOrders = new List<ORDRViewModel>();
            processedCancelations = new List<ORDRViewModel>();
        }
    }
}