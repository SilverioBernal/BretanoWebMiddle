using Orkidea.Framework.SAP.BusinessOne.Entities.BusinessPartners;
using Orkidea.Framework.SAP.BusinessOne.Entities.Inventory;
using Orkidea.Framework.SAP.BusinessOne.Entities.MarketingDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public class BizFacade
    {
        #region Atributos        
        BizBusinessPartner bizBusinessPartner;
        BizPurchaseOrder bizPurchaseOrder;
        BizInventory bizInventory;
        #endregion

        #region Constructor
        public BizFacade(BusinessClass businessClass)
        {
            switch (businessClass)
            {
                case BusinessClass.BizBusinessPartner:
                    bizBusinessPartner = new BizBusinessPartner();
                    break;
                case BusinessClass.BizPurchaseOrder:
                    bizPurchaseOrder = new BizPurchaseOrder();
                    break;
                case BusinessClass.BizInventory:
                    bizInventory = new BizInventory();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Métodos
        #region Business Partners
        public List<BusinessPartner> GetBusinessPartners(CardType cardType)
        {
            return bizBusinessPartner.GetList(cardType);
        }

        public BusinessPartner GetBusinessPartner(string cardCode)
        {
            return bizBusinessPartner.GetSingle(cardCode);
        } 
        #endregion        

        #region Purchase Order        
        public bool AddPruchaseOrder(MarketingDocument document)
        {
            return bizPurchaseOrder.Add(document);
        }
        #endregion   
     
        #region Inventory
        public List<Item> GetItems()
        {
            return bizInventory.GetItemAll();
        }

        public List<Item> GetItemList(Warehouse warehouse)
        {
            return bizInventory.GetItemList(warehouse);
        }

        public List<StockLevel> GetItemStockLevel(string itemCode)
        {
            return bizInventory.GetItemStockLevel(itemCode);
        }
        #endregion   
        #endregion
    }
}
