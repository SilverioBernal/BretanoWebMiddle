using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.BusinessPartners;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Misc;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.Reports;
using Orkidea.Framework.Security;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.BackEnd.Business
{
    public class BizBusinessPartner
    {
        #region Attributes
        /// <summary>
        /// Permite el acceso módulo de socio de negocios
        /// </summary>
        private BusinessPartnerData BusinessPartnerAccess;
        private BusinessPartnerGroupData BusinessPartnerGroupAccess;
        public SAPConnectionData DataConnection;        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public BizBusinessPartner()
        {
            //DataConnection = new SAPConnectionData();
        }        
        #endregion

        #region Methods
        public List<GenericBusinessPartner> GetAll(AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                List<GenericBusinessPartner> businessPartners = BusinessPartnerAccess.GetAll();
                

                //if (socio.CardCode.Length == 0)
                //    throw new BusinessException(42, "El valor enviado: " + codigoSocio + "no esta registrado como un cliente en SAP Business One");
                return businessPartners;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
                    outEx.Data.Add("3", outEx.Message +  " Descripción: " + ex.Message);
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

        public List<GenericBusinessPartner> GetList(CardType cardType, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);

                List<GenericBusinessPartner> businessPartners = BusinessPartnerAccess.GetList(cardType);


                //if (socio.CardCode.Length == 0)
                //    throw new BusinessException(42, "El valor enviado: " + codigoSocio + "no esta registrado como un cliente en SAP Business One");
                return businessPartners;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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

        public List<GenericBusinessPartner> GetList(CardType cardType, string slpCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);

                List<GenericBusinessPartner> businessPartners = BusinessPartnerAccess.GetList(cardType, slpCode);


                return businessPartners;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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

        public List<GenericBusinessPartner> GetList(CardType cardType, string[] cardCodes, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);

                List<GenericBusinessPartner> businessPartners = BusinessPartnerAccess.GetList(cardType, cardCodes);


                return businessPartners;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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

        public List<ContactEmployee> GetContactList(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                
                List<ContactEmployee> contacts = BusinessPartnerAccess.GetContactList(cardCode);
                
                return contacts;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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

        public List<BusinessPartnerAddress> GetAddressList(string cardCode, AddressType addressType, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);

                List<BusinessPartnerAddress> addresses = BusinessPartnerAccess.GetAddressList(cardCode, addressType);

                return addresses;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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

        public List<PaymentAge> GetPaymentAgeList(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                return BusinessPartnerAccess.GetPaymentAgeList(cardCode);               
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

        public List<BusinessPartnerProp> GetBusinessPartnerPropList(AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                return BusinessPartnerAccess.GetBusinessPartnerPropList();               
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

        public List<ItemPrice> GetBusinessPartnerLastPricesList(string cardCode, DateTime from, DateTime to, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                return BusinessPartnerAccess.GetBusinessPartnerLastPricesList(cardCode, from, to);
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

        public BusinessPartner GetSingle(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                BusinessPartner businessPartner = BusinessPartnerAccess.GetSingle(cardCode);


                //if (socio.CardCode.Length == 0)
                //    throw new BusinessException(42, "El valor enviado: " + codigoSocio + "no esta registrado como un cliente en SAP Business One");
                return businessPartner;
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

        public bool GetCreditStatus(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                return  BusinessPartnerAccess.GetCreditStatus(cardCode);                
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
            return false;
        }

        public int GetOldestOpenInvoice(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                
                return BusinessPartnerAccess.GetOldestOpenInvoice(cardCode);
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
            return -1;
        }

        public bool Add(BusinessPartner partner, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                string licenseServer =  Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["licenseServer"]));
                string dbServer = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbServer"]));
                string dbUser = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUser"]));
                string dbUserPassword = Cryptography.Decrypt(HexSerialization.HexToString(ConfigurationManager.AppSettings["dbUserPassword"]));
                string serverType = ConfigurationManager.AppSettings["serverType"];

                DataConnection = new SAPConnectionData(oAppConnData.dataBaseName, licenseServer, dbServer, oAppConnData.sapUser, oAppConnData.sapUserPassword, dbUser, dbUserPassword, serverType);
                //DataConnection.Conn = DataConnection.Conn.company;

                if (DataConnection.ConnectCompany(oAppConnData.dataBaseName, oAppConnData.sapUser, oAppConnData.sapUserPassword))
                {
                    DataConnection.BeginTran();
                    BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                    BusinessPartnerAccess.Add(partner, DataConnection.Conn);
                    DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);

                    
                    return true;
                }                
            }
            #region Catch
            catch (SAPException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");                
                return false;
            }
            catch (COMException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                    throw outEx;
                }
                else
                {
                    throw;
                }
                //return false;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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
                ex.Data.Add("1", ex.ErrorId);
                ex.Data.Add("2", "NA");
                ex.Data.Add("3", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                    //return 0;
                }
                return false;
            }
            #endregion
            return false;
        }

        public bool Update(BusinessPartner partner, AppConnData oAppConnData)
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
                //DataConnection.Conn = DataConnection.Conn.company;

                if (DataConnection.ConnectCompany(oAppConnData.dataBaseName, oAppConnData.sapUser, oAppConnData.sapUserPassword))
                {
                    DataConnection.BeginTran();
                    BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                    BusinessPartnerAccess.Update(partner, DataConnection.Conn);
                    DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
                    return true;
                }
            }
            #region Catch
            catch (SAPException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");
                return false;
            }
            catch (COMException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                    throw outEx;
                }
                else
                {
                    throw;
                }
                //return false;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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
                ex.Data.Add("1", ex.ErrorId);
                ex.Data.Add("2", "NA");
                ex.Data.Add("3", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message + " Descripción: " + ex.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                    //return 0;
                }
                return false;
            }
            #endregion
            return false;
        }

        public bool AddContact(ContactEmployee contact, AppConnData oAppConnData)
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
                    BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                    BusinessPartnerAccess.AddContact(contact, DataConnection.Conn);
                    DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
                    return true;
                }
            }
            #region Catch
            catch (SAPException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");
                return false;
            }
            catch (COMException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                try
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception)
                {                                       
                }

                throw new Exception(ex.Message + "::" + ex.StackTrace);
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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
                ex.Data.Add("1", ex.ErrorId);
                ex.Data.Add("2", "NA");
                ex.Data.Add("3", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                    //return 0;
                }
                return false;
            }
            #endregion
            return false;
        }

        public bool AddAddress(BusinessPartnerAddress address, AppConnData oAppConnData)
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
                    BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                    BusinessPartnerAccess.AddAddress(address, DataConnection.Conn);
                    DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
                    return true;
                }
            }
            #region Catch
            catch (SAPException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");
                return false;
            }
            catch (COMException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw;
                }
                
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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
                ex.Data.Add("1", ex.ErrorId);
                ex.Data.Add("2", "NA");
                ex.Data.Add("3", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                    //return 0;
                }
                return false;
            }
            #endregion
            return false;
        }

        public List<BusinessPartnerGroup> GetAllBusinessPartnerGroup(CardType cardType, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);                
                BusinessPartnerGroupAccess = new BusinessPartnerGroupData(oAppConnData.adoConnString);

                List<BusinessPartnerGroup> businessPartners = BusinessPartnerGroupAccess.GetList(cardType);
                
                return businessPartners;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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

        public List<BusinessPartnerDunninTerm> GetDunninTermList(AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                List<BusinessPartnerDunninTerm> businessPartners = BusinessPartnerAccess.GetDunninTermList();


                //if (socio.CardCode.Length == 0)
                //    throw new BusinessException(42, "El valor enviado: " + codigoSocio + "no esta registrado como un cliente en SAP Business One");
                return businessPartners;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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

        public List<BusinessPartnerWithholdingTax> GetBusinessPartnerWithholdingTaxList(string cardCode, AppConnData oAppConnData)
        {
            try
            {
                if (!BizUtilities.ValidateServiceConnection(oAppConnData))
                    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                oAppConnData = BizUtilities.GetDataConnection(oAppConnData);

                BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                List<BusinessPartnerWithholdingTax> contacts = BusinessPartnerAccess.GetBusinessPartnerWithholdingTaxList(cardCode);

                return contacts;
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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

        public bool AddBusinessPartnerWithholdingTax(BusinessPartnerWithholdingTax withholdingTax, AppConnData oAppConnData)
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
                    BusinessPartnerAccess = new BusinessPartnerData(oAppConnData.adoConnString);
                    BusinessPartnerAccess.AddBusinessPartnerWithholdingTax(withholdingTax, DataConnection.Conn);
                    DataConnection.EndTranAndRelease(BoWfTransOpt.wf_Commit);
                    return true;
                }
            }
            #region Catch
            catch (SAPException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                BizUtilities.ProcessSapException(ex, "Gestión de Pagos");
                return false;
            }
            catch (COMException ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                {
                    outEx.Data.Add("1", "3");
                    outEx.Data.Add("2", "NA");
                    outEx.Data.Add("3", outEx.Message);
                    throw outEx;
                }
                else
                {
                    throw;
                }
                
            }
            catch (DbException ex)
            {
                Exception outEx;
                if (ExceptionPolicy.HandleException(ex, "Politica_SQLServer", out outEx))
                {
                    outEx.Data.Add("1", "14");
                    outEx.Data.Add("2", "NA");
                    //outEx.Data.Add("3", outEx.Message);
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
                ex.Data.Add("1", ex.ErrorId);
                ex.Data.Add("2", "NA");
                ex.Data.Add("3", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                DataConnection.EndTranAndRelease(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Exception outEx;
                if (ex.Data["1"] == null)
                {
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;

                    }
                }
                else
                {
                    throw ex;
                    //return 0;
                }
                return false;
            }
            #endregion
            return false;
        }
        #endregion
    }
}
