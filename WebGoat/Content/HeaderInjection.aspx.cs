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
                string userCookie = Request.QueryString["Cookie"];
                // TODO: Replace the regex below with a proper validation logic for the cookie value as per security requirements.
                if (!System.Text.RegularExpressions.Regex.IsMatch(userCookie, "^[a-zA-Z0-9]*$"))
                {
                    // If validation fails, assign a default safe value or log as needed.
                    userCookie = "defaultSafeValue";
                }
                // TODO: Optionally, sign the cookie value using cryptographic signing (e.g., HMAC) to ensure its integrity.
                HttpCookie cookie = new HttpCookie("UserAddedCookie");
                cookie.Value = userCookie;
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
