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
                HttpCookie cookie = new HttpCookie("UserAddedCookie");
                cookie.Value = GenerateSignedCookieValue(Request.QueryString["Cookie"]);
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

        // Helper method to generate a signed cookie value.
        // TODO: Replace the secret key with an appropriate production value.
        private string GenerateSignedCookieValue(string input)
        {
            string secretKey = "ReplaceWithYourSecretKey";
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                string signature = Convert.ToBase64String(hashBytes);
                return input + "|sig=" + signature;
            }
        }

    }
}
