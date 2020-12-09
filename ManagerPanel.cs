using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace warehouse
{
    public partial class ManagerPanel : Form
    {
        public ManagerPanel()
        {
            InitializeComponent();
        }
        string cs = @"URI=file:C:\Users\Viper\source\repos\warehouse\bin\Debug\data.db";
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            using var con = new SQLiteConnection(cs);
            con.Open();


            using var cmd = new SQLiteCommand(con);
            string pic_name = String.Empty;
            string dateFrom = String.Empty;
            string DateTo = String.Empty;
            string weight = String.Empty;
            string phone_number = String.Empty;
            string price = String.Empty;


            cmd.CommandText = "SELECT pic_name,dateFrom,DateTo,weight,phone_number,price FROM data;";
            cmd.ExecuteNonQuery();
            SQLiteDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {


                pic_name = r["pic_name"].ToString();
                dateFrom = r["dateFrom"].ToString();
                DateTo = r["DateTo"].ToString();
                weight = r["weight"].ToString();
                phone_number = r["phone_number"].ToString();
                price = r["price"].ToString();



                dataGridView1.Rows.Add(new string[] { pic_name, dateFrom, DateTo, weight, phone_number, price });
            }
            r.Close();
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            
            
        }
    }
}
