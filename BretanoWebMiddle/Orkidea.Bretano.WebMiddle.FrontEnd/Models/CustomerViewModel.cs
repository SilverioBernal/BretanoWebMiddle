using Orkidea.Bretano.WebMiddle.FrontEnd.WebMiddleBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Models
{
    public class CustomerViewModel:BusinessPartner
    {        
        public Dictionary<string, string> bpTypes { get; set; }
        
        public string bpType { get; set; }
        public string uBpcoRt { get; set; }
        public string uBpcoTdc { get; set; }
        public string uBpcoCs { get; set; }
        public string uBpcoCity { get; set; }
        public string uBpcoTp { get; set; }
        public string uCssIva { get; set; }
        public string uCssAcceptInvoice { get; set; }
        public string uQcaSegment { get; set; }

        public CustomerViewModel()
        {
            bpTypes = new Dictionary<string, string>();

            bpTypes.Add("C", "Customer");
            bpTypes.Add("L", "Lead");
        }
    }
}