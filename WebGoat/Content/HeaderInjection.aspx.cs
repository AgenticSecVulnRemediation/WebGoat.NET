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
                string cookieInput = Request.QueryString["Cookie"];
                if (Regex.IsMatch(cookieInput, "^[A-Za-z0-9]{1,50}$"))
                {
                    string signedValue = SignCookieValue(cookieInput);
                    HttpCookie cookie = new HttpCookie("UserAddedCookie", signedValue);
                    cookie.HttpOnly = true;
                    cookie.Secure = true;
                    cookie.SameSite = SameSiteMode.Strict;
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("Invalid cookie value received.");
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

        private string SignCookieValue(string value)
        {
            string secretKey = "ReplaceWithYourSecretKey"; // TODO: Retrieve secret key from secure configuration
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
                string hash = Convert.ToBase64String(hashBytes);
                return value + "|" + hash;
            }
        }

    }
}
