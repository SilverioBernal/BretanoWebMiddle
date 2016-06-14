using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Entities
{
    public class AuthReport
    {
        public int id { get; set; }
        public string cardCode { get; set; }
        public string cardName { get; set; }
        public string docNum { get; set; }
        public int? docEntry { get; set; }
        public DateTime docDate { get; set; }
        public decimal subtotalDoc { get; set; }
        public decimal ivaDoc { get; set; }
        public decimal totalDoc { get; set; }
        public string status { get; set; }
        public string authUser { get; set; }
        public DateTime? authDate { get; set; }
        public string draftComments { get; set; }
        public string authComments { get; set; }
    }
}
