using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Models
{
    public class ORDRViewModel
    {
        public string id { get; set; }
        public string cardCode { get; set; }
        public string cardName { get; set; }
        public DateTime docDate { get; set; }
        public decimal ordrValue { get; set; }
        public string comment { get; set; }
    }
}