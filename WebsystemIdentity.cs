using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Diagnostics;
using System.Data.Entity.Core.Objects;
using System.Web.UI;

/// <summary>
/// třída obsluhující nový systém ASP.NET Identity
/// umožňuje zakládání a mazání rolí, popř. jejich přiřazování uživatelům
/// </summary>


namespace WebSystem
{
    public class Identity
    {
        public Identity()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string GetCurrentUserIdentityId()
        {
            //funkce vraci unikatni ID prave prihlaseneho uzivatele
            var page = HttpContext.Current.Handler as Page;

            string retVal = null;
            if (page.User.Identity.IsAuthenticated)
            {
                string userName = page.User.Identity.GetUserName();
                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
                var userid = userManager.FindByName(userName);
                retVal = userid.Id;
            }
            return retVal;
        }

        public static bool ChangeUserIdentityPassword(string userName,string newPassword)
        {
            //funkce mění heslo právě přihlášeného uživatele
            bool retVal = false;
            var page = HttpContext.Current.Handler as Page;


                //jmeno prihlaseneho uzivatele
                //string userName = page.User.Identity.GetUserName();

                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = userManager.FindByName(userName);
                

                string hashedNewPassword = userManager.PasswordHasher.HashPassword(newPassword);

                string userId = page.User.Identity.GetUserId();

                //var cUser = userStore.FindByIdAsync(userId);
                System.Threading.Tasks.Task success = userStore.SetPasswordHashAsync(user, hashedNewPassword);

                userStore.UpdateAsync(user);
                retVal = true;

                //Debug.WriteLine(success.Exception.Message);
 
            return retVal;
        }

        public static string GetCurrentUserIdentityName()
        {
            //funkce vraci jmeno prave prihlaseneho uzivatele

            var page = HttpContext.Current.Handler as Page;

            string retVal = null;
            if (page.User.Identity.IsAuthenticated)
            {
                retVal = page.User.Identity.GetUserName();
            }
            return retVal;
        }


        public static string GetIdentityIdByName(string userName)
        {
            //funkce vraci jmeno prave prihlaseneho uzivatele

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            string id = manager.FindByName(userName).Id;

            return id;
        }


        public static bool CreateIdentityUser(string userName, string password)
        {
            //funkce tvoří nového uživatele v systému AspNetIdentity
            bool retVal = false;

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            //pokus o vytvoreni noveho uzivatele
            var user = new IdentityUser() { UserName = userName };

            //vysledek pokusu o vytvoreni noveho uzivatele
            IdentityResult result = manager.Create(user, password);

            if (result.Succeeded)
            {
                Debug.WriteLine(String.Format("User {0} has been successfully registered!", user.UserName));
                retVal = true;
                //login new user
                //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                //var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                //authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
            }
            else
            {
                Debug.WriteLine(String.Format("Creation of new user {0} failed with error:" +  result.Errors.FirstOrDefault(), user.UserName));
                retVal = false;
            }

            return retVal;
        }

        public static bool CreateIdentityRole(string roleName)
        {
            //funkce tvoří novou roli v systému AspNetIdentity
            bool retVal = false;


            var roleStore = new RoleStore<IdentityRole>();
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            //zjistujeme, jestli role existuje
            if (!roleManager.RoleExists(roleName))
            {
                //tvorime roli
                var newRole = new IdentityRole() { Name = roleName };

                IdentityResult result = roleManager.Create(newRole);

                if (result.Succeeded)
                {
                    Debug.WriteLine("Websystem.Identity.CreateIdentityRole: Nová role vytvořena ok");
                    retVal = true;
                }
                else
                {
                    Debug.WriteLine("Websystem.Identity.CreateIdentityRole: Nová role nebyla vytvořena. Chyba je: " + result.Errors.FirstOrDefault());
                    retVal = false;
                }
            }
            else
            //role jiz existuje
            {
                Debug.WriteLine("Websystem.Identity.CreateIdentityRole: Tato role jiz existuje:" + roleName);
                retVal = false;
            }


            return retVal;

        }

        public static bool AssignIdentityRole(string userName, string roleName)
        {
            //funkce přiřazuje roli uživateli
            bool retVal = false;

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var usr = manager.FindByName(userName);

            IdentityResult result = manager.AddToRole(usr.Id, roleName);

            if (result.Succeeded)
            {
                Debug.WriteLine("Websystem.Identity.AssignIdentityRole: Role "+ roleName +" prirazena uzivateli " + userName);
                retVal = true;
            }
            else
            {
                Debug.WriteLine("Websystem.Identity.AssignIdentityRole: Role nebyla prirazena: " + result.Errors.FirstOrDefault());
                retVal = false;
            }

            return retVal;

        }

        public static bool RemoveIdentityRole(string userName, string roleName)
        {
            //funkce odebírá uživateli roli
            bool retVal = false;

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            var usr = manager.FindByName(userName);
            IdentityResult result = manager.RemoveFromRole(usr.Id, roleName);

            if (result.Succeeded)
            {
                Debug.WriteLine("Websystem.Identity.RemoveIdentityRole: Role " + roleName + " odebrana uzivateli " + userName);
                retVal = true;
            }
            else
            {
                Debug.WriteLine("Websystem.Identity.RemoveIdentityRole: Role nebyla odebrána: " + result.Errors.FirstOrDefault());
                retVal = false;
            }
            return retVal;

        }


        public static bool DeleteIdentityRole(string roleName)
        {
            //funkce maže roli z tabulky rolí
            bool retVal = false;
            var roleStore = new RoleStore<IdentityRole>();
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (roleManager.RoleExists(roleName))
            {

                var role = roleManager.FindByName(roleName);
                IdentityResult result = roleManager.Delete(role);


                if (result.Succeeded)
                {
                    Debug.WriteLine("Websystem.Identity.DeleteIdentityRole: Role " + roleName + " smazána ok");
                    retVal = true;
                }
                else
                {
                    Debug.WriteLine("Websystem.Identity.DeleteIdentityRole: Role " + roleName + " nebyla smazána. Chyba je: " + result.Errors.FirstOrDefault());
                    retVal = false;
                }
            }

            return retVal;
        }

        public static bool DeleteIdentityUser(string userName)
        {
            //funkce ma6e uživatele v systému AspNetIdentity
            bool retVal = false;

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var usr = manager.FindByName(userName);

            if (usr==null)
            {
                Debug.WriteLine("Websystem.Identity.DeleteIdentityUser: Uživatel " + userName + " neexistuje.");
                return false;
            }

            IdentityResult result = manager.Delete(usr);

            if (result.Succeeded)
            {
                Debug.WriteLine("Websystem.Identity.DeleteIdentityUser: Uživatel " + userName + " smazán ok");
                retVal = true;
            }
            else
            {
                Debug.WriteLine("Websystem.Identity.DeleteIdentityUser: Uživatel " + userName + " nebyl smazán. Chyba je: " + result.Errors.FirstOrDefault());
                retVal = false;
            }

            return retVal;
        }

        public static bool SignInIdentityUser(string userName, string password)
        {
            //funkce přihlašuje uživatele do systému AspNetIdentity
            bool retVal = false;

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            //search for user
            var user = userManager.Find(userName, password);

            //if usr exists
            if (user != null)
            {
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                //sign in
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

                retVal = true;

            }
            //if user does not exists
            else
            {
                Debug.WriteLine("Websystem.Identity.SignInIdentityUser:Invalid login or password");
                retVal = false;
            }


            return retVal;
        }

        public static void SignOutIdentityUser()
        {
            //funkce odhlašuje uživatele do systému AspNetIdentity
            var authenticationmanager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationmanager.SignOut();
        }

        public static string[] GetIdentityRoles()
        {
            //funkce vrací seznam všech rolí v systému AspNet Identity
            var roleStore = new RoleStore<IdentityRole>();
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var allRoles = roleManager.Roles.ToArray();//.ToList();

            int len = allRoles.Length;

            string[] roles = new string[len];

            for (int i = 0; i < len; i++)
            {
                Debug.WriteLine(allRoles[i].Name);
                roles[i] = allRoles[i].Name;
            }
            return roles;
        }

        public static bool IsUserInRole(string userName, string roleName)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var identity = userManager.FindByName(userName);

            bool hasRole = userManager.IsInRole(identity.Id, roleName);// (userName, roleName);

            if (hasRole)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetCurrentUser()
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var usr = userManager.FindById(userId);
            if (usr==null)
            {
                return "";
            }
            else
            {
                return usr.UserName;
            }
            
        }

        public static string[] GetAllUsers()
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            int counter = 0;
            var lst = userManager.Users.ToList();

            string[] output = new string[lst.Count];

            foreach (var usr in lst)
            {
                output[counter] = usr.UserName.ToString();
                //Debug.WriteLine("User name: " + usr.UserName);
                counter++;

            }
            return output;
        }

        public static string[] GetUserRoles(string UserName)
        {
            //funkce vraci vsechny role uzivatele
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var userid = userManager.FindByName(UserName);
            var roles = userManager.GetRoles(userid.Id);
            int counter = 0;

            string[] output = new string[roles.Count];


            foreach (var role in roles)
            {
                //Debug.WriteLine("User " + UserName + " has role " + role.ToString());
                output[counter] = role.ToString();
                    counter++;
            }

            return output;
        }

    }
}