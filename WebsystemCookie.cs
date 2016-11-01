using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for Websystem
/// </summary>
/// 
namespace WebSystem
{

    public class Cookies
    {
        public Cookies()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public static string getCookie(string cookieName, string key)
        {
            string retVal = null;

            //Assuming user comes back after several hours. several < 12.
            //Read the cookie from Request.
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (myCookie == null)
            {
                //No cookie found or cookie expired.
                //Handle the situation here, Redirect the user or simply return;
                return retVal;
            }

            //ok - cookie is found.
            //Gracefully check if the cookie has the key-value as expected.
            if (!string.IsNullOrEmpty(myCookie.Values[key]))
            {
                string keyValue = myCookie.Values[key].ToString();
                //Yes userId is found. Mission accomplished.
                retVal = keyValue;

            }
            return retVal;
        }

    }

}