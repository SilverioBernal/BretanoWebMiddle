﻿using Orkidea.Bretano.WebMiddle.FrontEnd.Business.Enums;
using Orkidea.Bretano.WebMiddle.FrontEnd.DAL;
using Orkidea.Bretano.WebMiddle.FrontEnd.Entities;
using Orkidea.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Business
{
    public static class BizWebUser
    {
        public static IList<WebUser> GetList()
        {
            return DbMngmt<WebUser>.GetList();
        }

        public static WebUser GetSingle(int id)
        {
            return DbMngmt<WebUser>.GetSingle(c => c.id.Equals(id));
        }

        public static WebUser GetSingle(string name)
        {
            return DbMngmt<WebUser>.GetSingle(c => c.name.Equals(name));
        }

        public static void Add(params WebUser[] WebUsers)
        {
            try
            {
                foreach (WebUser item in WebUsers)
                {
                    //item.pass = Cryptography.Encrypt(HexSerialization.StringToHex(PasswordHelper.Generate()));
                    DbMngmt<WebUser>.Add(WebUsers);

                    //SendWebUserNotification(item, WebUserAction.New);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(WebUserAction action, params WebUser[] WebUsers)
        {
            try
            {
                foreach (WebUser item in WebUsers)
                {
                    switch (action)
                    {
                        case WebUserAction.AdminReset:
                            item.pass = Cryptography.Encrypt(HexSerialization.StringToHex(PasswordHelper.Generate()));
                            break;
                        case WebUserAction.UserReset:
                            item.pass = Cryptography.Encrypt(HexSerialization.StringToHex(item.pass));
                            break;
                        case WebUserAction.Disable:
                            item.active = false;
                            break;
                        case WebUserAction.Enable:
                            item.active = true;
                            break;
                        default:
                            break;
                    }

                    DbMngmt<WebUser>.Update(WebUsers);

                    //SendWebUserNotification(item, action);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove(params WebUser[] WebUsers)
        {
            DbMngmt<WebUser>.Remove(WebUsers);
        }
    }
}
