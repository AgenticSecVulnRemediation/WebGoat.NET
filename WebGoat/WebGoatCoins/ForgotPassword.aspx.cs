using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.WebGoatCoins
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
    
        private IDbProvider du = Settings.CurrentDbProvider;
        // Secret key for HMAC signing of cookies. Replace the placeholder value with a secure key.
        private static readonly string secretKey = "ReplaceThisWithASecretKey";
        
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

            //encode twice for more security!

            cookie.Value = Encoder.Encode(Encoder.Encode(result[1]));
            // Compute HMAC signature for cookie integrity
            string signature = ComputeHmac(cookie.Value);
            cookie.Value = cookie.Value + "|" + signature;
            // Set cookie to be HttpOnly to prevent client-side script access
            cookie.HttpOnly = true;
            // Set cookie to be secure so that it is sent only over HTTPS
            cookie.Secure = true;
            // Optionally, set the SameSite attribute to 'Strict'
            cookie.SameSite = System.Web.SameSiteMode.Strict;

            Response.Cookies.Add(cookie);
        }

        protected void ButtonRecoverPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the security question answer from the cookie with integrity check
                string cookieValue = Request.Cookies["encr_sec_qu_ans"].Value.ToString();
                string[] parts = cookieValue.Split('|');
                if (parts.Length != 2 || ComputeHmac(parts[0]) != parts[1])
                {
                    throw new Exception("Cookie integrity check failed.");
                }
                string encrypted_password = parts[0];
                
                // Decode it (twice for extra security!)
                string security_answer = Encoder.Decode(Encoder.Decode(encrypted_password));
                
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

        protected void ButtonGoToCustomerLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerLogin.aspx");
        }

        string getPassword(string email)
        {
            string password = du.GetPasswordByEmail(email);
            return password;
        }

        // Computes HMAC signature using HMACSHA256 algorithm for cookie integrity
        private string ComputeHmac(string data)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }

    }
}
