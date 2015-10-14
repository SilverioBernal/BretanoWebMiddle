﻿using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Administration;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using Orkidea.Framework.SAP.BusinessOne.Entities.MarketingDocuments;
using Orkidea.Framework.Security;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public class BizSaleOrder
    {
        #region Attributes
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private MarketingDocumentData SaleOrderAccess;
        public SAPConnectionData DataConnection;
        #endregion

        #region Constructor
        public BizSaleOrder()
        {
            DataConnection = new SAPConnectionData();
        }
        #endregion

        #region Methods
        public MarketingDocument Add(MarketingDocument document, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                string licenseServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["licenseServer"]));
                string dbServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbServer"]));
                string dbUser = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUser"]));
                string dbUserPassword = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUserPassword"]));
                string serverType = ConfigurationManager.AppSettings["serverType"];

                DataConnection = new SAPConnectionData(oAppConnData.dataBaseName, licenseServer, dbServer, oAppConnData.sapUser, oAppConnData.sapUserPassword, dbUser, dbUserPassword, serverType);

                if (DataConnection.ConnectCompany(oAppConnData.dataBaseName, oAppConnData.sapUser, oAppConnData.sapUserPassword))
                {
                    DataConnection.BeginTran();
                    SaleOrderAccess = new MarketingDocumentData();
                    document = SaleOrderAccess.Add(SapDocumentType.SalesOrder, document, DataConnection.Conn);
                    DataConnection.EndTran(BoWfTransOpt.wf_Commit);
                    return document;
                }


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
            return document;
        }
        #endregion
    }
}