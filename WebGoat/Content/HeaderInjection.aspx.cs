using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

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
                // Validate cookie value. This regex is a placeholder; replace with production-grade validation or a proper signing mechanism for cookies.
                if (!Regex.IsMatch(cookie.Value, "^[a-zA-Z0-9]*$"))
                {
                    // If validation fails, reset the cookie value (or handle accordingly)
                    cookie.Value = "";
                }
                cookie.HttpOnly = true;
                cookie.Secure = true; // Ensure this aligns with the deployment environment using HTTPS

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
