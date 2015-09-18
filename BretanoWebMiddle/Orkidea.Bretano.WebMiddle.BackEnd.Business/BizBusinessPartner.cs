using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient;
using Orkidea.Framework.SAP.BusinessOne.DiApiClient.SecurityData;
using Orkidea.Framework.SAP.BusinessOne.Entities.BusinessPartners;
using Orkidea.Framework.SAP.BusinessOne.Entities.Global.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
        public SAPConnectionData DataConnection;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public BizBusinessPartner()
        {
            DataConnection = new SAPConnectionData();
        }
        #endregion

        #region Methods
        public List<BusinessPartner> GetAll()
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                BusinessPartnerAccess = new BusinessPartnerData();
                List<BusinessPartner> businessPartners = BusinessPartnerAccess.GetAll();
                

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

        public List<BusinessPartner> GetList(CardType cardType)
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                BusinessPartnerAccess = new BusinessPartnerData();
                List<BusinessPartner> businessPartners = BusinessPartnerAccess.GetList(cardType);


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

        public BusinessPartner GetSingle(string cardCode)
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                BusinessPartnerAccess = new BusinessPartnerData();
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

        public bool Add(BusinessPartner partner)
        {
            try
            {
                //if (!Util.ValidarDatosAccesoServicio(conexion))
                //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");

                BusinessPartnerAccess = new BusinessPartnerData();
                BusinessPartnerAccess.Add(partner);

                return true;
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
        #endregion
    }
}
