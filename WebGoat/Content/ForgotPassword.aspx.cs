using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;

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
            
                   
            HttpCookie cookie = new HttpCookie("encr_sec_qu_ans");
            cookie.HttpOnly = true;
            cookie.Secure = true;
            string encodedValue = Encoder.Encode(Encoder.Encode(result[1]));
            string secretKey = ConfigurationManager.AppSettings["CookieSecretKey"]; // TODO: Replace with proper key management solution
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(encodedValue));
                string signature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                cookie.Value = encodedValue + ":" + signature;
            }
            Response.Cookies.Add(cookie); 
        }

        protected void ButtonRecoverPassword_Click(object sender, EventArgs e)
        {
            try
            {
                //get the security question answer from the cookie with integrity verification
                HttpCookie cookie = Request.Cookies["encr_sec_qu_ans"];
                if (cookie == null) { throw new Exception("Missing cookie."); }
                string cookieVal = cookie.Value;
                string[] parts = cookieVal.Split(':');
                if (parts.Length != 2) { throw new Exception("Invalid cookie format."); }
                string encodedValue = parts[0];
                string signature = parts[1];
                string secretKey = ConfigurationManager.AppSettings["CookieSecretKey"]; // TODO: Replace with proper key management solution
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                {
                    byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(encodedValue));
                    string computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    if (computedSignature != signature)
                    {
                        // Integrity check failed, clear cookie and throw error
                        Response.Cookies.Remove("encr_sec_qu_ans");
                        throw new Exception("Cookie integrity check failed.");
                    }
                }
                string security_answer = Encoder.Decode(Encoder.Decode(encodedValue));
                
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
