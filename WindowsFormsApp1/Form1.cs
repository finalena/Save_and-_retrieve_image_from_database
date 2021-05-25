using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private static string Conn = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = WinFormTest; Integrated Security = True";

        public Form1()
        {
            InitializeComponent();

            this.btnChooseFile.Click += Button_Click;
            this.btnSave.Click += Button_Click;
            this.btnCancel.Click += Button_Click;

        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender.Equals(this.btnChooseFile))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "選擇圖片檔";
                openFileDialog.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    this.lblChooseFile.Text = openFileDialog.SafeFileName;
                    this.pictureBox1.ImageLocation = openFileDialog.FileName;
                }
            }
            else if (sender.Equals(this.btnSave))
            {
                if (this.pictureBox1.ImageLocation == "") 
                {
                    TLC.MsgBox.Show("請選擇要上傳的圖檔", this.Text);
                    return;
                }

                //將檔案讀入二進位資料byte[]
                byte[] aryByte = null;
                using (FileStream fs = new FileStream(this.pictureBox1.ImageLocation, FileMode.Open))
                {
                    aryByte = new byte[(int)fs.Length]; 
                    fs.Read(aryByte, 0, aryByte.Length);
                }

                string sql = "Insert Into BinaryImg(cre_date,cre_time,usr_code,per_code,img_data,img_name,img_size) " +
                             " Values(@cre_date,@cre_time,@usr_code,@per_code,@img_data,@img_name,@img_size)";
                SqlConnection conn = new SqlConnection(Conn);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cre_date", DateTime.Now.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@cre_time", DateTime.Now.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("@usr_code", "User");
                cmd.Parameters.AddWithValue("@per_code", "");
                cmd.Parameters.AddWithValue("@img_data", aryByte);
                cmd.Parameters.AddWithValue("@img_name", this.lblChooseFile.Text);
                cmd.Parameters.AddWithValue("@img_size", (aryByte.Length / 1024.0).ToString("0.00"));
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Close();

                TLC.MsgBox.Show("圖片上傳完成", this.Text);
            }
            else if (sender.Equals(this.btnCancel))
            {
                this.lblChooseFile.Text = "No file chosen";
                this.pictureBox1.ImageLocation = "";
            }
        }
    }
}
