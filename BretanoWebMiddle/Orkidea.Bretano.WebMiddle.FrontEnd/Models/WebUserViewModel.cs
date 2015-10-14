using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Models
{
    public class WebUserViewModel
    {
        public int webUserId { get; set; }
        public int webUserName { get; set; }
        public int companyId { get; set; }
        public bool admin { get; set; }
        public bool customerCreator { get; set; }
        public bool purchaseOrderCreator { get; set; }
        public int slpCode { get; set; }
    }
}