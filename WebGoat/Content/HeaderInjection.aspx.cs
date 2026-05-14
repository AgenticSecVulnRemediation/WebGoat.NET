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
using System.Text.RegularExpressions;

namespace OWASP.WebGoat.NET
{
    public partial class HeaderInjection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Cookie"] != null)
            {
                string cookieInput = Request.QueryString["Cookie"].Trim();
                if (Regex.IsMatch(cookieInput, "^[a-zA-Z0-9]+$"))
                {
                    string signature = GenerateCookieSignature(cookieInput, "YOUR_SECRET_KEY"); // TODO: replace YOUR_SECRET_KEY with a secure secret loaded from configuration
                    string combinedValue = cookieInput + "|" + signature;
                    HttpCookie cookie = new HttpCookie("UserAddedCookie");
                    cookie.Value = combinedValue;
                    cookie.HttpOnly = true;
                    cookie.Secure = true;
                    cookie.SameSite = System.Web.SameSiteMode.Strict;
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    // Invalid cookie input - do not add cookie
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

        private string GenerateCookieSignature(string value, string secretKey)
        {
            // TODO: Replace 'YOUR_SECRET_KEY' with a secure secret loaded from configuration
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

    }
}
