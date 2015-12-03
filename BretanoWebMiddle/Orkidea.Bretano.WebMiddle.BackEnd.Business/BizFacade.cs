using Orkidea.Framework.SAP.BusinessOne.Entities.BusinessPartners;
using Orkidea.Framework.SAP.BusinessOne.Entities.Finance;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Administration;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Reports;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.UserDefinedFileds;
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
        BizSaleOrder bizSalesOrder;
        BizQuotation bizQuotation;
        BizInventory bizInventory;
        BizCommon bizCommon;
        BizFinance bizFinance;
        #endregion

        #region Constructor
        public BizFacade(BusinessClass businessClass)
        {
            switch (businessClass)
            {
                case BusinessClass.BizBusinessPartner:
                    bizBusinessPartner = new BizBusinessPartner();
                    break;
                case BusinessClass.BizSalesOrder:
                    bizSalesOrder = new BizSaleOrder();
                    break;
                case BusinessClass.BizQuotation:
                    bizQuotation = new BizQuotation();
                    break;
                case BusinessClass.BizInventory:
                    bizInventory = new BizInventory();
                    break;
                case BusinessClass.BizFinance:
                    bizFinance = new BizFinance();
                    break;
                case BusinessClass.BizCommon:
                    bizCommon = new BizCommon();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Métodos

        #region Business Partners
        public List<GenericBusinessPartner> GetBusinessPartners(CardType cardType, AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetList(cardType, oAppConnData);
        }

        public BusinessPartner GetBusinessPartner(string cardCode, AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetSingle(cardCode, oAppConnData);
        }

        public List<BusinessPartnerGroup> GetAllBusinessPratnerGroup(CardType cardType, AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetAllBusinessPartnerGroup(cardType, oAppConnData);
        }

        public List<ContactEmployee> GetContactList(string cardCode, AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetContactList(cardCode, oAppConnData);
        }

        public List<BusinessPartnerAddress> GetAddressList(string cardCode, AddressType addressType, AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetAddressList(cardCode, addressType, oAppConnData);
        }

        public List<PaymentAge> GetPaymentAgeList(string cardCode, AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetPaymentAgeList(cardCode, oAppConnData);
        }

        public List<BusinessPartnerProp> GetBusinessPartnerPropList(AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetBusinessPartnerPropList(oAppConnData);
        }

        public List<ItemPrice> GetBusinessPartnerLastPricesList(string cardCode, DateTime from, DateTime to, AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetBusinessPartnerLastPricesList(cardCode, from, to, oAppConnData);
        }

        public void AddBusinessPartner(BusinessPartner partner, AppConnData oAppConnData)
        {
            bizBusinessPartner.Add(partner, oAppConnData);
        }

        public void UpdateBusinessPartner(BusinessPartner partner, AppConnData oAppConnData)
        {
            bizBusinessPartner.Update(partner, oAppConnData);
        }

        public void AddBusinessPartnerContact(ContactEmployee contact, AppConnData oAppConnData)
        {
            bizBusinessPartner.AddContact(contact, oAppConnData);
        }

        public void AddBusinessPartnerAddress(BusinessPartnerAddress address, AppConnData oAppConnData)
        {
            bizBusinessPartner.AddAddress(address, oAppConnData);
        }

        public List<BusinessPartnerDunninTerm> GetDunninTermList(AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetDunninTermList(oAppConnData);
        }

        public List<BusinessPartnerWithholdingTax> GetBusinessPartnerWithholdingTaxList(string cardCode, AppConnData oAppConnData)
        {
            return bizBusinessPartner.GetBusinessPartnerWithholdingTaxList(cardCode, oAppConnData);
        }

        public bool AddBusinessPartnerWithholdingTax(BusinessPartnerWithholdingTax withholdingTax, AppConnData oAppConnData)
        {
            return bizBusinessPartner.AddBusinessPartnerWithholdingTax(withholdingTax, oAppConnData);
        }        
        #endregion

        #region Sales Order
        public MarketingDocument AddSalesOrder(MarketingDocument document, AppConnData oAppConnData)
        {
            return bizSalesOrder.Add(document, oAppConnData);
        }

        public List<LightMarketingDocument> ListSaleOrders(DateTime startDate, DateTime endDate, string cardCode, AppConnData oAppConnData)
        {
            return bizSalesOrder.List(startDate, endDate, cardCode, oAppConnData);
        }

        public LightMarketingDocument GetSingleOrder(string docNum, AppConnData oAppConnData) 
        {
            return bizSalesOrder.GetSingle(docNum, oAppConnData);
        }

        public void CancelOrder(int docEntry, AppConnData oAppConnData)
        {
            bizSalesOrder.Cancel(docEntry, oAppConnData);
        }
        #endregion

        #region Quotation
        public MarketingDocument AddQuotation(MarketingDocument document, AppConnData oAppConnData)
        {
            return bizQuotation.Add(document, oAppConnData);
        }        
        #endregion

        #region Inventory
        public List<GenericItem> GetItems(AppConnData oAppConnData)
        {
            return bizInventory.GetItemAll(oAppConnData);
        }

        public List<GenericItem> GetItemList(Warehouse warehouse, AppConnData oAppConnData)
        {
            return bizInventory.GetItemList(warehouse, oAppConnData);
        }

        public List<StockLevel> GetItemStockLevel(string itemCode, AppConnData oAppConnData)
        {
            return bizInventory.GetItemStockLevel(itemCode, oAppConnData);
        }

        public double GetItemPrice(string itemCode, int priceList, AppConnData oAppConnData)
        {
            return bizInventory.GetItemPrice(itemCode, priceList, oAppConnData);
        }
        #endregion

        #region Finance
        public List<SalesTaxCode> GetSalesTaxCodeList(AppConnData oAppConnData)
        {

            List<SalesTaxCode> taxes = bizFinance.GetSalesTaxCodeList(oAppConnData).Where(x => x.name.Equals("IVA VENTAS 16%")).ToList();
            return taxes;
        }

        public List<WithholdingTax> GetWithholdingTax(AppConnData oAppConnData)
        {
            return bizFinance.GetWithholdingTax(oAppConnData);
        }
        #endregion

        #region Common

        #region Currencies
        public List<Currency> GetCurrencyList(AppConnData oAppConnData)
        {
            return bizCommon.GetCurrencyList(oAppConnData);
        }
        #endregion

        #region Document series
        public List<DocumentSeries> GetDocumentSeriesList(SapDocumentType docType, AppConnData oAppConnData)
        {
            return bizCommon.GetDocumentSeriesList(docType, oAppConnData);
        }

        public DocumentSeries GetDocumentSeriesSingle(int series, AppConnData oAppConnData)
        {
            return bizCommon.GetDocumentSeriesSingle(series, oAppConnData);
        }
        #endregion

        #region SalesPerson
        public List<SalesPerson> GetSalesPersonList(AppConnData oAppConnData)
        {
            return bizCommon.GetSalesPersonList(oAppConnData);
        }
        #endregion

        #region PaymentTerms
        public List<PaymentTerm> GetPaymentTermList(AppConnData oAppConnData)
        {
            return bizCommon.GetPaymentTermList(oAppConnData);
        }
        #endregion

        #region Country/state
        public List<Country> GetCountryList(AppConnData oAppConnData)
        {
            return bizCommon.GetCountryList(oAppConnData);
        }

        public List<State> GetStateList(string countryCode, AppConnData oAppConnData)
        {
            return bizCommon.GetStateList(countryCode,oAppConnData);
        }
        #endregion

        #region Distribution rules
        public List<SapDistributionRule> GetDistributionRulesList(AppConnData oAppConnData)
        {
            return bizCommon.GetDistributionRulesList(oAppConnData);
        }

        #endregion

        #region UDF's
        public List<UserDefinedFieldValue> GetUserDefinedFieldValuesList(string tableId, string fieldId, AppConnData oAppConnData)
        {
            return bizCommon.GetUserDefinedFieldValuesList(tableId, fieldId, oAppConnData);
        }
        #endregion

        #region UDO's
        public List<UserDefinedFieldValue> GetUdoGenericKeyValueList(string tableId, AppConnData oAppConnData)
        {
            return bizCommon.GetUdoGenericKeyValueList(tableId, oAppConnData);
        }
        #endregion

        #endregion

        #endregion
    }
}
