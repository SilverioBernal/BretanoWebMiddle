using Orkidea.Framework.SAP.BusinessOne.Entities.BusinessPartners;
using Orkidea.Framework.SAP.BusinessOne.Entities.Finance;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Administration;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Reports;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.UserDefinedFileds;
using Orkidea.Framework.SAP.BusinessOne.Entities.Inventory;
using Orkidea.Framework.SAP.BusinessOne.Entities.MarketingDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Contracts
{
    [ServiceContract(Namespace = "http://WSSAP", Name = "WSSAP")]
    public interface iWSSAP
    {
        #region Management
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<AuthorizationStatus> GetAuthorizationStatusList(DateTime startDate, DateTime endDate, AppConnData oAppConnData);
        #endregion

        #region Business Partners
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<GenericBusinessPartner> GetBusinessPartners(CardType cardType, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<GenericBusinessPartner> GetBusinessPartnersBySalesPerson(CardType cardType, string slpCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<GenericBusinessPartner> GetBusinessPartnersByIds(CardType cardType, string[] cardCodes, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        BusinessPartner GetBusinessPartner(string cardCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        bool GetBusinessPartnerCreditStatus(string cardCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        int GetOldestOpenInvoice(string cardCode, AppConnData oAppConnData);
        
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<BusinessPartnerGroup> GetAllBusinessPratnerGroup(CardType cardType, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        void AddBusinessPartner(BusinessPartner partner, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        void UpdateBusinessPartner(BusinessPartner partner, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        void AddBusinessPartnerContact(ContactEmployee contact, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<ContactEmployee> GetContactList(string cardCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<BusinessPartnerAddress> GetAddressList(string cardCode, AddressType addressType, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<PaymentAge> GetPaymentAgeList(string cardCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<BusinessPartnerProp> GetBusinessPartnerPropList(AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<ItemPrice> GetBusinessPartnerLastPricesList(string cardCode, DateTime from, DateTime to, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        void AddBusinessPartnerAddress(BusinessPartnerAddress address, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<BusinessPartnerDunninTerm> GetDunninTermList(AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<BusinessPartnerWithholdingTax> GetBusinessPartnerWithholdingTaxList(string cardCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        void AddBusinessPartnerWithholdingTax(BusinessPartnerWithholdingTax withholdingTax, AppConnData oAppConnData);
        #endregion

        #region Sales Order
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        MarketingDocument AddSalesOrder(MarketingDocument document, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<MarketingDocument> ProcessBatchTransaction(List<MarketingDocument> documents, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<LightMarketingDocument> ListSaleOrders(DateTime startDate, DateTime endDate, string cardCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<LightMarketingDocument> ListSaleOrdersFiltered(DateTime startDate, DateTime endDate, char fieldFilter, string slp_card_Code, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        LightMarketingDocument GetSingleOrder(string docNum, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        int GetOrderNum(int docEntry, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        void CancelOrder(int docEntry, AppConnData oAppConnData);
        #endregion

        #region Quotation
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        MarketingDocument AddQuotation(MarketingDocument document, AppConnData oAppConnData);
        #endregion

        #region Inventory
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<GenericItem> GetItems(AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<GenericItem> GetItemList(Warehouse warehouse, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        Item GetItem(string itemCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<StockLevel> GetItemStockLevel(string itemCode, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        double GetItemPrice(string itemCode, int priceList, AppConnData oAppConnData);
        #endregion

        #region Finance
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<SalesTaxCode> GetSalesTaxCodeList(AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<WithholdingTax> GetWithholdingTax(AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        SalesTaxCode GetSingleTaxCode(string taxCode, AppConnData oAppConnData);
        #endregion

        #region Common

        #region Currencies
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<Currency> GetCurrencyList(AppConnData oAppConnData);
        #endregion

        #region Document series
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<DocumentSeries> GetDocumentSeriesList(SapDocumentType docType, AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        DocumentSeries GetDocumentSeriesSingle(int series, AppConnData oAppConnData);
        #endregion

        #region SalesPerson
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<SalesPerson> GetSalesPersonList(AppConnData oAppConnData);
        #endregion

        #region PaymentTerms
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<PaymentTerm> GetPaymentTermList(AppConnData oAppConnData);
        #endregion

        #region Country/state
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<Country> GetCountryList(AppConnData oAppConnData);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<State> GetStateList(string countryCode, AppConnData oAppConnData);
        #endregion

        #region Distribution rules
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<SapDistributionRule> GetDistributionRulesList(AppConnData oAppConnData);
        #endregion

        #region UDF's
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<UserDefinedFieldValue> GetUserDefinedFieldValuesList(string tableId, string fieldId, AppConnData oAppConnData);

        #endregion

        #region UDO's
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        List<UserDefinedFieldValue> GetUdoGenericKeyValueList(string tableId, AppConnData oAppConnData);
        #endregion
        #endregion
    }
}
