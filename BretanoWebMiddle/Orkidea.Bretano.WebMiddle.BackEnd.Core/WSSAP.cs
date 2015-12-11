using Orkidea.Bretano.WebMiddle.BackEnd.Business;
using Orkidea.Bretano.WebMiddle.BackEnd.Contracts;
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
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Core
{
    public class WSSAP:iWSSAP
    {
        #region Atributos
        /// <summary>
        /// Acceso a la capa de negocios
        /// </summary>
        private BizFacade facade;
        #endregion

        #region Management
        public List<AuthorizationStatus> GetAuthorizationStatusList(DateTime startDate, DateTime endDate, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizManagement);
                return facade.GetAuthorizationStatusList(startDate, endDate, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region Business Partners
        public List<GenericBusinessPartner> GetBusinessPartners(CardType cardType, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetBusinessPartners(cardType, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        //public List<BusinessPartner> GetBusinessPartnersJson(CardType cardType, AppConnData oAppConnData)
        //{
        //    try
        //    {
        //        facade = new BizFacade(BusinessClass.BizBusinessPartner);
        //        return facade.GetBusinessPartners(cardType, oAppConnData);
        //    }
        //    catch (Exception ex)
        //    {
        //        DataAccessFault detalleError = new DataAccessFault();
        //        foreach (string valores in ex.Data.Keys)
        //        {
        //            switch (valores)
        //            {
        //                case "1": detalleError.ErrorID = ex.Data[valores].ToString();
        //                    break;
        //                case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
        //                    break;
        //                case "3": detalleError.Description = ex.Data[valores].ToString();
        //                    break;
        //                default: detalleError.ErrorID = ex.Data[valores].ToString();
        //                    break;
        //            }
        //        }
        //        throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
        //    }
        //}

        public BusinessPartner GetBusinessPartner(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetBusinessPartner(cardCode, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<BusinessPartnerGroup> GetAllBusinessPratnerGroup(CardType cardType, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetAllBusinessPratnerGroup(cardType, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<ContactEmployee> GetContactList(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetContactList(cardCode, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<BusinessPartnerAddress> GetAddressList(string cardCode, AddressType addressType, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetAddressList(cardCode, addressType, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public void AddBusinessPartner(BusinessPartner partner, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                facade.AddBusinessPartner(partner, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public void UpdateBusinessPartner(BusinessPartner partner, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                facade.UpdateBusinessPartner(partner, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public void AddBusinessPartnerContact(ContactEmployee contact, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                facade.AddBusinessPartnerContact(contact, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public void AddBusinessPartnerAddress(BusinessPartnerAddress address, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                facade.AddBusinessPartnerAddress(address, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<PaymentAge> GetPaymentAgeList(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetPaymentAgeList(cardCode, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<BusinessPartnerProp> GetBusinessPartnerPropList(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetBusinessPartnerPropList(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<ItemPrice> GetBusinessPartnerLastPricesList(string cardCode, DateTime from, DateTime to, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetBusinessPartnerLastPricesList(cardCode, from, to, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<BusinessPartnerDunninTerm> GetDunninTermList(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetDunninTermList(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<BusinessPartnerWithholdingTax> GetBusinessPartnerWithholdingTaxList(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetBusinessPartnerWithholdingTaxList(cardCode, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public void AddBusinessPartnerWithholdingTax(BusinessPartnerWithholdingTax withholdingTax, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                facade.AddBusinessPartnerWithholdingTax(withholdingTax, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }        
        #endregion

        #region Sales Order
        public MarketingDocument AddSalesOrder(MarketingDocument document, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizSalesOrder);
                return facade.AddSalesOrder(document, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<LightMarketingDocument> ListSaleOrders(DateTime startDate, DateTime endDate, string cardCode, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizSalesOrder);
                return facade.ListSaleOrders(startDate, endDate, cardCode, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public LightMarketingDocument GetSingleOrder(string docNum, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizSalesOrder);
                return facade.GetSingleOrder(docNum, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public void CancelOrder(int docEntry, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizSalesOrder);
                facade.CancelOrder(docEntry, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region Quotation        
        public MarketingDocument AddQuotation(MarketingDocument document, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizQuotation);
                return facade.AddQuotation(document, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region Inventory
        public List<GenericItem> GetItems(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizInventory);
                return facade.GetItems(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<GenericItem> GetItemList(Warehouse warehouse, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizInventory);
                return facade.GetItemList(warehouse, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<StockLevel> GetItemStockLevel(string itemCode, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizInventory);
                return facade.GetItemStockLevel(itemCode, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public double GetItemPrice(string itemCode, int priceList, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizInventory);
                return facade.GetItemPrice(itemCode,priceList, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion   

        #region Finance        
        public List<SalesTaxCode> GetSalesTaxCodeList(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizFinance);
                return facade.GetSalesTaxCodeList(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public List<WithholdingTax> GetWithholdingTax(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizFinance);
                return facade.GetWithholdingTax(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion   

        #region Common

        #region Currencies
        public List<Currency> GetCurrencyList(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetCurrencyList(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region Document series        
        public List<DocumentSeries> GetDocumentSeriesList(SapDocumentType docType, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetDocumentSeriesList(docType, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        public DocumentSeries GetDocumentSeriesSingle(int series, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetDocumentSeriesSingle(series, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region SalesPerson
        public List<SalesPerson> GetSalesPersonList(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetSalesPersonList(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region PaymentTerms
        public List<PaymentTerm> GetPaymentTermList(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetPaymentTermList(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region Country/state

        public List<Country> GetCountryList(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetCountryList(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }


        public List<State> GetStateList(string countryCode, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetStateList(countryCode, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region Distribution rules
        public List<SapDistributionRule> GetDistributionRulesList(AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetDistributionRulesList(oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }

        #endregion

        #region UDF's
        public List<UserDefinedFieldValue> GetUserDefinedFieldValuesList(string tableId, string fieldId, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetUserDefinedFieldValuesList(tableId, fieldId, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion

        #region UDO's
        public List<UserDefinedFieldValue> GetUdoGenericKeyValueList(string tableId, AppConnData oAppConnData)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizCommon);
                return facade.GetUdoGenericKeyValueList(tableId, oAppConnData);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }
        }
        #endregion
        #endregion
    }
}

