using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public class BizInventory
    {
         #region Attributes
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private InventoryData InventoryAccess;
        public SAPConnectionData DataConnection;
        #endregion

        #region Constructor
        public BizInventory ()
        {
            DataConnection = new SAPConnectionData();
        }
        #endregion

        #region Methods
        public List<Item> GetItemAll()
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                InventoryAccess = new InventoryData();
                return InventoryAccess.GetItemAll();                
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            catch (BusinessException ex)
            {
                BizUtilities.ProcessBusinessException(ex);
            }
            catch (Exception ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            return null;
        }

        public List<Item> GetItemList(Warehouse warehouse)
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                InventoryAccess = new InventoryData();
                return InventoryAccess.GetItemList(warehouse);
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            catch (BusinessException ex)
            {
                BizUtilities.ProcessBusinessException(ex);
            }
            catch (Exception ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            return null;
        }

        public List<StockLevel> GetItemStockLevel(string itemCode)
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                InventoryAccess = new InventoryData();
                return InventoryAccess.GetItemStockLevel(itemCode);
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            catch (BusinessException ex)
            {
                BizUtilities.ProcessBusinessException(ex);
            }
            catch (Exception ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw ex;
                }
            }
            return null;
        }
        #endregion
    }
}
