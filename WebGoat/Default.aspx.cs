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
                // Retrieve the raw value from Server.MachineName after encoding
                string rawValue = Encoder.Encode(Server.MachineName);
                
                // Use a secret key from secure configuration (replace 'your_secret_key_here' with an actual secure key retrieval mechanism)
                string secretKey = "your_secret_key_here";  // TODO: Replace with secure key retrieval
                
                // Compute HMACSHA256 signature for cookie integrity
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey))) {
                    byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawValue));
                    string signature = Convert.ToBase64String(signatureBytes);
                    
                    // Append the signature to the raw value. Using a delimiter to separate value and signature
                    string cookieValue = rawValue + "|" + signature;

                    // Create cookie with signed value
                    HttpCookie cookie = new HttpCookie("Server", cookieValue);
                    cookie.HttpOnly = true;      // Mitigates XSS attacks
                    cookie.Secure = true;        // Ensures cookie is only sent over HTTPS
                    Response.Cookies.Add(cookie);
                }
            }
            else
            {
                lblOutput.Text = "Before proceeding, please ensure this instance of WebGoat.NET can connect to the database!";
            }

            // Write viewState to Screen 
            ViewState["Session"] = Session.SessionID;
        }

        // Helper method to verify the integrity of the 'Server' cookie
        private string VerifyServerCookie()
        {
            HttpCookie cookie = Request.Cookies["Server"];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return null;
            string[] parts = cookie.Value.Split(new char[] { '|' }, 2);
            if (parts.Length != 2)
                return null;
            string rawValue = parts[0];
            string signature = parts[1];
            string secretKey = "your_secret_key_here";  // TODO: Replace with secure key retrieval
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] expectedSignatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawValue));
                string expectedSignature = Convert.ToBase64String(expectedSignatureBytes);
                if (signature == expectedSignature)
                    return rawValue;
                else
                    return null;
            }
        }
    }
    }
}

