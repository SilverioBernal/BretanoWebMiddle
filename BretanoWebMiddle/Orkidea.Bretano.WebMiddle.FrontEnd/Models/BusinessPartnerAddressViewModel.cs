using Orkidea.Bretano.WebMiddle.FrontEnd.WebMiddleBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Models
{
    public class BusinessPartnerAddressViewModel : BusinessPartnerAddress
    {
        public Dictionary<string, string> addressTypes { get; set; }        

        public string addType { get; set; }
        
        public string uCssIva { get; set; }
                        
        public BusinessPartnerAddressViewModel()
        {            
            addressTypes = new Dictionary<string, string>();
            addressTypes.Add("B", "Entrega de factura");
            addressTypes.Add("S", "Entrega de producto");            
        }
    }
}