using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;//SQL
using System.Net.Mail;//for email sending
using System.Security.Cryptography;
namespace LinqExample
{
    public partial class StartScreen : Form
    {
        static RTDataGenerator x;//The second thrad that create the simulated data
       
       
        //connect to SQL
        SqlConnection con = new SqlConnection();
        SqlCommand cmd=new SqlCommand();
        SqlDataReader dr;
        static int flag = 0;
        static string  UserPass = "";
        public StartScreen()
        {
            InitializeComponent(); //Initializes this form

    
            cmbUserType.MaxLength = 20;
            txtPassword.PasswordChar='*';//show * insted of clear password
            txtPassword.MaxLength = 20;
       
            InitCmb();//Intial function 
        }
        //Load data from database for user 
        private void loaddata()
        {
            cmd.CommandText = "SELECT username FROM users";
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows) {
                while (dr.Read())
                {
                    cmbUserType.Items.Add(dr[0].ToString());
                    
                }
            }
            dr.Close();
            con.Close();
            if (cmbUserType.Items.Count > 0)
            {
                cmbUserType.SelectedIndex = 0;//Take the first value and show on the dropdownlist
                
            }
           
        }

        private void InitCmb()
        {
           con.ConnectionString = "Data Source=SHELLEE07YANIV\\SQLEXPRESS;Initial Catalog=SLA_RT_monitoring;Integrated Security=True";
          // con.ConnectionString = "Data Source=THE_RAIN;Initial Catalog=SLA_RT_monitoring;Integrated Security=True";
            
            cmd.Connection = con;
            loaddata();
           
        }
        
        private void btnEnter_Click(object sender, EventArgs e)
        {
            //Password Validation
          
            
            if (flag == 0)
            {
                cmd.Parameters.AddWithValue("@username", cmbUserType.SelectedItem.ToString());
                cmd.CommandText = "SELECT password FROM users WHERE username = @username";
                con.Open();
                dr = cmd.ExecuteReader();
               
                if (dr.Read())
                    UserPass = dr.GetString(0);

                UserPass = UserPass.Replace(" ", String.Empty);//remove whitespaces
               
               
                    
            }
            flag = 1;
            bool result = string.Equals(txtPassword.Text, UserPass);
            if (result)
            {//Password is OK

                con.Close();//close the connection 
                MainMenu menu = new MainMenu();
                menu.FormClosed += new FormClosedEventHandler(child_FormClosed);  //add handler to catch when child form is closed

                menu.Show(); //show child
                this.Hide(); //hide parent

                x = new RTDataGenerator();
                x.Start();

            }
            else
            {
                MessageBox.Show("Wrong Password", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text ="";
                txtPassword.Focus();//Set the focus to the password field for second chance.
                con.Close();//close the connection
               
               
            
            }
  
            
        }
        void child_FormClosed(object sender, FormClosedEventArgs e)//handles the forms
        {
            //when child form is closed, the parent reappears
            this.Close();
        }

        private void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1_Info.ToolTipTitle =("Password");
            this.toolTip1_Info.SetToolTip(this.txtPassword, "Maximum 20 Characters");
          
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnExit_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1_Info.ToolTipTitle = "Exit";
            this.toolTip1_Info.SetToolTip(this.btnExit, "Exit Program");

        }
        //Send email if user forget his password
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cmd.Parameters.Clear();
            linkLabel1.Enabled= false;//disable the link until the email sent
            var email="";
            string password = "";
            string user = cmbUserType.SelectedItem.ToString().Replace(" ", String.Empty);//remove whitespaces  
        
            cmd.Parameters.AddWithValue("@username", cmbUserType.SelectedItem.ToString());
            cmd.CommandText = "SELECT email_address , password,id FROM users WHERE username = @username";
            con.Open();
            dr = cmd.ExecuteReader();

            if (dr.Read() && !dr.IsDBNull(0))//handle users without email
                {
                    email = dr.GetString(0);
                    password = dr.GetString(1);
                    password = password.Replace(" ", String.Empty);//remove whitespaces
                }
                else
                {
                    MessageBox.Show("No email configured for user" + " " + user + " Contact ynsk4@hotmail.com" );
                    linkLabel1.Enabled = true;
                    cmd.Parameters.RemoveAt(0);
                    con.Close();
                    return;
                }
                 
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("ynsk44@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Forget my password sent from SLA Real Time Monitoring" ;
                mail.Body = "The password for user" + " " + user + " is " + password;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ynsk44@gmail.com", "shellee1!");
                SmtpServer.EnableSsl = true;
                
                SmtpServer.Send(mail);

                MessageBox.Show("Email with password Send to user" + " " + user + " at " + email);
                linkLabel1.Enabled = true;
                txtPassword.Focus();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Parameters.RemoveAt(0);
            con.Close();//close the connection 

        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        
            private void toolTip1_Info_Popup(object sender, PopupEventArgs e)
            {

            }

            private void StartScreen_Load(object sender, EventArgs e)
            {
           


            }

            private void txtPassword_TextChanged(object sender, EventArgs e)
            {

            }

           
   
     
      
        }
}
