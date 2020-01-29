using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace _4444
{
    public partial class Form1 : Form
    {
        public OleDbConnection connection = new OleDbConnection();
        public OleDbCommand command;
        public OleDbDataReader reader;
        //public OleDbDataReader reader2;
        public Form1()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DBmaxim.accdb;" + "Persist Security Info=False";
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            //הוצאת הפרטים מהדאטה בייס לליסט ויו

            try
            {
                listView1.Items.Clear();
                connection.Open();
                command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "select * from Employed";
                reader = command.ExecuteReader(); //הוצאת נתונים

                while (reader.Read())
                {
                    // יצירת משתנה אל.וי והכנסת פרמטרים לתוכו.
                    ListViewItem lv = new ListViewItem(reader["id"].ToString());
                    lv.SubItems.Add(reader["fname"].ToString());
                    lv.SubItems.Add(reader["lname"].ToString());
                    lv.SubItems.Add(reader["age"].ToString());
                    lv.SubItems.Add(reader["yearsarmy"].ToString());
                    lv.SubItems.Add(reader["yearearmy"].ToString());
                    listView1.Items.Add(lv); // שליחת אל וי לתוך ליסט ויו.. 
                }
                reader.Close();
                connection.Close();
                label13.Visible = false;

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            connection.Close();
        }

        private void btnShowByID_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.Connection = connection;

                if (!(string.IsNullOrWhiteSpace(txtID.Text))) //בדיקה אם זה ריק  - לא ימחק 
                {
                    command.CommandText = "select * from Employed where id= (" + txtID.Text + ");";
                    reader = command.ExecuteReader();//הוצאת נתונים

                    while (reader.Read())
                    {
                        txtID.Text = reader["id"].ToString();
                        txtFname.Text = reader["fname"].ToString();
                        txtLname.Text = reader["lname"].ToString();
                        txtAge.Text = reader["age"].ToString();
                    }
                    reader.Close();

                    label13.Visible = false;

                }
                else
                {
                    label13.Visible = true;
                    MessageBox.Show("ID field is empty !");// אחרת תרשום שהשדה ריק
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            connection.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.Connection = connection;

                if (!(string.IsNullOrWhiteSpace(txtID.Text))) //בדיקה אם זה ריק  - לא ימחק 
                {
                    DialogResult result = MessageBox.Show("Do you want to delelet " + txtID.Text + "?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        command.CommandText = "delete from Employed where id= (" + txtID.Text + ");";
                        command.ExecuteNonQuery();
                        MessageBox.Show("Deleted " + txtID.Text + " !");//סיום מחיקה תקינה.
                        label13.Visible = false;
                        deleteField();// אתחול השדות  
                    }
                    else
                    {
                        label13.Visible = false;
                        MessageBox.Show("Not deleled " + txtID.Text + " !!! ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    label13.Visible = true;
                    MessageBox.Show("ID field is empty !");// אחרת תרשום שהשדה ריק
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            connection.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.Connection = connection;

                if (!(string.IsNullOrWhiteSpace(txtID.Text)) && !(string.IsNullOrWhiteSpace(txtFname.Text)) && !(string.IsNullOrWhiteSpace(txtLname.Text)) && !(string.IsNullOrWhiteSpace(txtAge.Text))) // אם זה לא ריק  - תבצע הוספת עובד
                {
                    command.CommandText = "insert into Employed (id,fname,lname,age,sarmy,earmy,yearsarmy,yearearmy) values ('" + txtID.Text + "','" + txtFname.Text + "','" + txtLname.Text + "','" + txtAge.Text + "','" + dateStart.Text + "','" + dateFinsh.Text + "','" + dateStart.Value.Year.ToString() + "','" + dateFinsh.Value.Year.ToString() + "')";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Good !");//סיום הוספה תקינה.

                    emptyField();// מחיקת כוכביות - בגלל שידוע שכאשר הגענו לפה כל השדות מלאים וכעת נאתחל
                    deleteField();// אתחול השדות  

                }
                else
                {
                    emptyField();
                    MessageBox.Show("Some filed is empty !");// אחרת תרשום שהשדה ריק
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            connection.Close();
        }
        public void deleteField()
        {
            txtID.Text = "";
            txtFname.Text = "";
            txtLname.Text = "";
            txtAge.Text = "";
        }
        public void emptyField()
        {
            if ((string.IsNullOrWhiteSpace(txtID.Text)))
                label13.Visible = true;
            else
                label13.Visible = false;
            if ((string.IsNullOrWhiteSpace(txtFname.Text)))
                label12.Visible = true;
            else
                label12.Visible = false;
            if ((string.IsNullOrWhiteSpace(txtLname.Text)))
                label11.Visible = true;
            else
                label11.Visible = false;
            if ((string.IsNullOrWhiteSpace(txtAge.Text)))
                label10.Visible = true;
            else
                label10.Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            deleteField();// מנקה את כל השדות 
            listView1.Items.Clear();
            // מנקה את הכוכביות
            label13.Visible = false;
            label12.Visible = false;
            label11.Visible = false;
            label10.Visible = false;

            lblCount.Text = "";
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            //הוצאת הפרטים ע"י לחיצה יחידה על השורה הרלוונטית והעתקה לטקסט בוקס
            //0 בכולם קובע שאנחנו בשורה הראשונה
            // 0,1,2 מספר של העמודה עצמה
            txtID.Text = listView1.SelectedItems[0].SubItems[0].Text;
            txtFname.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txtLname.Text = listView1.SelectedItems[0].SubItems[2].Text;
            txtAge.Text = listView1.SelectedItems[0].SubItems[3].Text;
            // dateTimePicker3.Text = listView1.SelectedItems[0].SubItems[4].Text;
            // dateTimePicker4.Text = listView1.SelectedItems[0].SubItems[5].Text;
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            showCount();
        }

        public void showCount()
        {
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "select count (id) from Employed ";
                reader = command.ExecuteReader();

                while (reader.Read())
                    lblCount.Text = reader[0].ToString();
                reader.Close();
                command.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            connection.Close();
        }

        private void btnAvg_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "select avg(age) from Employed ";
                reader = command.ExecuteReader();

                while (reader.Read())
                    lblCount.Text = reader[0].ToString();
                reader.Close();
                command.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            connection.Close();
        }

        private void btnBetween_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                connection.Open();
                command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "select * from Employed where yearsarmy between '" + dateTimePicker3.Value.Year.ToString() + "' and '" + dateTimePicker4.Value.Year.ToString() + "' ";
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // יצירת משתנה אל.וי והכנסת פרמטרים לתוכו.
                    ListViewItem lv = new ListViewItem(reader["id"].ToString());
                    lv.SubItems.Add(reader["fname"].ToString());
                    lv.SubItems.Add(reader["lname"].ToString());
                    lv.SubItems.Add(reader["age"].ToString());
                    lv.SubItems.Add(reader["yearsarmy"].ToString());
                    lv.SubItems.Add(reader["yearearmy"].ToString());
                    listView1.Items.Add(lv); // שליחת אל וי לתוך ליסט ויו.. 
                }
                reader.Close();
                command.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            connection.Close();
        }

        private void btnFinshBetween_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                connection.Open();
                command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "select * from Employed where yearearmy between '" + dateTimePicker3.Value.Year.ToString() + "' and '" + dateTimePicker4.Value.Year.ToString() + "' ";
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // יצירת משתנה אל.וי והכנסת פרמטרים לתוכו.
                    ListViewItem lv = new ListViewItem(reader["id"].ToString());
                    lv.SubItems.Add(reader["fname"].ToString());
                    lv.SubItems.Add(reader["lname"].ToString());
                    lv.SubItems.Add(reader["age"].ToString());
                    lv.SubItems.Add(reader["yearsarmy"].ToString());
                    lv.SubItems.Add(reader["yearearmy"].ToString());
                    listView1.Items.Add(lv); // שליחת אל וי לתוך ליסט ויו.. 
                }
                reader.Close();
                command.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            connection.Close();
        }
    }
}




