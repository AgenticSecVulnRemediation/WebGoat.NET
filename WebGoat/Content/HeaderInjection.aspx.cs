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
                string rawCookieValue = Request.QueryString["Cookie"];
                // Validate the cookie value. Replace 'IsValidCookieValue' with actual validation logic.
                if (IsValidCookieValue(rawCookieValue))
                { 
                    HttpCookie cookie = new HttpCookie("UserAddedCookie");
                    // Optionally sign the cookie value to enforce integrity
                    // cookie.Value = SignCookieValue(rawCookieValue);  // Uncomment and implement if HMAC signing is required
                    cookie.Value = rawCookieValue;
                    
                    // Set security attributes
                    cookie.HttpOnly = true;  // Prevents access via JavaScript
                    cookie.Secure = true;    // Ensures transmission over HTTPS
                    
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    // Handle invalid cookie value appropriately (e.g., logging, error response)
                }
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
