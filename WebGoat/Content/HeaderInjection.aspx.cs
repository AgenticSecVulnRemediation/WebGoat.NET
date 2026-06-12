using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;

namespace OWASP.WebGoat.NET
{
    public partial class HeaderInjection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Cookie"] != null)
            {
                string cookieInput = Request.QueryString["Cookie"];
                // Validate cookie value to allow only safe characters (alphanumeric)
                if (!System.Text.RegularExpressions.Regex.IsMatch(cookieInput, "^[a-zA-Z0-9]+$"))
                {
                    // If validation fails, use a default safe value or handle accordingly
                    cookieInput = "";
                }
                // Optionally sign the cookie value. Replace 'CookiePurpose' with your application-specific value.
                byte[] protectedBytes = System.Web.Security.MachineKey.Protect(System.Text.Encoding.UTF8.GetBytes(cookieInput), "CookiePurpose");
                string protectedValue = System.Convert.ToBase64String(protectedBytes);
                HttpCookie cookie = new HttpCookie("UserAddedCookie");
                cookie.Value = protectedValue;
                cookie.HttpOnly = true;
                cookie.Secure = true;
                Response.Cookies.Add(cookie);
            }
            else if (Request.QueryString["Header"] != null)
            {
                NameValueCollection newHeader = new NameValueCollection();
                newHeader.Add("newHeader", Request.QueryString["Header"]);
                Response.Headers.Add(newHeader);
            }



            //Headers
            lblHeaders.Text = Request.Headers.ToString().Replace("&", "<br />");

            //Cookies
            ArrayList colCookies = new ArrayList();
            for (int i = 0; i < Request.Cookies.Count; i++)
                colCookies.Add(Request.Cookies[i]);

            gvCookies.DataSource = colCookies;
            gvCookies.DataBind();

            //possibly going to be used later for something interesting

        }
    }
}
