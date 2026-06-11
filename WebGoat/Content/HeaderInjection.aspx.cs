using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;

using System.Security.Cryptography;
using System.Text;

namespace OWASP.WebGoat.NET
{
    public partial class HeaderInjection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Cookie"] != null)
            {
                string cookieInput = Request.QueryString["Cookie"];
                // Optional: Add input validation (e.g., regex or length checks) here
                // Example: if (!Regex.IsMatch(cookieInput, "^[a-zA-Z0-9]+$")) { /* handle error */ }
                HttpCookie cookie = new HttpCookie("UserAddedCookie");
                cookie.Value = SignCookieValue(cookieInput);
                cookie.HttpOnly = true;
                cookie.Secure = true;  // Always set the cookie as secure
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

        private string SignCookieValue(string value) {
            // TODO: Replace 'YourSecretKey' with a secure key fetched from configuration
            string secretKey = "YourSecretKey";
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey))) {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
                string signature = BitConverter.ToString(hash).Replace("-", "");
                return value + "|" + signature;
            }
        }

    }
}
