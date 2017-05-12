using Orkidea.Bretano.WebMiddle.BackEnd.Contracts;
using Orkidea.Bretano.WebMiddle.FrontEnd.Business;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Framework.SAP.BusinessOne.Entities.BusinessPartners;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChannelFactory<iWSSAP> factory = null;
            try
            {
                AppConnData appConnData = new AppConnData();
                appConnData = GetAppConnData(7);
                BasicHttpBinding binding = new BasicHttpBinding();
                EndpointAddress address = new EndpointAddress("http://localhost:60593/WSSAP.svc");
                factory = new ChannelFactory<iWSSAP>(binding, address);
                iWSSAP channel = factory.CreateChannel();
                List<GenericBusinessPartner> resturnmessage = channel.GetBusinessPartners(CardType.Customer, appConnData);
                Console.WriteLine(resturnmessage);
                Console.ReadKey(true);
            }
            catch (CommunicationException)
            {
                if (factory != null)
                    factory.Abort();
            }
            catch (TimeoutException)
            {
                if (factory != null)
                    factory.Abort();
            }
            catch (Exception ex)
            {
                if (factory != null)
                    factory.Abort();
                Console.WriteLine(ex.Message);
            }
        }

        private AppConnData GetAppConnData(int companyId)
        {
            Company company = BizCompany.GetSingle(companyId);

            return new AppConnData()
            {
                adoConnString = company.connStringName,
                dataBaseName = company.dataBaseName,
                sapUser = company.sapUser,
                sapUserPassword = company.sapPassword,
                wsAppKey = ConfigurationManager.AppSettings["WSAppKey"].ToString(),
                wsSecret = ConfigurationManager.AppSettings["WSSecret"].ToString()
            };
        }
    }
}
