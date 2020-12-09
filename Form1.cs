using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
namespace warehouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        string cs = @"URI=file:C:\Users\Viper\source\repos\warehouse\bin\Debug\data.db";


        Point stage0 = new Point(436, 442);
        Point stage1 = new Point(438, 335);
        Point stage2 = new Point(435, 217);
        Point stage3 = new Point(436, 92);



        public void Form1_Paint(object sender, PaintEventArgs e, Keys KeyData)
        {

            pictureBox1.Image = Properties.Resources.pixil_frame_0__1_;
            pictureBox2.Image = Properties.Resources.robot;



        }

        Random rnd = new Random();
        Random gen = new Random();
        int item_weight;
        int range;
        DateTime randomDate;
        DateTime ourDays;
        string phone_number;
        int price;

        public void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            item_weight = rnd.Next(1, 400);
            range = 5 * 365;
            ourDays = DateTime.Now;
            randomDate = ourDays.AddDays(gen.Next(2, range));
            phone_number = GetRandomTelNo();

            price = (int)(randomDate - ourDays).TotalDays; ;
            if (item_weight > 300)
            {
                MessageBox.Show("I can't took this weight,call human", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (item_weight < 300)
            {


                dataGridView1.Rows.Add(new string[] { ourDays.ToString(), randomDate.ToString(), item_weight.ToString(), phone_number, price.ToString() });
                //= String.Format("Saving to date {0} ,weight is {1} kg", randomDate, item_weight);
                button2.Enabled = true;
            }


        }
        static string GetRandomTelNo()
        {
            var random = new Random();

            string s = String.Empty;
            char a = (char)43;
            for (int i = 0; i < 10; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s = String.Concat(a, s);
        }

        public PictureBox getPictureBoxByName(string name)
        {
            foreach (object p in this.Controls)
            {
                if (p.GetType() == typeof(PictureBox))
                    if (((PictureBox)p).Name == name)
                        return (PictureBox)p;
            }
            return new PictureBox(); //OR return null;
        }
        string pic_number = String.Empty;
        public void button2_Click_1(object sender, EventArgs e)
        {
            button2.Enabled = false;
            dataGridView1.Rows.Clear();
            using var con = new SQLiteConnection(cs);
            con.Open();


            using var cmd = new SQLiteCommand(con);
            string line = String.Empty;

            cmd.CommandText = "SELECT pic_name, CASE WHEN is_full = 0 THEN 'EMPTY' END as 'DATA' FROM data;";
            SQLiteDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {

                pic_number = r["pic_name"].ToString();
                line = r["DATA"].ToString();

                if (line == "EMPTY")
                {
                    break;
                }

            }
            r.Close();
            temp = getPictureBoxByName(pic_number);
            if (temp.Location.X > stage1.X)
            {
                Xdirection = 1;
                Sprava = false;

            }
            else
            {
                Xdirection = -1;
                Sprava = true;
            }
            if (temp.Location.Y > stage1.Y)
            {
                height = stage1.Y;
            }
            else if (temp.Location.Y > stage2.Y && temp.Location.Y < stage1.Y)
            {
                height = stage2.Y;
            }

            else //(temp.Location.Y<stage2.Y && temp.Location.Y>stage3.Y)
            {
                height = stage3.Y;
            }





            cmd.CommandText = "UPDATE data SET is_full=1,dateFrom= :dateFrom,DateTo= :DateTo ,weight= :weight,phone_number= :phone_number,price= :price WHERE pic_name = :pic_number";


            cmd.Parameters.AddWithValue("dateFrom", ourDays.ToString());
            cmd.Parameters.AddWithValue("DateTo", randomDate.ToString());
            cmd.Parameters.AddWithValue("weight", item_weight);
            cmd.Parameters.AddWithValue("pic_number", pic_number);
            cmd.Parameters.AddWithValue("phone_number", phone_number);
            cmd.Parameters.AddWithValue("price", Math.Abs(price));

            cmd.ExecuteNonQuery();
            Task = true;
            MoveTimer.Start();

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            // using var con = new SQLiteConnection(cs);
            // con.Open();
            // using  var cmd = new SQLiteCommand(con);
            //// cmd.CommandText = "DROP TABLE IF EXISTS data";
            // //cmd.ExecuteNonQuery();
            // //cmd.CommandText = @"CREATE TABLE data(id INTEGER PRIMARY KEY, pic_name TEXT,is_full INTEGER NOT NULL DEFAULT 0 CHECK(is_full IN (0,1)),date TEXT, weight INT)";
            // //cmd.ExecuteNonQuery();


            // //var connection = new SQLiteConnection("Data Source=:memory:");
            // foreach (var pb in this.Controls.OfType<PictureBox>())
            // {
            //     //cmd.CommandText = "INSERT INTO data(pic_name,is_full) VALUES(@pic_name,@is_full)";
            //    // cmd.Parameters.AddWithValue("@pic_name", pb.Name);
            //     //cmd.Parameters.AddWithValue("@is_full", 0);
            //     //cmd.ExecuteNonQuery();
            //     

            // }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = @"UPDATE data SET is_full=0,dateFrom=NULL,DateTo=NULL,weight=NULL,phone_number=NULL,price=NULL";
            cmd.ExecuteNonQuery();
            con.Close();

            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            string weight = String.Empty;
            string dateFrom = String.Empty;
            string dateTo = String.Empty;
            string is_full = String.Empty;
            string pic_name = String.Empty;
            string phone_number = String.Empty;
            string price = String.Empty;
            cmd.CommandText = "SELECT * FROM data WHERE pic_name= :pic_name";
            cmd.Parameters.AddWithValue("pic_name", comboBox1.Text);
            SQLiteDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                pic_name = r["pic_name"].ToString();
                is_full = r["is_full"].ToString();
                dateFrom = r["dateFrom"].ToString();
                dateTo = r["DateTo"].ToString();
                weight = r["weight"].ToString();
                phone_number = r["phone_number"].ToString();
                price = r["price"].ToString();
                if (is_full == "1")
                {
                    MessageBox.Show("FULL", comboBox1.Text);
                    button4.Enabled = true;

                }
                else
                {
                    MessageBox.Show("Empty", comboBox1.Text);
                }
            }
            dataGridView1.Rows.Add(new string[] { dateFrom, dateTo, weight, phone_number, price });

            r.Close();
            con.Close();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            dataGridView1.Rows.Clear();
            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "UPDATE data SET is_full = 0, dateFrom = NULL,DateTo=NULL, weight = NULL,phone_number=NULL,price=NULL WHERE pic_name = :pic_name";
            cmd.Parameters.AddWithValue("pic_name", comboBox1.Text);
            cmd.ExecuteNonQuery();

            temp = getPictureBoxByName(comboBox1.Text);

            if (temp.Location.X > stage1.X)
            {
                Xdirection = 1;
                Sprava = false;

            }
            else
            {
                Xdirection = -1;
                Sprava = true;
            }
            if (temp.Location.Y > stage1.Y)
            {
                height = stage1.Y;
            }
            else if (temp.Location.Y > stage2.Y && temp.Location.Y < stage1.Y)
            {
                height = stage2.Y;
            }

            else
            {
                height = stage3.Y;
            }
            Task = true;


        }
        int speed = 20;
        PictureBox temp;
        bool iSOnPlace = false;
        int Xdirection = 0;
        int Ydirection = -1;
        bool Sprava = false;
        bool Task = false;
        int height = 335;
        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            if (Task)
            {
                if (Sprava)
                {
                    if (!iSOnPlace)
                    {
                        if (pictureBox2.Location.Y > height)
                        {
                            pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + speed * Ydirection);

                        }
                        else
                        {
                            if (pictureBox2.Location.X > temp.Location.X)
                            {
                                pictureBox2.Location = new Point(pictureBox2.Location.X + speed * Xdirection, pictureBox2.Location.Y);
                            }
                            else
                            {
                                iSOnPlace = true;
                                if (temp.BackColor == Color.Lime)
                                {
                                    temp.BackColor = Color.Red;
                                }
                                else //if(temp.BackColor == Color.Red)
                                {
                                    temp.BackColor = Color.Lime;
                                }
                                Ydirection = -Ydirection;
                                Xdirection = -Xdirection;

                            }
                        }
                    }
                    else
                    {
                        if (pictureBox2.Location.X < stage1.X)
                        {
                            pictureBox2.Location = new Point(pictureBox2.Location.X + speed * Xdirection, pictureBox2.Location.Y);
                        }
                        else
                        {
                            if (pictureBox2.Location.Y < stage0.Y)
                            {
                                pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + Ydirection * speed);

                            }
                            else
                            {
                                iSOnPlace = false;

                                Task = false;
                                Ydirection = -Ydirection;
                            }
                        }
                    }
                }
                else
                {
                    if (!iSOnPlace)
                    {
                        if (pictureBox2.Location.Y > height)
                        {
                            pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + speed * Ydirection);

                        }
                        else
                        {
                            if (pictureBox2.Location.X < temp.Location.X)
                            {
                                pictureBox2.Location = new Point(pictureBox2.Location.X + speed * Xdirection, pictureBox2.Location.Y);
                            }
                            else
                            {
                                iSOnPlace = true;
                                if (temp.BackColor == Color.Lime)
                                {
                                    temp.BackColor = Color.Red;
                                }
                                else //if(temp.BackColor == Color.Red)
                                {
                                    temp.BackColor = Color.Lime;
                                }
                                Ydirection = -Ydirection;
                                Xdirection = -Xdirection;


                            }
                        }
                    }
                    else
                    {
                        if (pictureBox2.Location.X > stage1.X)
                        {
                            pictureBox2.Location = new Point(pictureBox2.Location.X + speed * Xdirection, pictureBox2.Location.Y);
                        }
                        else
                        {
                            if (pictureBox2.Location.Y < stage0.Y)
                            {
                                pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + Ydirection * speed);

                            }
                            else
                            {
                                iSOnPlace = false;

                                Task = false;
                                Ydirection = -Ydirection;
                            }
                        }
                    }
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ManagerPanel mg = new ManagerPanel();
            mg.Show();

        }
    }
}

