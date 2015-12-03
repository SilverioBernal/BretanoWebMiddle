using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Models
{
    public class SearchFormViewModel
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public DateTime cardCode { get; set; }
    }
}