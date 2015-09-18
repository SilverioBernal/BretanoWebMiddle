using Orkidea.Framework.SAP.BusinessOne.Entities.BusinessPartners;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Inventory;
using Orkidea.Framework.SAP.BusinessOne.Entities.MarketingDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Contracts
{
    [ServiceContract(Namespace = "http://WSSAP", Name = "WSSAP")]
    public interface iWSSAP
    {        
        #region Business Partners
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<BusinessPartner> GetBusinessPartners(CardType cardType);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        BusinessPartner GetBusinessPartner(string cardCode);
        #endregion

        #region Purchase Order
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        bool AddPruchaseOrder(MarketingDocument document);
        #endregion

        #region Inventory
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<Item> GetItems();

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<Item> GetItemList(Warehouse warehouse);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<StockLevel> GetItemStockLevel(string itemCode);
        #endregion   
    }
}
