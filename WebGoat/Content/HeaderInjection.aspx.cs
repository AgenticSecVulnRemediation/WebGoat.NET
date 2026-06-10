using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;

using System.Security.Cryptography;

namespace OWASP.WebGoat.NET
{
    public partial class HeaderInjection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Cookie"] != null)
            {
                HttpCookie cookie = new HttpCookie("UserAddedCookie");
                string cookieInput = Request.QueryString["Cookie"];
                // Basic validation: allow only alphanumeric characters and length 1 to 50.
                if (!System.Text.RegularExpressions.Regex.IsMatch(cookieInput, "^[a-zA-Z0-9]{1,50}$"))
                {
                    // If validation fails, assign a default safe value
                    cookieInput = "default";
                }
                // TODO: Replace with actual signing logic or call to the security helper method.
                string signedCookieValue = cookieInput; // Placeholder for signed value.
                cookie.Value = signedCookieValue;
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
