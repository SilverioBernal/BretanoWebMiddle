using Orkidea.Bretano.WebMiddle.FrontEnd.DAL;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Business
{
    public static class BizParameter
    {
        public static IList<Parameter> GetList()
        {
            return DbMngmt<Parameter>.GetList();
        }

        public static Parameter GetSingle(int id)
        {
            return DbMngmt<Parameter>.GetSingle(c => c.id.Equals(id));
        }

        public static Parameter GetSingle(string name)
        {
            return DbMngmt<Parameter>.GetSingle(c => c.name.Equals(name));
        }

        public static void Add(params Parameter[] Parameters)
        {            
            try
            {
                foreach (Parameter item in Parameters)
                {
                    DbMngmt<Parameter>.Add(Parameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public static void Update(WebUserAction action, params WebUser[] WebUsers)
        //{
        //    EntityCRUD<WebUser> ec = new EntityCRUD<WebUser>();

        //    try
        //    {
        //        foreach (WebUser item in WebUsers)
        //        {
        //            switch (action)
        //            {
        //                case WebUserAction.AdminReset:
        //                    item.pass = Cryptography.Encrypt(HexSerialization.StringToHex(PasswordHelper.Generate()));
        //                    break;
        //                case WebUserAction.UserReset:
        //                    item.pass = Cryptography.Encrypt(HexSerialization.StringToHex(item.pass));
        //                    break;
        //                case WebUserAction.Disable:
        //                    item.active = false;
        //                    break;
        //                case WebUserAction.Enable:
        //                    item.active = true;
        //                    break;
        //                default:
        //                    break;
        //            }

        //            ec.Update(WebUsers);

        //            //SendWebUserNotification(item, action);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void Remove(params WebUser[] WebUsers)
        //{
        //    EntityCRUD<WebUser> ec = new EntityCRUD<WebUser>();
        //    ec.Remove(WebUsers);
        //}
    }
}
