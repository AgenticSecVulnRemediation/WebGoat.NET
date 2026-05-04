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
                if (!string.IsNullOrEmpty(cookieInput) && IsValidCookieValue(cookieInput)) { // TODO: Implement IsValidCookieValue to validate the input
                    HttpCookie cookie = new HttpCookie("UserAddedCookie");
                    cookie.Value = cookieInput;
                    cookie.HttpOnly = true; // Prevent access from client-side scripts
                    cookie.Secure = true;   // Ensure cookie is sent over HTTPS only
                    // Optional: Implement signing mechanism to ensure integrity:
                    // cookie.Value = SignCookieValue(cookie.Value, "<SecretKey>"); // Replace <SecretKey> with application-specific key
                    Response.Cookies.Add(cookie);
                } else {
                    // Handle invalid cookie input appropriately
                    // e.g., log the incident or set a default safe value
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
