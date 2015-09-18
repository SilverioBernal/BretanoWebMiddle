using Orkidea.Bretano.WebMiddle.BackEnd.Business;
using Orkidea.Bretano.WebMiddle.BackEnd.Contracts;
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

        #region Business Partners
        public List<BusinessPartner> GetBusinessPartners(CardType cardType)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetBusinessPartners(cardType);
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

        public BusinessPartner GetBusinessPartner(string cardCode)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizBusinessPartner);
                return facade.GetBusinessPartner(cardCode);
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

        #region Purchase Order
        public bool AddPruchaseOrder(MarketingDocument document)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizPurchaseOrder);
                return facade.AddPruchaseOrder(document);
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
        public List<Item> GetItems()
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizInventory);
                return facade.GetItems();
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

        public List<Item> GetItemList(Warehouse warehouse)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizInventory);
                return facade.GetItemList(warehouse);
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

        public List<StockLevel> GetItemStockLevel(string itemCode)
        {
            try
            {
                facade = new BizFacade(BusinessClass.BizInventory);
                return facade.GetItemStockLevel(itemCode);
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
    }
}
