using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for WebsystemClientscript
/// </summary>
/// 
namespace WebSystem
{

    public class Clientscript
    {
        public Clientscript()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public static void loadClientScript(string myScript, string uid, Type t)
        {
            //funkce nahrává klientský script do stránky


            Page page = HttpContext.Current.Handler as Page;
            // Define the name and type of the client scripts on the page.
            String clientScriptName = uid;
            Type cstype = t.GetType();// this..GetType();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = page.ClientScript;

            // Check to see if the client script is already registered.
            if (!cs.IsClientScriptBlockRegistered(cstype, clientScriptName))
            {
                System.Text.StringBuilder clientScriptString = new StringBuilder();
                clientScriptString.Append(myScript);
                cs.RegisterClientScriptBlock(cstype, clientScriptName, clientScriptString.ToString());
            }
        }
    }
}