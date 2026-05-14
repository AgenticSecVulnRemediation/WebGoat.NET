using System;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET.App_Code.DB;
using OWASP.WebGoat.NET.App_Code;

using System.Security.Cryptography;
using System.Text;

namespace OWASP.WebGoat.NET
{
	public partial class Default : System.Web.UI.Page
	{
        private IDbProvider du = Settings.CurrentDbProvider;
        
        protected void ButtonProceed_Click(object sender, EventArgs e)
        {
            Response.Redirect("RebuildDatabase.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //do a quick test.  If the database connects, inform the user the database seems to be working.
            if (du.TestConnection())
            {
                lblOutput.Text = string.Format("You appear to be connected to a valid {0} provider. " +
                                               "If you want to reconfigure or rebuild the database, click on the button below!", du.Name);
                Session["DBConfigured"] = true;

                //Info leak
                HttpCookie cookie = new HttpCookie("Server", Encoder.Encode(Server.MachineName));
                // Set secure attributes to mitigate insecure cookie vulnerabilities
                cookie.HttpOnly = true; // Prevents client-side scripts from accessing the cookie
                cookie.Secure = true;   // Ensures the cookie is only sent over HTTPS. Remove if HTTPS is not available, but strongly recommended.
                // Optionally sign the cookie value to ensure integrity using an HMAC signature.
                // TODO: Replace 'CHANGE_ME' with an appropriate secret key as per your security policies.
                string secretKey = "CHANGE_ME";
                byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
                using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
                {
                    // Compute HMAC signature for the cookie value
                    byte[] valueBytes = Encoding.UTF8.GetBytes(Encoder.Encode(Server.MachineName));
                    byte[] signatureBytes = hmac.ComputeHash(valueBytes);
                    string signature = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
                    cookie.Value += "|" + signature;
                }
                Response.Cookies.Add(cookie);
            }
            else
            {
                lblOutput.Text = "Before proceeding, please ensure this instance of WebGoat.NET can connect to the database!";
            }

            // Write viewState to Screen 
            ViewState["Session"] = Session.SessionID;
        }
    }
}

