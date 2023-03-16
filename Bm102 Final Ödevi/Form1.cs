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
using System.Drawing.Text;

namespace Bm102_Final_Ödevi
{
    public partial class Form1 : Form
    {

        OleDbConnection baglantim;
        OleDbDataReader oku;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        //Database bağlantılarımızı oluşturmak için gerekebilecek öğeleri oluşturuyoruz

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            baglantim = new OleDbConnection("Provider=Microsoft.JET.OleDb.4.0; Data Source=Bilgiler.mdb");
            //Bağlantımızı açtık
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            Image image = Image.FromFile(Application.StartupPath + "/Logo.jpg");
            pictureBox1.Image = image;
            var pos = this.PointToScreen(label7.Location);
            pos = pictureBox1.PointToClient(pos);
            textBox1.MaxLength = 11;
            textBox3.MaxLength = 11;
            label7.Parent = pictureBox1;
            label7.BackColor = Color.Transparent;
            label7.Text = "Rent a car yani araç kiralama hizmeti hem bireyler hem de kurumlar için giderek daha fazla önem kazanan bir ihtiyaç haline geldi. Turizm algısının gelişmesi ve havalimanı sayısının artması ile hem uzun hem de kısa dönem araç kiralamada talep arttı. Cağır bu noktada kullanıcıya kiralık araba seçimini kolaylaştıran bir hizmet sağlıyor. Çok sayıda yerel ve global markanın sunduğu ayrıcalıklı hizmetler arasından seçim yapmak kullanıcı için oldukça zorlayıcı. Bununla beraber en uygun fiyatlı araç kiralama seçeneğinin, hangi marka ya da ofis tarafından sunulduğunu araştırmak da çok zahmetli olabiliyor. İşte tam da bu ihtiyacı karşılama fikrinden yola çıkan Çağır sistemli ve güvenli bir yapı ile sunduğu rent a car fiyat karşılaştırma sistemiyle kullanıcının aradığını hızlı ve zahmetsizce bulmasına odaklanıyor. Çağır ile araç kiralamanın kolayı var! Araç kiralamak ne kolaymış mottosuyla hizmet veren Çağır kiralık araç ihtiyaçlarınıza uygun rent a car fiyatlarını kıyaslayabileceğiniz bir yapıyla çözüm getiriyor.";
            label7.MaximumSize = new Size(1000, 0);
            label7.AutoSize = true;
            //Hakkımızda kısmına logomuzu yerleştirdik ardından da strechleyeyek bütün alanı kaplamasını sağladık. label'imizin logonunun arkaplan rengini alması için label'imizi transparent yaptık.
            PrivateFontCollection ozelFont = new PrivateFontCollection();
            ozelFont.AddFontFile("StintUltraCondensed-Regular.ttf");
            label7.Font = new Font(ozelFont.Families[0], 20, FontStyle.Regular);
            //Hakkımızda kısmı daha güzel gözüksün diye google fonts'tan font eklemesi gerçekleştirdik

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = 0;
            cmd = new OleDbCommand("Select Tc_No From Kullanici", baglantim);
            baglantim.Open();
            oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                if (oku["Tc_No"].ToString() == textBox3.Text)
                {
                    i++;
                }
            }
            //Eğer bu Tc_No'yu bulabilirse daha önceden kayıtlı demektir ve o yüzden aynı no ile kayıt olunamaz bunu sağlamak için flag yöntemini kullanıyoruz.
            oku.Close();
            baglantim.Close();
            if(i == 0)
            {
                if (textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
                {
                    MessageBox.Show("Lütfen Tüm Boslukları Doldurunuz");
                } 
                //Tc No doğruluğunu kontrol ediyoruz
                else if (textBox3.Text.Length != 11)
                {
                    MessageBox.Show("Lütfen Gecerli Tc No Giriniz");
                } 
                else if ((textBox4.Text != textBox5.Text))
                {
                    MessageBox.Show("Sifreler Uyusmuyor");
                }
                //Sifrelerin uyuşmasını kontrol ediyoruz
                else
                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand ekle = new OleDbCommand("insert into Kullanici (Tc_No,Sifre,Sifre_Ipucu) values('" + textBox3.Text + "','" + textBox4.Text + "','" + textBox6.Text + "')", baglantim);
                        ekle.ExecuteNonQuery();
                        baglantim.Close();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                        //Kullanıcıdan aldığımız bilgileri access veri tabanımıza giriyoruz
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Hata meydana geldi");
                        baglantim.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Bu Tc Zaten Kayıtlı Durumda");
                //Flag yukarıdaysa daha önce kayıtlı bir no girilmiş demektir.
            }

        }

        public static string user;
        public static string pass;
        //Diğer formlardan da erişilsin diye bu iki değişkeni static tanımlıyoruz

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Lütfen Tüm Boslukları Doldurunuz");
            }
            else if (textBox1.Text.Length != 11)
            {
                MessageBox.Show("Lütfen Gecerli Tc No Giriniz");
            }
            else
            {
                user = textBox1.Text;
                pass = textBox2.Text;
                cmd = new OleDbCommand("Select * From Kullanici where Tc_No='" + user + "'", baglantim);
                baglantim.Open();
                oku = cmd.ExecuteReader();

                if (oku.Read())
                {
                    //Tc no bulunduysa işlemlere devam edilir ama bulunamadıysa şifre kontrolü yerine öyle bir numara kayıtlı değil mesajı gösterilir.
                    cmd = new OleDbCommand("Select Admin From Kullanici where Tc_No='" + user + "' AND Sifre='" + pass + "'", baglantim);
                    oku = cmd.ExecuteReader();
                    if (oku.Read())
                    {
                        //Eğer bu no ve bu şifrede kayıtlı birisi bulunursa bu if bloğu çalışır çalışmazsa hatalı demektir çünkü no zaten kayıtlı
                        MessageBox.Show("Giris Basarili");
                        if (Convert.ToBoolean(oku["Admin"]) == true)
                        {
                            this.Hide();
                            Form3 Frm3 = new Form3();
                            Frm3.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            this.Hide();
                            Form2 Frm2 = new Form2();
                            Frm2.ShowDialog();
                            this.Close();
                        }
                        //Eğer admin yetkisine sahipse admin formuna değilse kullanıcı formuna yönlendirilir
                    }
                    else
                    {
                        MessageBox.Show("Sifre Hatali");
                        //Eğer bu no ve bu şifrede kayıtlı birisi bulunursa bu if bloğu çalışır çalışmazsa hatalı demektir çünkü no zaten kayıtlı
                    }
                }
                else
                {
                    MessageBox.Show("Böyle Bir Kullanıcı Kayıtlı Değil");
                    //Tc no bulunduysa işlemlere devam edilir ama bulunamadıysa şifre kontrolü yerine öyle bir numara kayıtlı değil mesajı gösterilir.
                }
                oku.Close();
                baglantim.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Şirfesini Unuttuğunuz Hesabın Tc No'su Yukarıda Girili Olmalıdır");
            }
            else if (textBox1.Text.Length != 11)
            {
                MessageBox.Show("Lütfen Gecerli Tc No Giriniz");
            }
            try
            {
                user = textBox1.Text;
                cmd = new OleDbCommand("Select Sifre_Ipucu From Kullanici where Tc_No='" + user + "'", baglantim);
                baglantim.Open();
                oku = cmd.ExecuteReader();
                while (oku.Read())
                {
                    MessageBox.Show("Sifre Ipucu: "+oku[0].ToString());
                }
                oku.Close();
                baglantim.Close();
                //Kullanıcı şifresini unuttuysa oluştururken eklediği şifre ipucunu şirefmi unuttum kısmından öğrenebilir
            }
            catch (Exception)
            {
                MessageBox.Show("Hata meydana geldi");
                baglantim.Close();
            }
        }

    }
}
