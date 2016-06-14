using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Models
{
    [Serializable]
    public class vmAuthReport
    {
        [XmlAttribute("Cliente")]        
        public string cardName { get; set; }
        [XmlAttribute("Pedido")]
        public string docNum { get; set; }
        [XmlAttribute("Fecha_pedido")]
        public string docDate { get; set; }
        [XmlAttribute("Total_documento")]
        public decimal totalDoc { get; set; }
        [XmlAttribute("Estatus")]
        public string status { get; set; }
        [XmlAttribute("Usuario_autorizador")]
        public string authUser { get; set; }
        [XmlAttribute("Fecha_autorizacion")]
        public string authDate { get; set; }
        [XmlAttribute("Comentarios_vendedor")]
        public string draftComments { get; set; }
        [XmlAttribute("Comentarios_autorizador")]
        public string authComments { get; set; }        
    }
}