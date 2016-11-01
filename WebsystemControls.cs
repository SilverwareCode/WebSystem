using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Seznam funkci pro praci s instancemi uzivatrelskych contrlu
/// </summary>
/// 
namespace WebSystem
{

    public class Controls
    {
        public Controls()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void populateDropDownListEx(DropDownList ddlist, string col1Name, string col2Name, string tableName, string connectionStringName, string sql)
        {
            //funkce plni zaslany dropdown list sloupcem z databaze
            string comm = sql;

            System.Data.DataTable myTable = WebSystem.Database.getDataTableFromReader(comm, connectionStringName);

            ddlist.DataSource = myTable;
            ddlist.DataTextField = col1Name;
            ddlist.DataValueField = col2Name;
            ddlist.DataBind();
        }



        public static void populateDropDownList(DropDownList ddlist, string col1Name, string col2Name, string tableName, string connectionStringName)
        {
            //funkce plni zaslany dropdown list sloupcem z databaze
            string comm = "SELECT " + col1Name + "," + col2Name + " FROM " + tableName;

            System.Data.DataTable myTable = WebSystem.Database.getDataTableFromReader(comm, connectionStringName);

            ddlist.DataSource = myTable;
            ddlist.DataTextField = col1Name;
            ddlist.DataValueField = col2Name;
            ddlist.DataBind();

        }



        //funkce nastavuje property Double uzivatelskeho controlu
        public static void setControlProperty(Control uc, string propertyName, object propValue)
        {
            //funkce nastavuje property uzivatelskeho controlu
            //ktery ne na stranku umisten dynamicky

            foreach (var prop in uc.GetType().GetProperties())
            {
                if (prop.Name == propertyName)
                {
                    uc.GetType().GetProperty(propertyName).SetValue(uc, propValue);
                }
            }
        }


        //funkce nastavuje property Double uzivatelskeho controlu
        public static void setControlPropertyDouble(Control uc, string propertyName, double propValue)
        {
            //funkce nastavuje property uzivatelskeho controlu
            //ktery ne na stranku umisten dynamicky

            foreach (var prop in uc.GetType().GetProperties())
            {
                if (prop.Name == propertyName)
                {
                    uc.GetType().GetProperty(propertyName).SetValue(uc, propValue);
                }
            }
        }

        //funkce nastavuje property INT uzivatelskeho controlu
        public static void setControlPropertyInt(Control uc, string propertyName, int propValue)
        {
            //funkce nastavuje property uzivatelskeho controlu
            //ktery ne na stranku umisten dynamicky

            foreach (var prop in uc.GetType().GetProperties())
            {
                if (prop.Name == propertyName)
                {
                    uc.GetType().GetProperty(propertyName).SetValue(uc, propValue);
                }
            }
        }


        //funkce nastavuje property STRING uzivatelskeho controlu
        public static void setControlPropertyString(Control uc, string propertyName, string propValue)
        {
            //funkce nastavuje property uzivatelskeho controlu
            //ktery ne na stranku umisten dynamicky

            foreach (var prop in uc.GetType().GetProperties())
            {
                if (prop.Name == propertyName)
                {
                    uc.GetType().GetProperty(propertyName).SetValue(uc, propValue);

                }
            }
        }


        //funkce cte property INT z property uzivatelskeho controlu
        public static int getControlPropertyInt(Control uc, string propertyName)
        {
            //funkce nastavuje property uzivatelskeho controlu
            //ktery ne na stranku umisten dynamicky

            int myValue = 0;

            foreach (var prop in uc.GetType().GetProperties())
            {
                if (prop.Name == propertyName)
                {
                    myValue = (int)uc.GetType().GetProperty(propertyName).GetValue(uc);
                    return myValue;
                }
            }

            return myValue;
        }


        //funkce cte property STRING z property uzivatelskeho controlu
        public static string getControlPropertyString(Control uc, string propertyName)
        {
            //funkce nastavuje property uzivatelskeho controlu
            //ktery ne na stranku umisten dynamicky
            string myValue = "";


            foreach (var prop in uc.GetType().GetProperties())
            {
                if (prop.Name == propertyName)
                {
                    myValue = (string)uc.GetType().GetProperty(propertyName).GetValue(uc);
                    return myValue;
                }
            }
            return myValue;
        }


        //funkce vola motody uzivatelskeho conntrolu
        public static void ExecuteControlMethod(Control mujControl, string jmenoMojiFunkce, string[] param )
        {
            //volame vnorenou funkci uzivatelskeho controlu
            foreach (var jmenoFunkce in mujControl.GetType().GetMethods())
            {
                //Debug.WriteLine(prop.Name);
                if (jmenoFunkce.Name == jmenoMojiFunkce)
                {
                    MethodInfo mInfo = mujControl.GetType().GetMethod(jmenoMojiFunkce);
                    object result = mInfo.Invoke(mujControl, param);
                    //Debug.WriteLine(result.ToString());
                }
            }
        }
    }
}
