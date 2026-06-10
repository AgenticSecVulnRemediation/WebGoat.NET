using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using System.Security.Cryptography;
using System.Text;

namespace OWASP.WebGoat.NET
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private IDbProvider du = Settings.CurrentDbProvider;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PanelForgotPasswordStep2.Visible = false;
                PanelForgotPasswordStep3.Visible = false;
            }
        }

        protected void ButtonCheckEmail_Click(object sender, EventArgs e)
        {
            string[] result = du.GetSecurityQuestionAndAnswer(txtEmail.Text);
            
            if (string.IsNullOrEmpty(result[0]))
            {
                labelQuestion.Text = "That email address was not found in our database!";
                PanelForgotPasswordStep2.Visible = false;
                PanelForgotPasswordStep3.Visible = false;
                
                return;
            }    
            labelQuestion.Text = "Here is the question we have on file for you: <strong>" + result[0] + "</strong>";
            PanelForgotPasswordStep2.Visible = true;
            PanelForgotPasswordStep3.Visible = false;
            
                   
            // Create a secure cookie with HMAC signature
            HttpCookie cookie = new HttpCookie("encr_sec_qu_ans");
            // Encode the security answer twice
            string encodedValue = Encoder.Encode(Encoder.Encode(result[1]));
            // Retrieve secret key from configuration (TODO: Replace with secure key retrieval)
            string secretKey = "PLACEHOLDER_SECRET";
            // Compute HMAC-SHA256 signature on the cookie data
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(encodedValue)));
                // Concatenate the encoded value and signature using "::" delimiter
                cookie.Value = encodedValue + "::" + signature;
            }
            // Set secure cookie attributes
            cookie.HttpOnly = true;
            cookie.Secure = true;
            cookie.SameSite = System.Web.SameSiteMode.Strict;
            // NOTE: On subsequent retrieval, compute HMAC of the data and compare with appended signature
            Response.Cookies.Add(cookie); 
        }

        protected void ButtonRecoverPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the cookie containing the secure question answer and validate its HMAC signature
                HttpCookie cookie = Request.Cookies["encr_sec_qu_ans"];
                if (cookie == null)
                {
                    throw new Exception("Security cookie not found.");
                }
                string cookieValue = cookie.Value;
                // Expected format: encodedData::signature
                string[] parts = cookieValue.Split(new string[] { "::" }, StringSplitOptions.None);
                if (parts.Length != 2)
                {
                    throw new Exception("Security cookie format invalid.");
                }
                string encodedData = parts[0];
                string sentSignature = parts[1];
                // Retrieve secret key from configuration (TODO: Replace with secure key retrieval)
                string secretKey = "PLACEHOLDER_SECRET";
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                {
                    string expectedSignature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(encodedData)));
                    if (!string.Equals(sentSignature, expectedSignature, StringComparison.Ordinal))
                    {
                        throw new Exception("Security cookie signature validation failed.");
                    }
                }
                // Decode the data (twice) after successful validation
                string security_answer = Encoder.Decode(Encoder.Decode(encodedData));
                
                if (security_answer.Trim().ToLower().Equals(txtAnswer.Text.Trim().ToLower()))
                {
                    PanelForgotPasswordStep1.Visible = false;
                    PanelForgotPasswordStep2.Visible = false;
                    PanelForgotPasswordStep3.Visible = true;
                    labelPassword.Text = "Security Question Challenge Successfully Completed! <br/>Your password is: " + getPassword(txtEmail.Text);
                }
            }
            catch (Exception ex)
            {
                labelMessage.Text = "An unknown error occurred - Do you have cookies turned on? Further Details: " + ex.Message;
            }
        }

        string getPassword(string email)
        {
            string password = du.GetPasswordByEmail(email);
            return password;
        }

    }
}
