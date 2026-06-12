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
                string userCookie = Request.QueryString["Cookie"];
                // TODO: Replace '^[a-zA-Z0-9]+$' with an appropriate pattern for allowed cookie values
                if (System.Text.RegularExpressions.Regex.IsMatch(userCookie, "^[a-zA-Z0-9]+$"))
                {
                    HttpCookie cookie = new HttpCookie("UserAddedCookie");
                    cookie.Value = userCookie;
                    cookie.HttpOnly = true;
                    cookie.Secure = true;
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    // Log or handle invalid cookie value
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
