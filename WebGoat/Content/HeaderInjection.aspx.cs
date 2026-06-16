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
            // Check if the 'Cookie' query parameter is present and valid
            if (Request.QueryString["Cookie"] != null)
            {
                string rawCookieValue = Request.QueryString["Cookie"];
                // TODO: Replace the following validation with appropriate logic (e.g. regex or business rules) for acceptable cookie values
                if (!System.Text.RegularExpressions.Regex.IsMatch(rawCookieValue, "^[a-zA-Z0-9]*$")) {
                    // Optionally log the incident and set a safe default value
                    rawCookieValue = "defaultSafeValue"; // OR consider rejecting the request
                }

                HttpCookie cookie = new HttpCookie("UserAddedCookie");
                cookie.Value = rawCookieValue;
                // Set secure flags to reduce risk
                cookie.HttpOnly = true;  // Prevent access from client-side scripts
                // If the application uses HTTPS, uncomment the following line
                // cookie.Secure = true; 
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
