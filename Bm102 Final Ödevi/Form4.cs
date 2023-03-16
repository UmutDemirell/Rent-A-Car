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
using System.Text.RegularExpressions;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;

namespace Bm102_Final_Ödevi
{
    public partial class Form4 : Form
    {
        OleDbConnection baglantim;
        OleDbDataReader oku;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        //Data base ile bağlantı kurmak için gerekebilecek değişkenler tanımlandı

        public Form4()
        {
            InitializeComponent();
        }

        public void SetMyCustomFormat()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";
            //Kredi kartı gün değerine sahip olmadığı için ay yılda sınırlandırdık
        }

        public double kira,km;
        public void BilgiGuncelle1()
        {
            cmd = new OleDbCommand("Select * from Araclar Where Sase_No = '" + Form2.Sase_No + "' ", baglantim);
            baglantim.Open();
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                label11.Text = oku["Marka"].ToString();
                label12.Text = oku["Model"].ToString();
                label13.Text = oku["Uretim_Yili"].ToString();
                label8.Text = oku["Gunluk_Fiyat"].ToString();
                kira = Convert.ToDouble(oku["Gunluk_Fiyat"]);
                km = Convert.ToDouble(oku["Kilometre"]);
            }
            baglantim.Close();
            //Seçilen arabanın sase numarasını 2. formdan alarak bilgilerini kaydettik.
        }

        public double bakiye;
        public void BilgiGuncelle2()
        {
            cmd = new OleDbCommand("Select Kart_Bakiye from Kullanici Where Tc_No = '" + Form1.user + "' ", baglantim);
            baglantim.Open();
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                if(oku["Kart_Bakiye"].ToString() != "0")
                {
                    label9.Text = oku["Kart_Bakiye"].ToString();
                    bakiye = Convert.ToDouble(oku["Kart_Bakiye"]);
                }
                else
                {
                    label9.Text = "---------";
                }
            }
            baglantim.Close();
            //Eğer kullanıcı arabayı incelerken daha kredi kartı bilgilerini girmemişse değer çekme işlemi yapmadık kiralama esnasında alınacak
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            baglantim = new OleDbConnection("Provider=Microsoft.JET.OleDb.4.0; Data Source=Bilgiler.mdb");
            SetMyCustomFormat();
            BilgiGuncelle1();
            BilgiGuncelle2();
            //Bilgilerimizi çektik
            gmap.MapProvider = GMapProviders.GoogleMap;
            gmap.Position = new PointLatLng(39.92077, 32.85411);
            gmap.DragButton = MouseButtons.Left;
            gmap.Zoom = 65;
            gmap.MaxZoom = 75;
            gmap.MinZoom = 5;
            gmap.ShowCenter = false;
            //Tekrardan haritamızda aynı ayarları uyguladık
            textBox1.MaxLength = 11;
            textBox2.MaxLength = 3;
            //Kredi kartı için sınırlarımızı belirledik
            dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker2.MinDate = DateTime.Today;
            //Tarih sınırlarımızı ayarladık
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            Image image = Image.FromFile(Application.StartupPath + "/Araclar/" +Form2.Sase_No + ".jpg");
            pictureBox1.Image = image;
            //Arabaların fotoğraflarını sase no ile kaydedip ona göre otomatik bir fotoğraf seçip picturebox içine yerleştirme işlemi gerçekleştirdik ve fotoğrafın kutuyu doldurmasını sağladık.
        }

        public bool kayitli_kart = false;
        //Kart önceden kayıtlı ise kiralama anında tekrardan kart bilgilerini istemeyeceğiz
        private void button2_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand("Select * from Kullanici Where Tc_No = '" + Form1.user + "' ", baglantim);
            baglantim.Open();
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                if(oku["Kredi_Kart_Date"].ToString() != "" && oku["Kredi_Kart_No"].ToString() != "" && oku["Kredi_Kart_CVV"].ToString() != "")
                {
                    MessageBox.Show("Haritadan seçip işleminize devam edebilirsiniz");
                    textBox1.Text = oku["Kredi_Kart_No"].ToString();
                    textBox2.Text = oku["Kredi_Kart_CVV"].ToString();
                    dateTimePicker1.Value = DateTime.Parse(oku["Kredi_Kart_Date"].ToString());
                    kayitli_kart = true;
                    //Eğer kart önceden kayıtlıysa direkt haritadan seçim yapılabilir
                }
                else
                {      
                    MessageBox.Show("Kayıtlı bir kart bulunamadı");
                    //Kart kaydetmelisiniz
                }
            }
            baglantim.Close();
        }

        static public DateTime alis = new DateTime();
        //Alış zamanını tutmak için
        private void button1_Click(object sender, EventArgs e)
        {
            string user = Form1.user;
            if(harita_Secimi == true) { 
                //Harita seçildiyse
                if (kayitli_kart == false) 
                { 
                    //Kart kayıtlı değilse
                    if (textBox1.Text != "" && textBox2.Text != "")
                    {
                        //Kart bilgileri doluysa
                        if (textBox1.Text.All(char.IsDigit) && textBox2.Text.All(char.IsDigit) && textBox1.Text.Length == 11&& textBox2.Text.Length == 3)
                        {
                            //Ve kart bilgileri rakam 11 3 uzunluğundaysa
                            Random rnd = new Random();
                            bakiye = rnd.Next(10000, 100000);
                            //Bakiye ataması
                            baglantim.Open();
                            OleDbCommand Guncelle = new OleDbCommand("Update Kullanici Set Kredi_Kart_No = '" + textBox1.Text + "' ,Kredi_Kart_CVV = '" + textBox2.Text + "' ,Kredi_Kart_Date = '" + dateTimePicker1.Value.ToString("MM-yyyy") + "', Kart_Bakiye = '" + bakiye + "' Where Tc_No = '" + user + "' ", baglantim);
                            Guncelle.ExecuteNonQuery();
                            baglantim.Close();
                            textBox1.Clear();
                            textBox2.Clear();
                            dateTimePicker1.ResetText();
                            //Kart bilgilerinin kullanıcı datasına eklenmesi

                            string adress = latitudeA + "-" + longtitudeA;
                            baglantim.Open();
                            OleDbCommand ekle = new OleDbCommand("insert into Kiralanan (Kiralayan,Ilk_Km,Alis_Tarih,Alis_Adres,Sase_No) values('" + user + "','" + km + "','" + dateTimePicker2.Value + "','" + adress  + "','" + Form2.Sase_No + "')", baglantim);
                            ekle.ExecuteNonQuery();
                            baglantim.Close();
                            //Alınan arabanın alış bilgilerinin kiralanan veri bütünlüğüne yazılması

                            alis = dateTimePicker2.Value; 
                            this.Hide();
                            Form2 Frm2 = new Form2();
                            Frm2.ShowDialog();
                            this.Close();
                            //Formun kapatılması
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lutfen Bütün Alanları Doldurunuz");
                        //Kart bilgileri dolu değil
                    }
                }
                else
                {
                    string adress = latitudeA.ToString() + "-" + longtitudeA.ToString();
                    baglantim.Open();
                    cmd = new OleDbCommand("insert into Kiralanan (Kiralayan,Ilk_Km,Alis_Tarih,Alis_Adres,Sase_No,Teslim_Edildi) values('" + user + "', '"+ km + "' ,'" + dateTimePicker2.Value + "','" + adress + "','" + Form2.Sase_No + "', 0)", baglantim);
                    cmd.ExecuteNonQuery();
                    baglantim.Close();

                    this.Hide();
                    Form2 Frm2 = new Form2();
                    Frm2.ShowDialog();
                    this.Close();
                    //Kart bilgilerini değiştirmeden sadece kiralanan bilgileri oluşturuluyor
                }
            }
            else
            {
                MessageBox.Show("Lutfen Alış Noktasını Seçiniz");
                //Harita seçilmemiş
            }
        }

        public static double latitudeA, longtitudeA;
        bool harita_Secimi = false;
        //Harita seçildi mi ve kordinatlar neler

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 Frm2 = new Form2();
            Frm2.ShowDialog();
            this.Close();
            //Arabayı inceledikten sonra kiralamak zorunda kalmamak için geri dönme butonu
        }

        public int i = 0;
        //Aynı şekilde bayrak mantığı ilk markerda öncesi olmayacağı için silme işlemi yapılamayacaktır
        private void gmap_OnMapClick(PointLatLng pointClick, MouseEventArgs e)
        {
            GMapOverlay markers = new GMapOverlay("markers");
            if(i == 0)
            {
                i++;
            }
            else
            {
                gmap.Overlays.RemoveAt(0);
                //Önceki marker silindi
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                latitudeA = gmap.FromLocalToLatLng(e.X, e.Y).Lat;
                longtitudeA = gmap.FromLocalToLatLng(e.X, e.Y).Lng;
                //Kordinatler çekildi
            }
            GMapMarker marker = new GMarkerGoogle(new PointLatLng(latitudeA, longtitudeA), GMarkerGoogleType.blue_pushpin);
            markers.Markers.Add(marker);
            gmap.Overlays.Add(markers);
            //Marker yerleştirildi
            label17.Text = latitudeA.ToString();
            label19.Text = longtitudeA.ToString();
            harita_Secimi = true;
            //Harita seçildiğine işaret verildi ve kordinatlar kullanıcıya gösterildi
        }
    }
}
