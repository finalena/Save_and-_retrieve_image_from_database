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
        List<BinaryImg> list = null;

        public Form1()
        {
            InitializeComponent();
            InitTitle();

            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;

            this.listView1.FullRowSelect = true;
            this.listView1.Scrollable = true;
            this.listView1.MultiSelect = false;
            this.listView1.GridLines = true;
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Img_name");
    
            this.listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
            this.btnChooseFile.Click += Button_Click;
            this.btnUpload.Click += Button_Click;
            this.btnCancel.Click += Button_Click;
            this.btnDownload.Click += Button_Click;
            this.btnLoadAll.Click += Button_Click;
        }

        private void InitTitle()
        {
            this.lblChooseFile.Text = "No file chosen";
            this.pictureBox1.ImageLocation = null;
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.FocusedItem != null)
            {
                this.pictureBox1.Image = ConvertBinaryToImage(list[listView1.FocusedItem.Index].img_data);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender.Equals(this.btnCancel))
            {
                InitTitle();
            }
            else if (sender.Equals(this.btnChooseFile))
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
            else if (sender.Equals(this.btnUpload))
            {
                if (this.pictureBox1.ImageLocation == null) 
                {
                    TLC.MsgBox.Show("請選擇要上傳的圖檔", this.Text);
                    return;
                }

                byte[] aryByte = ConvertImageToBinary(this.pictureBox1.ImageLocation);
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
                InitTitle();
            }
            else if (sender.Equals(this.btnLoadAll))
            {
                this.listView1.Items.Clear();
                list = new List<BinaryImg>();

                string sql = "select * from BinaryImg order by ser_no";
                SqlConnection conn = new SqlConnection(Conn);
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        list.Add(new BinaryImg{
                            cre_date = (string)dr["cre_date"],
                            cre_time = (string)dr["cre_time"],
                            usr_code = (string)dr["usr_code"],
                            per_code = (string)dr["per_code"],
                            img_data = (byte[])dr["img_data"],
                            img_name = (string)dr["img_name"],
                            img_size = (string)dr["img_size"]
                        });
                    }
                }
                cmd.Dispose();
                conn.Close();

                foreach (BinaryImg Img in list)
                {
                    ListViewItem item = new ListViewItem(Img.img_name);
                    listView1.Items.Add(item);
                }
            }
            else if (sender.Equals(this.btnDownload))
            {
                if (this.listView1.FocusedItem != null)
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + list[this.listView1.FocusedItem.Index].img_name;
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        byte[] aryWrite = list[this.listView1.FocusedItem.Index].img_data;
                        fs.Write(aryWrite, 0, aryWrite.Length);
                    }

                    TLC.MsgBox.Show("檔案下載完成: " + path, this.Text);
                }
            }
        }

        private byte[] ConvertImageToBinary(string ImgPath)
        {
            using (FileStream fs = new FileStream(ImgPath, FileMode.Open))
            {
                byte[] aryByte = new byte[(int)fs.Length];
                fs.Read(aryByte, 0, aryByte.Length);

                return aryByte;
            }
        }

        private Image ConvertBinaryToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
