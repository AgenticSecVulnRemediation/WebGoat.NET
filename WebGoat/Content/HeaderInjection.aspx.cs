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
                HttpCookie cookie = new HttpCookie("UserAddedCookie");
                cookie.Value = Request.QueryString["Cookie"]; 
                // TODO: Validate the cookie value to ensure it contains only expected characters
                // Example: if(!Regex.IsMatch(cookie.Value, "^[a-zA-Z0-9]+$")) { /* handle error */ }
                cookie.HttpOnly = true;  // Prevent client-side scripts from accessing the cookie
                cookie.Secure = true;    // Ensure the cookie is sent only over HTTPS
                // TODO: Validate the cookie value to ensure it contains only expected characters
                if(!System.Text.RegularExpressions.Regex.IsMatch(cookie.Value, "^[a-zA-Z0-9]+$"))
                {
                    // Handle error: cookie value contains invalid characters
                    cookie.Value = "default"; // or take appropriate action
                }
                cookie.HttpOnly = true;  // Prevent client-side scripts from accessing the cookie
                cookie.Secure = true;    // Ensure the cookie is sent only over HTTPS
                // TODO: Optionally, sign the cookie value for integrity check.
                // Example: using System.Web.Security;
                // cookie.Value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, cookie.Value, DateTime.Now, DateTime.Now.AddMinutes(30), false, "replace_with_secret"));

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
