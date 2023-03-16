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

namespace Bm102_Final_Ödevi
{
    public partial class Form3 : Form
    {
        OleDbConnection baglantim;
        OleDbDataReader oku;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        //Data base ile bağlantı kurmak için gerekebilecek değişkenler tanımlandı

        public Form3()
        {
            InitializeComponent();
        }
        public void DataGridGuncelle()
        {
            string user = Form1.user;
            da = new OleDbDataAdapter("Select * from Kullanici ", baglantim);
            ds = new DataSet();
            baglantim.Open();
            da.Fill(ds, "Kullanici");
            dataGridView1.DataSource = ds.Tables["Kullanici"];
            baglantim.Close();
            //Bütün kullanıcıları listeliyoruz
        }
        public void DataGridGuncelle2(int i)
        {
            if (i == 0)
            {
                da = new OleDbDataAdapter("Select * from Araclar", baglantim);
                ds = new DataSet();
                baglantim.Open();
                da.Fill(ds, "Araclar");
                dataGridView2.DataSource = ds.Tables["Araclar"];
                baglantim.Close();
                //Bütün aracları listeliyoruz
            }
            else if (i == 1)
            {
                if(comboBox4.SelectedItem == "Tumu") { 
                    if ((comboBox1.SelectedItem == null && comboBox2.SelectedItem == null && comboBox3.SelectedItem == null))
                    {
                        MessageBox.Show("Lutfen Secim Yapiniz");
                    }
                    else
                    {
                        if (comboBox2.SelectedItem == null && comboBox3.SelectedItem == null)
                        {
                            da = new OleDbDataAdapter("Select * from Araclar where Marka='" + comboBox1.SelectedItem.ToString() + "'", baglantim);
                        }
                        else if (comboBox3.SelectedItem == null && comboBox2.SelectedItem != null)
                        {
                            da = new OleDbDataAdapter("Select * from Araclar where Marka='" + comboBox1.SelectedItem.ToString() + "' AND Model= '" + comboBox2.SelectedItem.ToString() + "'", baglantim);
                        }
                        else if (comboBox2.SelectedItem == null && comboBox3.SelectedItem != null)
                        {
                            da = new OleDbDataAdapter("Select * from Araclar where Marka='" + comboBox1.SelectedItem.ToString() + "' AND Uretim_Yili='" + comboBox3.SelectedItem.ToString() + "'", baglantim);
                        }
                        else
                        {
                            da = new OleDbDataAdapter("Select * from Araclar where Marka='" + comboBox1.SelectedItem.ToString() + "' AND Model= '" + comboBox2.SelectedItem.ToString() + "' AND Uretim_Yili='" + comboBox3.SelectedItem.ToString() + "'", baglantim);
                        }
                        ds = new DataSet();
                        baglantim.Open();
                        da.Fill(ds, "Araclar");
                        dataGridView2.DataSource = ds.Tables["Araclar"];
                        //Aracları filtreleme özelliğine göre listeliyoruz
                    }
                }
                else
                {
                    int hasar;
                    if(comboBox4.SelectedItem == "Evet")
                    {
                        hasar = -1;
                    }
                    else
                    {
                        hasar = 0;
                    }
                    //Hasar var mı yok mu onu kaydedip boolean değerler atıyoruz fakat access 1 yerine -1 kullandığı için -1 atadık

                    if ((comboBox1.SelectedItem == null && comboBox2.SelectedItem == null && comboBox3.SelectedItem == null))
                    {
                        da = new OleDbDataAdapter("Select * from Araclar where Hasar = "+ hasar +" ", baglantim);
                        ds = new DataSet();
                        baglantim.Open();
                        da.Fill(ds, "Araclar");
                        dataGridView2.DataSource = ds.Tables["Araclar"];
                        //Tabloyu sadece hasra göre yeniliyoruz
                    }
                    else
                    {
                        if (comboBox2.SelectedItem == null && comboBox3.SelectedItem == null)
                        {
                            da = new OleDbDataAdapter("Select * from Araclar where Marka='" + comboBox1.SelectedItem.ToString() + "' AND Hasar = " + hasar + "", baglantim);
                        }
                        else if (comboBox3.SelectedItem == null && comboBox2.SelectedItem != null)
                        {
                            da = new OleDbDataAdapter("Select * from Araclar where Marka='" + comboBox1.SelectedItem.ToString() + "' AND Model= '" + comboBox2.SelectedItem.ToString() + "' AND Hasar = " + hasar + "  ", baglantim);
                        }
                        else if (comboBox2.SelectedItem == null && comboBox3.SelectedItem != null)
                        {
                            da = new OleDbDataAdapter("Select * from Araclar where Marka='" + comboBox1.SelectedItem.ToString() + "' AND Uretim_Yili='" + comboBox3.SelectedItem.ToString() + "' AND  Hasar = " + hasar + " ", baglantim);
                        }
                        else
                        {
                            da = new OleDbDataAdapter("Select * from Araclar where Marka='" + comboBox1.SelectedItem.ToString() + "' AND Model= '" + comboBox2.SelectedItem.ToString() + "' AND Uretim_Yili='" + comboBox3.SelectedItem.ToString() + "' AND Hasar  hasar  ", baglantim);
                        }
                        ds = new DataSet();
                        baglantim.Open();
                        da.Fill(ds, "Araclar");
                        dataGridView2.DataSource = ds.Tables["Araclar"];
                        //Tbloyu bütün filtrelere göre yeniliyoruz.
                    }
                }
                baglantim.Close();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            baglantim = new OleDbConnection("Provider=Microsoft.JET.OleDb.4.0; Data Source=Bilgiler.mdb");
            ComboxGuncelle();
            DataGridGuncelle();
            DataGridGuncelle2(0);
            //Datagridlerimizi doldurduk
            dataGridView1.Columns[4].DefaultCellStyle.Format = "MM/yyyy";
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Width = 100;
            column = dataGridView1.Columns[1];
            column.Width = 135;
            column = dataGridView1.Columns[2];
            column.Width = 115;
            column = dataGridView1.Columns[3];
            column.Width = 135;
            column = dataGridView1.Columns[4];
            column.Width = 125;
            column = dataGridView1.Columns[5];
            column.Width = 135;
            column = dataGridView1.Columns[6];
            column.Width = 140;
            column = dataGridView1.Columns[7];
            column.Width = 115;
            column = dataGridView2.Columns[0];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView2.Columns[2];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView2.Columns[5];
            //Datagridlerimizi ayarladık
            textBox1.MaxLength = 17;
            textBox3.MaxLength = 11;
            textBox4.MaxLength = 11;
            //Textbox sınırlarımızı ayarladık
            comboBox4.Items.Add("Tumu");
            comboBox4.Items.Add("Evet");
            comboBox4.Items.Add("Hayır");
            //Comboboxlarda hasar için sadece 3 seçenek olacağı için ellerimizle eklemek daha kolay
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (textBox1.Text.All(char.IsDigit))
                {
                    da = new OleDbDataAdapter("Select * from Araclar Where Sase_No = '" + textBox1.Text + "' ", baglantim);
                    ds = new DataSet();
                    baglantim.Open();
                    da.Fill(ds, "Araclar");
                    dataGridView2.DataSource = ds.Tables["Araclar"];
                    baglantim.Close();
                    //Sase numarasına göre aratma islemi gerçekleştirdik
                }
                else
                {
                    MessageBox.Show("Lütfen Geçerli Bir Sase No Giriniz");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Sase No Giriniz");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridGuncelle2(0);
            //Filtrelemeden sonra tekrar bütün araçları görmek için resetleme butonu
        }

        public void ComboxGuncelle()
        {
            cmd = new OleDbCommand("Select * from Araclar", baglantim);
            baglantim.Open();
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                if (!comboBox1.Items.Contains(oku["Marka"]))
                {
                    comboBox1.Items.Add(oku["Marka"]);
                }
            }
            baglantim.Close();
            //Önceden yaptığımız gibi comboboxumuzu verilerimizle doldurduk ve diğerlerini de burdan seçilen verilere göre doldurucaz
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            cmd = new OleDbCommand("Select * from Araclar", baglantim);
            baglantim.Open();
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                if (oku["Marka"].ToString() == comboBox1.SelectedItem.ToString())
                {
                    comboBox2.Items.Add(oku["Model"]);
                    comboBox3.Items.Add(oku["Uretim_Yili"]);
                }
            }
            baglantim.Close();
            //İlk combobox'a göre onunla eşleşen verilere sahip bilgiler diğer comboboxlara dolduruldu
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            cmd = new OleDbCommand("Select * from Araclar", baglantim);
            baglantim.Open();
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                if (oku["Model"].ToString() == comboBox2.SelectedItem.ToString())
                {
                    comboBox3.Items.Add(oku["Uretim_Yili"]);
                }
            }
            baglantim.Close();
            //Son combobox temizlenerek ikincicombobox'a göre onunla ve ilkiyle eşleşen verilere sahip bilgiler diğer comboboxa dolduruldu
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataGridGuncelle2(1);
            //Filtreleme işlemi yapmak için fonksiyonumuzu 1 parametresiyle çağırdık
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "")
            {
                MessageBox.Show("Lutfen Bosluklardan En Az Birini Doldurunuz");
            }
            else if (textBox2.Text != "" && textBox3.Text == "" && textBox4.Text == "")
            {
                if (textBox2.Text.All(char.IsDigit))
                {
                    da = new OleDbDataAdapter("Select * from Kullanici Where Kullanici_Id = " + Convert.ToInt32(textBox2.Text) + " ", baglantim);
                }
                else
                {
                    MessageBox.Show("Lütfen Gecerli Bilgiler Giriniz");
                } 
            }
            else if (textBox2.Text == "" && textBox3.Text != "" && textBox4.Text == "")
            {
                if (textBox3.Text.All(char.IsDigit))
                {
                    da = new OleDbDataAdapter("Select * from Kullanici Where Tc_No = '" + textBox3.Text + "' ", baglantim);
                }
                else
                {
                    MessageBox.Show("Lütfen Gecerli Bilgiler Giriniz");
                }
            }
            else if (textBox2.Text == "" && textBox3.Text == "" && textBox4.Text != "")
            {
                if (textBox4.Text.All(char.IsDigit))
                {
                    da = new OleDbDataAdapter("Select * from Kullanici Where Kredi_Kart_No = '" + textBox4.Text + "' ", baglantim);
                }
                else
                {
                    MessageBox.Show("Lütfen Gecerli Bilgiler Giriniz");
                }
            }
            else if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text == "")
            {
                if (textBox2.Text.All(char.IsDigit) && textBox3.Text.All(char.IsDigit))
                {
                    da = new OleDbDataAdapter("Select * from Kullanici Where Kullanici_Id = " + Convert.ToInt32(textBox2.Text) + " AND Tc_No = '" + textBox3.Text + "' ", baglantim);
                }
                else
                {
                    MessageBox.Show("Lütfen Gecerli Bilgiler Giriniz");
                }
            }
            else if (textBox2.Text == "" && textBox3.Text != "" && textBox4.Text != "")
            {
                if (textBox3.Text.All(char.IsDigit) && textBox4.Text.All(char.IsDigit))
                {
                    da = new OleDbDataAdapter("Select * from Kullanici Where Tc_No = '" + textBox3.Text + "' AND Kredi_Kart_No = '" + textBox4.Text + "' ", baglantim);
                }
                else
                {
                    MessageBox.Show("Lütfen Gecerli Bilgiler Giriniz");
                }        
            }
            else if (textBox2.Text != "" && textBox3.Text == "" && textBox4.Text != "")
            {
                if (textBox2.Text.All(char.IsDigit) && textBox4.Text.All(char.IsDigit))
                {
                    da = new OleDbDataAdapter("Select * from Kullanici Where Kullanici_Id = " + Convert.ToInt32(textBox2.Text) + " AND Kredi_Kart_No = '" + textBox4.Text + "' ", baglantim);
                }
                else
                {
                    MessageBox.Show("Lütfen Gecerli Bilgiler Giriniz");
                }          
            }
            else
            {
                if (textBox2.Text.All(char.IsDigit) && textBox3.Text.All(char.IsDigit) && textBox4.Text.All(char.IsDigit))
                {
                    da = new OleDbDataAdapter("Select * from Kullanici Where Kullanici_Id = " + Convert.ToInt32(textBox2.Text) + " AND Tc_No = '" + textBox3.Text + "' AND Kredi_Kart_No = '" + textBox4.Text + "' ", baglantim);
                }
                else
                {
                    MessageBox.Show("Lütfen Gecerli Bilgiler Giriniz");
                }       
            }
            ds = new DataSet();
            baglantim.Open();
            da.Fill(ds, "Kullanici");
            dataGridView1.DataSource = ds.Tables["Kullanici"];
            baglantim.Close();
            //Kullanıcı araması yapmak için admini her boşluğu doldurmak zorunda bırakmayarak her ihtimale göre ayrı ayrı verileri çektik ve filtreledik
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataGridGuncelle();
            //Tekrardan bir resetleme tuşu oluşturduk
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0;
            //i'yi sıfırdan başlatarak datagridin row sayısının bir altına gelecek şekilde sonlandırdık çünkü son satırın boş olmasıyla verileri null olarak görmesini istemedik
            baglantim.Open();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if(i < dataGridView1.Rows.Count-1)
                {
                    //i'yi sıfırdan başlatarak datagridin row sayısının bir altına gelecek şekilde sonlandırdık çünkü son satırın boş olmasıyla verileri null olarak görmesini istemedik
                    int admin;
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[8];
                    //Admin  checkbox kısmı dolu mu değil mi diye bakmak için bir değişken oluşturduk
                    if (Convert.ToBoolean(chk.Value) == true)
                    {
                        admin = -1;
                        //Admin yetkisi varsa true yani -1
                    }
                    else
                    {
                        admin = 0;
                        //Yoksa 0
                    }
                    cmd = new OleDbCommand("Update Kullanici Set Ad_Soyad = '" + row.Cells["Ad_Soyad"].Value.ToString() + "'" + ",Sifre = '" + row.Cells["Sifre"].Value.ToString() + "' ,Sifre_Ipucu = '" + row.Cells["Sifre_Ipucu"].Value.ToString() + "', Admin = " + admin + " Where Kullanici_Id = " + Convert.ToInt32(row.Cells["Kullanici_Id"].Value) + " ", baglantim);
                    cmd.ExecuteNonQuery();
                    i++;
                    //Ve kullanıcı bilgilerini güncelleyip i'i 1 arttırdık
                }
            }
            baglantim.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = 0;
            //i'yi sıfırdan başlatarak datagridin row sayısının bir altına gelecek şekilde sonlandırdık çünkü son satırın boş olmasıyla verileri null olarak görmesini istemedik
            baglantim.Open();
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (i < dataGridView2.Rows.Count - 1)
                {
                    //i'yi sıfırdan başlatarak datagridin row sayısının bir altına gelecek şekilde sonlandırdık çünkü son satırın boş olmasıyla verileri null olarak görmesini istemedik
                    int hasar;
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[4];
                    //Hasar  checkbox kısmı dolu mu değil mi diye bakmak için bir değişken oluşturduk
                    if (Convert.ToBoolean(chk.Value) == true)
                    {
                        hasar = -1;
                        //Hasar varsa true yani -1
                    }
                    else
                    {
                        hasar = 0;
                        //Yoksa 0
                    }
                    cmd = new OleDbCommand("Update Araclar Set Uretim_Yili = '" + row.Cells["Uretim_Yili"].Value.ToString() + "', Marka = '" + row.Cells["Marka"].Value.ToString() + "', Model = '" + row.Cells["Model"].Value.ToString() + "', Hasar = " + hasar + ", Kilometre = " + Convert.ToInt32(row.Cells["Kilometre"].Value) + " ,Gunluk_Fiyat = " + Convert.ToInt32(row.Cells["Gunluk_Fiyat"].Value) + " Where Sase_No = '" + row.Cells["Sase_No"].Value.ToString() + "' ", baglantim);
                    cmd.ExecuteNonQuery();
                    i++;
                    //Ve Arac bilgilerini güncelleyip i'i 1 arttırdık
                }
            }
            baglantim.Close();

        }

    }
}
