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
                string rawCookieValue = Request.QueryString["Cookie"];
                string validatedCookieValue = ValidateCookieValue(rawCookieValue);
                if (!string.IsNullOrEmpty(validatedCookieValue)) {
                    HttpCookie cookie = new HttpCookie("UserAddedCookie");
                    cookie.Value = validatedCookieValue;
                    cookie.HttpOnly = true;
                    cookie.Secure = true;
                     Response.Cookies.Add(cookie);
                    } else {
                    // Optionally log error here
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

        private string ValidateCookieValue(string rawValue)
        {
            // Example validation: allow only alphanumeric characters
            if (!Regex.IsMatch(rawValue, "^[a-zA-Z0-9]+$"))
            {
                return string.Empty;
            }
            return rawValue;
        }
    }
}
