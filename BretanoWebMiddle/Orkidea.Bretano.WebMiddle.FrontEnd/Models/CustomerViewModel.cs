using Orkidea.Bretano.WebMiddle.FrontEnd.WebMiddleBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Models
{
    public class CustomerViewModel : BusinessPartner
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

        public List<BusinessPartnerGroup> bpGroups { get; set; }
        public List<Currency> currencies { get; set; }
        public List<SalesPerson> salesPersons { get; set; }
        public List<PaymentTerm> paymentTerms { get; set; }
        public List<UserDefinedFieldValue> tributaryRegs { get; set; }
        public List<UserDefinedFieldValue> docTypes { get; set; }
        public List<UserDefinedFieldValue> magneticMediaCities { get; set; }
        public List<UserDefinedFieldValue> personTypes { get; set; }
        public List<UserDefinedFieldValue> ivaClasses { get; set; }
        public List<UserDefinedFieldValue> partnerSegments { get; set; }
        public Dictionary<string, string> invoiceDays { get; set; }
        public List<BusinessPartnerDunninTerm> dunningTerms { get; set; }

        public CustomerViewModel()
        {
            bpTypes = new Dictionary<string, string>();

            bpTypes.Add("C", "Customer");
            bpTypes.Add("L", "Lead");
        }

        public CustomerViewModel(AppConnData appConnData)
        {
            WSSAPClient backEnd = new WSSAPClient();
            bpGroups = backEnd.GetAllBusinessPratnerGroup(CardType.Customer, appConnData);
            currencies = backEnd.GetCurrencyList(appConnData);

            salesPersons = backEnd.GetSalesPersonList(appConnData);
            paymentTerms = backEnd.GetPaymentTermList(appConnData);
            tributaryRegs = backEnd.GetUdoGenericKeyValueList("@BPCO_RT", appConnData);
            docTypes = backEnd.GetUdoGenericKeyValueList("@BPCO_TD", appConnData);
            magneticMediaCities = backEnd.GetUdoGenericKeyValueList("@BPCO_MU", appConnData);
            personTypes = backEnd.GetUserDefinedFieldValuesList("OCRD", "4", appConnData);
            ivaClasses = backEnd.GetUserDefinedFieldValuesList("OCRD", "111", appConnData);
            partnerSegments = backEnd.GetUserDefinedFieldValuesList("OCRD", "125", appConnData);
            dunningTerms = backEnd.GetDunninTermList(appConnData);

            invoiceDays = new Dictionary<string, string>();
            
            for (int i = 1; i < 31; i++)
                invoiceDays.Add(i.ToString(), i.ToString());
        }


    }
}