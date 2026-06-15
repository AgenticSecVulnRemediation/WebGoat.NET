using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
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
                string validatedCookieValue;
                if (IsValidCookieValue(rawCookieValue, out validatedCookieValue))
                {
                    string signedCookie = SignCookieValue(validatedCookieValue);
                    HttpCookie cookie = new HttpCookie("UserAddedCookie");
                    cookie.Value = signedCookie; // The value in the format: <validated>|<HMAC signature>
                    cookie.HttpOnly = true; // Mitigates client side script access
                    cookie.Secure = true;   // Ensures the cookie is only sent over HTTPS
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    // Optionally log invalid cookie input and do not set cookie
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

        public bool IsValidCookieValue(string input, out string validatedValue) {
            string pattern = "^[a-zA-Z0-9_-]+$";
            if (!String.IsNullOrEmpty(input) && Regex.IsMatch(input, pattern)) {
                validatedValue = input;
                return true;
            }
            validatedValue = null;
            return false;
        }

        public string SignCookieValue(string validatedValue) {
            string secretKey = "YourSecretKeyHere";
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey))) {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(validatedValue));
                string signature = Convert.ToBase64String(hashValue);
                return validatedValue + "|" + signature;
            }
        }

    }
}
