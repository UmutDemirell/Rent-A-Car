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
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;

namespace Bm102_Final_Ödevi
{
    public partial class Form2 : Form
    {

        OleDbConnection baglantim;
        OleDbDataReader oku;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        //Data base ile bağlantı kurmak için gerekebilecek değişkenler tanımlandı

        public Form2()
        {
            InitializeComponent();
        }

        public void DataGridGuncelle()
        {
                string user = Form1.user;
                da = new OleDbDataAdapter("Select * from Kullanici where Tc_No='" + user + "'", baglantim);
                ds = new DataSet();
                baglantim.Open();
                da.Fill(ds, "Kullanici");
                dataGridView1.DataSource = ds.Tables["Kullanici"];
                baglantim.Close();
            //Kullanıcı sayfasında kullanıcının bilgilerinin de gözükmesini istediğimiz için form load işleminde çalışacak bir datagrid oluşturduk
            //Data adapter ile verileri data sete doldurup data setteki verileri de gridview kısmına yazıyoruz
        }

        public void DataGridGuncelle2(int i)
        {
            if(i == 0) 
            //i'ye göre parametreli olma sebebi filtreleme yapıplıp yapılmayacağıdır
            {
                da = new OleDbDataAdapter("Select * from Araclar", baglantim);
                ds = new DataSet();
                baglantim.Open();
                da.Fill(ds, "Araclar");
                dataGridView2.DataSource = ds.Tables["Araclar"];
                baglantim.Close();
                //Bütün araçların verisini mağazadaki kataloğa doluruyoruz
                //Data adapter ile verileri data sete doldurup data setteki verileri de gridview kısmına yazıyoruz
            }
            else if (i == 1)
            {
                if((comboBox1.SelectedItem == null && comboBox2.SelectedItem == null && comboBox3.SelectedItem == null))
                {
                    MessageBox.Show("Lutfen Secim Yapiniz");
                    //Eğer kullanıcı seçim yapmadıysa listeme gerçekleşmesine gerek yok
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
                    //Seçimlerin durumuna göre gridview'imizi yeniden şekillendiriyoruz kullanıcyı her seçeneği seçmek zorunda bırakmıyoruz
                    ds = new DataSet();
                    baglantim.Open();
                    da.Fill(ds, "Araclar");
                    dataGridView2.DataSource = ds.Tables["Araclar"];
                    //Data adapter ile verileri data sete doldurup data setteki verileri de gridview kısmına yazıyoruz
                }
                baglantim.Close();
            }
        }
        public void DataGridGuncelle3()
        {
            string user = Form1.user;
            da = new OleDbDataAdapter("Select * from Kiralanan where Kiralayan='" + user + "' AND Teslim_Edildi = " + 0 + " ", baglantim);
            ds = new DataSet();
            baglantim.Open();
            da.Fill(ds, "Kiralanan");
            dataGridView3.DataSource = ds.Tables["Kiralanan"];
            baglantim.Close();
            //Kullanıcının kiralama bilgilerini de 3. sekmemizdeki gridviewimize yazıyoruz
            //Data adapter ile verileri data sete doldurup data setteki verileri de gridview kısmına yazıyoruz
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
            //Combobox'ımızı Araclar veri setimizdeki bilgilerle dolduruyoruz ve bu combobox değişimine göre diğer comboboxlarımız da şekillenecek
        }

        public void SetMyCustomFormat()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";
            //Kredi kartında günler olmadığı için ay ve yıla göre ayarlıyoruz
        }

        public double bakiye = 0;
        public void Bakiye_Sorgu()
        {
            cmd = new OleDbCommand("Select Kart_Bakiye from Kullanici Where Tc_No = '" + Form1.user + "' ", baglantim);
            baglantim.Open();
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                if (oku["Kart_Bakiye"].ToString() != "0")
                {
                    bakiye = Convert.ToDouble(oku["Kart_Bakiye"]);
                }
                else
                {
                    label32.Text = "---------";
                }
            }
            baglantim.Close();
        }
        //Kullanıcının bilgilerini gösterirken kullanıcı daha kart bilgilerini girmemiş olabileceğinden bakiyesine erişemeyebiliriz o yüzden 0'a eşit değilse sadece bakiye ataması yapılır.

        public double kira_Fiyat = 0,Ilk_Km;
        public void Kira_Sorgu()
        {
                if (dataGridView3.SelectedRows.Count != 0)
                {
                //Datagriddeki seçili elemanın sayısı 0 değilse verilerine erişmeye çalışabiliriz
                    DataGridViewRow row = this.dataGridView3.SelectedRows[0];
                    Ilk_Km = Convert.ToDouble(row.Cells["Ilk_Km"].Value);
                    //Seçili satırdaki ilk kilometre değerini çekiyoruz ileride kullanmak için. (Alınan yol hesabı)
                    cmd = new OleDbCommand("Select Gunluk_Fiyat from Araclar Where Sase_No = '" + row.Cells["Sase_No"].Value + "' ", baglantim);
                    baglantim.Open();
                    oku = cmd.ExecuteReader();
                    while (oku.Read())
                    {
                        kira_Fiyat = Convert.ToDouble(oku["Gunluk_Fiyat"]);
                        //Kullanıcının faturası için de aracın günlük kiralama fiyatını çekiyoruz
                    }
                    baglantim.Close();
                 }
        }

        public string birlesim;
        //Enlem vce boylamın bir arada bulunan hali için bir değişken
        public double[] uzaklık = new double[2];
        //Enlem boylam hesabından ikisini de ayırarak kilometre ölçümü yapmak için kullandığımız uzunluklar
        public DateTime date2 = new DateTime();
        //Kullanıcının arabayı aldığı tarihi tutmak için 
        public void Uzaklık_Sorgu()
        {
            if (dataGridView3.SelectedRows.Count != 0)
            {
                cmd = new OleDbCommand("Select * from Kiralanan Where Kiralayan = '" + Form1.user + "' ", baglantim);
                baglantim.Open();
                oku = cmd.ExecuteReader();
                while (oku.Read())
                {
                    birlesim = (oku["Alis_Adres"].ToString());
                    date2 = Convert.ToDateTime(oku["Alis_Tarih"]);
                    //Kullanıcının kiraladığı aracın ve kiaralama durumunun verilerini alıyoruz ki verilerindeki değişimleri de hesaplayabilelim
                    Sase_No = oku["Sase_No"].ToString();
                }
                baglantim.Close();
                uzaklık[0] = Convert.ToDouble(birlesim.Split('-')[0]);
                uzaklık[1] = Convert.ToDouble(birlesim.Split('-')[1]);
                //Uzaklık hesabı için veriler parçalandı
                dateTimePicker2.MinDate = date2;
                //Kullanıcının aldığı tarihten önce arabayı vermesi imkansız olduğu için seçimi sınırlandırıyoruz
            }
        }

        public void Bigileri_Doldur()
        {
            var kullanici = dataGridView1.Rows[0];

            if (kullanici.Cells[3].Value.ToString() != "" || kullanici.Cells[4].Value.ToString() != "" || kullanici.Cells[5].Value.ToString() != "")
            {
                textBox1.Text = kullanici.Cells[3].Value.ToString();
                textBox2.Text = kullanici.Cells[5].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(kullanici.Cells[4].Value.ToString());
            }

            if (kullanici.Cells[1].Value.ToString() != "")
            {
                String kullanici_ad_soyad = kullanici.Cells[1].Value.ToString();
                string[] ad_soyad = kullanici_ad_soyad.Split(' ');

                textBox6.Text = "";
                int i;
                for (i = 0; i < ad_soyad.Length - 1; i++)
                {
                    textBox6.Text += ad_soyad[i] + " ";
                }
                textBox7.Text = ad_soyad[i];
            }
            
            if(kullanici.Cells[9].Value.ToString() != "")
            {
                richTextBox1.Text = kullanici.Cells[9].Value.ToString();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            baglantim = new OleDbConnection("Provider=Microsoft.JET.OleDb.4.0; Data Source=Bilgiler.mdb");
            ComboxGuncelle();
            SetMyCustomFormat(); 
            DataGridGuncelle();
            DataGridGuncelle2(0);
            DataGridGuncelle3();
            Bigileri_Doldur();
            Bakiye_Sorgu();
            //Fonksiyonlarımız çağırıldı
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].DefaultCellStyle.Format = "MM/yyyy";
            dataGridView1.RowTemplate.Height = 77;
            dataGridView2.Columns[4].Visible = false;
            dataGridView3.Columns[2].Visible = false;
            dataGridView3.Columns[4].Visible = false;
            dataGridView3.Columns[6].Visible = false;
            dataGridView3.Columns[9].Visible = false;
            DataGridViewColumn column = dataGridView1.Columns[1];
            column.Width = 155;
            column = dataGridView1.Columns[2];
            column.Width = 115;
            column = dataGridView1.Columns[3];
            column.Width = 140;
            column = dataGridView1.Columns[4];
            column.Width = 140;
            column = dataGridView1.Columns[5];
            column.Width = 140;
            column = dataGridView1.Columns[6];
            column.Width = 140;
            column = dataGridView1.Columns[7];
            column.Width = 115;
            column = dataGridView1.Columns[9];
            column.Width = 177;
            column = dataGridView2.Columns[0];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView2.Columns[2];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView2.Columns[5];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView3.Columns[0];
            column.Width = 105;
            column = dataGridView3.Columns[1];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView3.Columns[3];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView3.Columns[5];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView3.Columns[7];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column = dataGridView3.Columns[8];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.Visible = false;
            //Datagridlerimiz güzel gözüksün ve kullanıcının ihtiyacı olmayan veriler gizlensin diye küçük düzenlemeler yaptık
            textBox1.MaxLength = 11;
            textBox2.MaxLength = 3;
            //Kredi kartı bilgileri 11 ve 3 basamak olmak zorunda olduğu için sınırlandırma yapıyoruz
            label32.Text = bakiye.ToString();
            //Labelımıza kullanıcı bakiyesini görsün diye bakiye değerini yazıyoruz
            dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker2.MinDate = DateTime.Today;
            //Tarih sınırlarını çekiyoruz

            gmap.MapProvider = GMapProviders.GoogleMap;
            gmap.Position = new PointLatLng(39.92077, 32.85411);
            gmap.DragButton = MouseButtons.Left;
            gmap.Zoom = 65;
            gmap.MaxZoom = 75;
            gmap.MinZoom = 5;
            gmap.ShowCenter = false;
            //gmap eklentisine google mapimizi ekledik ve ardından türkiyenin ortasından bir başlangıç değeri belirledik.
            //Max min zoom değerlerini de ayarladıktan sonra haritanın ortasındaki cross'u kaldırdık haritanın sağ değil sol mouse ile hareket ettirilmesini de sağladık.
        }
        //column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int bakiye = rnd.Next(10000,100000);
            //Kart bilgilerinden gerçek bir bakiye çekemeyeceğimiz için random kullanmak zorunda kalıyoruz
            if (textBox1.Text != "" && textBox2.Text != "")
            {
            //Değerler boş değilse işlem yapılabilir
                string user = Form1.user;
                if (textBox1.Text.All(char.IsDigit) && textBox2.Text.All(char.IsDigit))
                {
                //Kredi kartında harf bulunamayacağı için isDigit kontrolü yapıyoruz
                    baglantim.Open();
                    OleDbCommand Guncelle = new OleDbCommand("Update Kullanici Set Kredi_Kart_No = '" + textBox1.Text + "' ,Kredi_Kart_CVV = '" + textBox2.Text + "' ,Kredi_Kart_Date = '" + dateTimePicker1.Value.ToString("MM-yyyy") + "', Kart_Bakiye = '" + bakiye + "' Where Tc_No = '" + user + "' ", baglantim);
                    Guncelle.ExecuteNonQuery();
                    baglantim.Close();
                    textBox1.Clear();
                    textBox2.Clear();
                    dateTimePicker1.ResetText();
                    DataGridGuncelle();
                    //Kullanıcının verilerine girdiği kart verilerini yazıyoruz
                }
                else
                {
                    MessageBox.Show("Lutfen Gecerli Kart Bilgileri Giriniz");
                    //Bütün alanların doğru girilmiş olmasını istiyoruz
                }
            }
            else
            {
                MessageBox.Show("Lutfen Bütün Alanları Doldurunuz");
                //Null değer girmemek için bütün alanların dolu olmasını istiyoruz
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Text != "") 
            {
                //Null değer girmemek için bütün alanların dolu olmasını istiyoruz
                string user = Form1.user;
                baglantim.Open();
                cmd = new OleDbCommand("Update Kullanici Set Adress = '" + richTextBox1.Text + "'  Where Tc_No = '" + user + "' ", baglantim);
                cmd.ExecuteNonQuery();
                baglantim.Close();
                richTextBox1.Clear();
                DataGridGuncelle();
                //Kullanıcının verilerine girdiği adres verilerini yazıyoruz
            }
            else
            {
                MessageBox.Show("Lütfen Adress Alanını Boş Bırakmayın");
                //Null değer girmemek için bütün alanların dolu olmasını istiyoruz
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string pass = Form1.pass;

            if(textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox9.Text != "")
            {
                //Null değer girmemek için bütün alanların dolu olmasını istiyoruz

                if (textBox3.Text != textBox4.Text)
                {
                    //Öncelikle girdiği şifre değerlerini kontrol ediyoruz aynı olmalılar
                    MessageBox.Show("Sifreler Uyusmuyor");
                    textBox3.Clear();
                    textBox4.Clear();
                }
                else if (pass != textBox9.Text)
                {
                    MessageBox.Show("Eski Şifre Yanlış Girildi");
                    //Eski şifre kullanılarak güvenlik arttırıyoruz
                }
                else if (pass == textBox3.Text)
                {
                    MessageBox.Show("Girilen Sifre Eskisi İle Aynı Olamaz");
                    //Eski şifresini ile aynı olup olmadığını kontrol ediyoruz
                }
                else{ 
                    string user = Form1.user;
                    baglantim.Open();
                    cmd = new OleDbCommand("Update Kullanici Set Sifre = '" + textBox3.Text + "' , Sifre_Ipucu = '" + textBox5.Text + "'  Where Tc_No = '" + user + "' ", baglantim);
                    cmd.ExecuteNonQuery();
                    baglantim.Close();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    DataGridGuncelle();
                    //Eğer hepsinden geçtiyse kullanıcının şifre verilerini yeniliyoruz
                }
            }
            else
            {
                MessageBox.Show("Lütfen Tüm Alanları Doldurun");
                //Null değer girmemek için bütün alanların dolu olmasını istiyoruz
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox6.Text != "" && textBox7.Text != "")
            {
                //Null değer girmemek için bütün alanların dolu olmasını istiyoruz
                bool isim_sorgu = textBox6.Text.All(i => char.IsLetter(i) || char.IsWhiteSpace(i));
                if (isim_sorgu && textBox7.Text.All(char.IsLetter))
                {
                    //İsimde harf bulunamayacağı için isletter kontrolü yapıyoruz
                    string Ad_Soyad = textBox6.Text + " " + textBox7.Text;
                    string user = Form1.user;
                    baglantim.Open();
                    cmd = new OleDbCommand("Update Kullanici Set Ad_Soyad = '" + Ad_Soyad + "'  Where Tc_No = '" + user + "' ", baglantim);
                    cmd.ExecuteNonQuery();
                    baglantim.Close();
                    textBox6.Clear();
                    textBox7.Clear();
                    DataGridGuncelle();
                    //Kullanıcının isim bilgilerini girdiklerine göre yeniliyoruz
                }
                else
                {
                    MessageBox.Show("Lütfen Gecerli Bir İsim Giriniz");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Tüm Boslukları Doldurunuz");
            }
            //Null değer girmemek için bütün alanların dolu olmasını istiyoruz
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataGridGuncelle2(1);
            //Filtreleme olacağı için datagrid içine 1 ile gönderme yapıyoruz
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
                if(oku["Marka"].ToString() == comboBox1.SelectedItem.ToString())
                {
                    comboBox2.Items.Add(oku["Model"]);
                    comboBox3.Items.Add(oku["Uretim_Yili"]);
                }
            }
            baglantim.Close();
            //Combobox'un içindeki seçilen değere göre o değere sahip arabaların modellerini ve üretim yıllarını da diğer comboboxlara etkliyoruz
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
            //Model değişimi yapıldığında da üretim yılı comboboxunu temizleyip yeniden hem marka hem model'le eşleşen yılları ekliyoruz
        }

        public static string Sase_No = "0";
        //Form4'te arabanın verilerini çekebilmek için statik bir dğeişken oluşturduk
        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count != 0)
            {
                //Eğer herhangi bir datagrid satırı seçiliyse o satırın Şase No değerini alarak bunu kaydediyor ve araç özellikleri formuna geçiyor seçili değilse bir işlem yapılmıyot
                DataGridViewRow row = this.dataGridView2.SelectedRows[0];
                Sase_No = row.Cells["Sase_No"].Value.ToString();
                this.Hide();
                Form4 Frm4 = new Form4();
                Frm4.ShowDialog();
                this.Close();
            }
        }

        public static double latitudeB, longtitudeB;
        //Uzaklık ölçmek için kullanıcının arabayı geri teslim ettiği yerin kornidatları
        bool harita_Secimi = false;
        //Haritada seçim yapılmadıysa işlem geçekleşmesin diye bir boolean değer
        public int i = 0;
        //Sonradan eklenilen markerlar eski markerları silsin diye yazılan kodda ilk marker koyulurken daha öncesi olmayacağı için yine bayrak yöntemi gerçekleştiriyoruz ve bayrak inikken silme işlemi yapmıyoruz

        public int Kullanilan_Gun = 0;

        private void gmap_OnMapClick(PointLatLng pointClick, MouseEventArgs e)
        {
            Kira_Sorgu();
            GMapOverlay markers = new GMapOverlay("markers");
            if (i == 0)
            {
                i++;
            }
            //Sonradan eklenilen markerlar eski markerları silsin diye yazılan kodda ilk marker koyulurken daha öncesi olmayacağı için yine bayrak yöntemi gerçekleştiriyoruz ve bayrak inikken silme işlemi yapmıyoruz

            else
            {
                gmap.Overlays.RemoveAt(0);
                //Eğer ilk marker dğeilse öncekini siliyoruz
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                latitudeB = gmap.FromLocalToLatLng(e.X, e.Y).Lat;
                longtitudeB = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
                //Üzerine tıklanılan noktanın kordinatlarını alıyoruz
            }
            GMapMarker marker = new GMarkerGoogle(new PointLatLng(latitudeB, longtitudeB), GMarkerGoogleType.blue_pushpin);
            markers.Markers.Add(marker);
            gmap.Overlays.Add(markers);
            //Tıklanılan noktaya blue pushpin diye adlandırılan görselli markeri yerleştiriyoruz
            label21.Text = latitudeB.ToString();
            label22.Text = longtitudeB.ToString();
            harita_Secimi = true;
            //Seçim yapıldığı için boolean true oluyor
            Uzaklık_Sorgu();
            //Haritaya tıklanıldığında kullanıcının arabayı aldığı yerden buraya kadar ki yaptığı yolu bulmak için kullancağımız fonksiyonu çağırıyoruz.
            label34.Text = Convert.ToInt32(Calculate(latitudeB, longtitudeB, uzaklık[0], uzaklık[1])).ToString() + "Km";
            label28.Text = (Ilk_Km + Convert.ToInt32(Calculate(latitudeB, longtitudeB, uzaklık[0], uzaklık[1]))).ToString();
            //Kullanıcıya da bu verileri yansıtıyoruz
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            Kira_Sorgu();
            Uzaklık_Sorgu();
            //Burda karşılaştırmak için kullanmamız gereken verilerin bazılarını bu fonksiyonlardan aldığımız için önce onları çağırıyoruz.
            System.TimeSpan diff = dateTimePicker2.Value.Subtract(date2);
            //İki süre arasındaki farkı buluyoruz
            Kullanilan_Gun = diff.Days;
            //Ve gün üzerinden hesap yapacağımız için gün kısmını seçiyoruz sadece
            if(Kullanilan_Gun == 0)
            {
                Kullanilan_Gun = 1;
                label30.Text = Kullanilan_Gun.ToString();
                label25.Text = (Kullanilan_Gun * kira_Fiyat).ToString();
                //Eğer gün içinde verildiyse 1 günlük kullanılmış gibi hesaplıyoruz
            }
            else
            {
                label30.Text = Kullanilan_Gun.ToString();
                label25.Text = (Kullanilan_Gun * kira_Fiyat).ToString();
                //Kaç gün kullanıldıysa o kadar fiyatı yansıtıyoruz
            }
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {     
            if (dataGridView3.SelectedRows.Count != 0)
            {
                DataGridViewRow row = dataGridView3.SelectedRows[0];
                dateTimePicker2.MinDate = DateTime.Parse(row.Cells["Alis_Tarih"].Value.ToString());
                button5.Enabled = true;
            }
            //Eğer bi satır seçili değilse butonumuz disabled seçiliyse enabled oluyor
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DataGridGuncelle2(0);
            //Eğer filtreledikten sonra tekrar bütün arabalrı görmek istersek resetleme tuşu
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string teslim = latitudeB.ToString() + "-" + longtitudeB.ToString();
            baglantim.Open();
            cmd = new OleDbCommand("Update Kiralanan Set Son_Km = '" + Convert.ToInt32(label28.Text) + "' ,Teslim_Tarih = '" + dateTimePicker2.Value + "' ,Teslim_Adres = '" + teslim + "' , Teslim_Edildi = " + 1 + " Where sase_No = '" + Sase_No + "' ", baglantim);
            cmd.ExecuteNonQuery();
            baglantim.Close();
            //Kiralanan arac geri teslim edileceği için teslim bilgilerini yazıyoruz

            double kalan = Convert.ToInt32(label32.Text) - Convert.ToInt32(label25.Text);
            baglantim.Open();
            cmd = new OleDbCommand("Update Kullanici Set Kart_Bakiye = '" + kalan + "' Where Tc_No = '" + Form1.user + "' ", baglantim);
            cmd.ExecuteNonQuery();
            baglantim.Close();
            //Aynı şekilde kullanıcının bakiyesinden de ücreti çekiyoruz

            DataGridGuncelle();
            DataGridGuncelle3();
            button5.Enabled = false;
            string cizgi = "-------";
            label21.Text = cizgi;
            label22.Text = cizgi;
            label28.Text = cizgi;
            label34.Text = cizgi;
            dateTimePicker2.ResetText();
            label25.Text = cizgi;
            label30.Text = cizgi;
            //Kullanıcıya gösterilen verileri temizliyoruz ve datagridleri tekrar güncelliyoruz
        }

        public static double Calculate(double sLatitude, double sLongitude, double eLatitude,
                               double eLongitude)
        {
            var radiansOverDegrees = (Math.PI / 180.0);

            var sLatitudeRadians = sLatitude * radiansOverDegrees;
            var sLongitudeRadians = sLongitude * radiansOverDegrees;
            var eLatitudeRadians = eLatitude * radiansOverDegrees;
            var eLongitudeRadians = eLongitude * radiansOverDegrees;

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;
            //Enlem ve boylamlardan iki nokta arası mesafe hesabı yapıyoruz
        }
    }
}
