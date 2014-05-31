using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW2
{
    public partial class Form1 : Form
    {
        //class Customer
        //{
        //    public Customer(String name, DateTime birth_date)
        //    {
        //        this.name = name;
        //        this.birth_date = birth_date;
        //    }
        //    public String name;
        //    public DateTime birth_date;
        //}

        //class Product
        //{
        //    public Product(String name, int price)
        //    {
        //        this.name = name;
        //        this.price = price;
        //    }

        //    public String name;
        //    public int price;
        //}


        Customer[] arrCustomers = new Customer[6];
        Product[] arrProducts = new Product[6];



        DBDataContext dbContext = new DBDataContext();

        public Form1()
        {
            InitializeComponent();

            Customer c;
            Product p;
            
            c = new Customer();
            c.name = "Elvis";
            c.birthdate = new DateTime(1940, 1, 1);
            arrCustomers[0] = c;

            c = new Customer();
            c.name = "Jony";
            c.birthdate = new DateTime(1950, 1, 1);
            arrCustomers[1] = c;

            c = new Customer();
            c.name = "Tami";
            c.birthdate = new DateTime(2000, 9, 9);
            arrCustomers[2] = c;

            c = new Customer();
            c.name = "Daniel";
            c.birthdate = new DateTime(1990, 4, 4);
            arrCustomers[3] = c;

            c = new Customer();
            c.name = "Gooliver";
            c.birthdate = new DateTime(1850, 2, 2);
            arrCustomers[4] = c;

            c = new Customer();
            c.name = "Cleopatra";
            c.birthdate = new DateTime(1790, 5, 5);
            arrCustomers[5] = c;


            p = new Product();
            p.name = "Violin";
            p.price = 2000;
            arrProducts[0] = p;

            p = new Product();
            p.name = "Cello";
            p.price = 5000;
            arrProducts[1] = p;

            p = new Product();
            p.name = "Drums";
            p.price = 1400;
            arrProducts[2] = p;

            p = new Product();
            p.name = "Trombon";
            p.price = 9000;
            arrProducts[3] = p;

            p = new Product();
            p.name = "Piano";
            p.price = 8000;
            arrProducts[4] = p;

            p = new Product();
            p.name = "Guitar";
            p.price = 9000;
            arrProducts[5] = p;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClearDB();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Thread t1 = new Thread(new ThreadStart(GenerageCustomers));

            t1.Start();

        }

        void ClearDB()
        {
            
            SqlConnection con = new SqlConnection(dbContext.Connection.ConnectionString);
            con.Open();
            try
            {

                SqlCommand cmd3 = new SqlCommand("DELETE FROM Transactions;", con);
                cmd3.ExecuteNonQuery();
                cmd3.Dispose();

                SqlCommand cmdCustomer = new SqlCommand("DELETE FROM Customers;", con);
                cmdCustomer.ExecuteNonQuery();
                cmdCustomer.Dispose();

                SqlCommand cmd2 = new SqlCommand("DELETE FROM Products;", con);
                cmd2.ExecuteNonQuery();
                cmd2.Dispose();

                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            con.Close();

        }

        void GenerageCustomers()
        {
            try
            {
                for (int i = 0; i < 60; ++i)
                {
                    this.Invoke((MethodInvoker)delegate
                    {

                        SqlConnection con = new SqlConnection(dbContext.Connection.ConnectionString);
                        con.Open();

                        SqlCommand cmdCustomer = new SqlCommand("Insert into Customers( name, birthdate) values( @name, @birthdate)");
                        cmdCustomer.Parameters.Add("@name", SqlDbType.VarChar, 50);
                        cmdCustomer.Parameters.Add("@birthdate", SqlDbType.DateTime);
                        cmdCustomer.Connection = con;


                        SqlCommand cmdProduct = new SqlCommand("Insert into Products(name, price) values( @name, @price)");
                        cmdProduct.Parameters.Add("@name", SqlDbType.VarChar, 50);
                        cmdProduct.Parameters.Add("@price", SqlDbType.Int);
                        cmdProduct.Connection = con;


                        cmdCustomer.Parameters["@name"].Value = arrCustomers[i % 6].name;
                        cmdCustomer.Parameters["@birthdate"].Value = arrCustomers[i % 6].birthdate;

                        cmdCustomer.ExecuteNonQuery();

                        cmdProduct.Parameters["@name"].Value = arrProducts[i % 6].name;
                        cmdProduct.Parameters["@price"].Value = arrProducts[i % 6].price;
                        cmdProduct.ExecuteNonQuery();


                        con.Close();


                        bindingSource1.DataSource = dbContext.Customers.Select(item => item);
                        bindingSource2.DataSource = dbContext.Products.Select(item => item);



                        bindingNavigator1.Refresh();
                        dataGridView1.Refresh();
                        dataGridView2.Refresh();
                        bindingNavigator2.Refresh();

                    });
                }

                {
                    SqlConnection con = new SqlConnection(dbContext.Connection.ConnectionString);
                    con.Open();

                    SqlCommand cmdTransaction = new SqlCommand("Insert into Transactions(idCustomer, idProduct) values(@idCust, @idProd)");
                    cmdTransaction.Parameters.Add("@idCust", SqlDbType.Int);
                    cmdTransaction.Parameters.Add("@idProd", SqlDbType.Int);
                    cmdTransaction.Connection = con;

                    for (int i = 0; i < 9; ++i)
                    {
                        cmdTransaction.Parameters["@idCust"].Value = Int32.Parse(dataGridView1.Rows[i % 6].Cells[0].Value.ToString());
                        cmdTransaction.Parameters["@idProd"].Value = Int32.Parse(dataGridView2.Rows[i % 4].Cells[0].Value.ToString());
                        cmdTransaction.ExecuteNonQuery();

                        
                    }

                    con.Close();

                    this.Invoke((MethodInvoker)delegate
                    {
                        bindingSource3.DataSource = dbContext.Transactions.Select(item => item);
                        bindingNavigator3.Refresh();
                        dataGridView3.Refresh();
                    });
                }



            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

             

            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }


        



    }
}
