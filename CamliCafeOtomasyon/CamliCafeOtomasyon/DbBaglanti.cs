using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CamliCafeOtomasyon
{
    public class DbBaglanti
    {
        MySqlConnection mySqlConnection;
        MySqlCommand mySqlCommand;
        MySqlDataReader mySqlDataReader;
        string baglantiMetni = "Server=localhost;Database=cafesimulation;user=root;Pwd=;SslMode=none";

        public MySqlConnection BaglantiOlustur()
        {
            if (mySqlConnection == null)
                mySqlConnection = new MySqlConnection(baglantiMetni);

            return mySqlConnection;
        }

        public List<string> KategoriGetir()
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("SELECT kategori FROM urunler GROUP BY kategori", BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<string> kategoriler = new List<string>();
            while (mySqlDataReader.Read())
            {
                kategoriler.Add(mySqlDataReader[0].ToString());
            }
            BaglantiOlustur().Close();

            return kategoriler;
        }

        public List<Urun> UrunGetirKategori(string kategori)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("Select * FROM urunler WHERE kategori ='" + kategori + "'", BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<Urun> urunler = new List<Urun>();
            while (mySqlDataReader.Read())
            {
                urunler.Add(new Urun(Convert.ToInt32(mySqlDataReader[0]), mySqlDataReader[1].ToString(), mySqlDataReader[2].ToString(), Convert.ToDouble(mySqlDataReader[3])));
            }
            BaglantiOlustur().Close();

            return urunler;
        }

        public List<Urun> UrunGetir()
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("Select * FROM urunler ORDER BY kategori ASC", BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<Urun> urunler = new List<Urun>();
            while (mySqlDataReader.Read())
            {
                urunler.Add(new Urun(Convert.ToInt32(mySqlDataReader[0]), 
                                     mySqlDataReader[1].ToString(), 
                                     mySqlDataReader[2].ToString(), 
                                     Convert.ToDouble(mySqlDataReader[3])));
            }
            BaglantiOlustur().Close();

            return urunler;
        }

        public List<Urun> UrunGetir(int id)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("Select * FROM urunler WHERE id=" + id, BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<Urun> urunler = new List<Urun>();
            while (mySqlDataReader.Read())
            {
                urunler.Add(new Urun(Convert.ToInt32(mySqlDataReader[0]),
                                     mySqlDataReader[1].ToString(),
                                     mySqlDataReader[2].ToString(),
                                     Convert.ToDouble(mySqlDataReader[3])));
            }
            BaglantiOlustur().Close();

            return urunler;
        }

        public bool UrunEkle(string urunAdi, string urunKategori, double urunFiyat)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("INSERT INTO urunler(adi,kategori,fiyat) VALUES('" + GrowfirstCharacter(urunAdi.ToLower()) + "','" + GrowfirstCharacter(urunKategori.ToLower()) + "'," + urunFiyat.ToString().Replace(',', '.') + ")", BaglantiOlustur());

            mySqlCommand.Connection = BaglantiOlustur();
            if(mySqlCommand.ExecuteNonQuery() != 0)
            {
                BaglantiOlustur().Close();
                MessageBox.Show("Ürün başarıyla veritabanına eklendi!");
                return true;
            }

            BaglantiOlustur().Close();
            MessageBox.Show("Ürün veritabanına eklenirken sorun oluştu");
            return false;
        }

        public bool UrunSil(string adi)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("DELETE FROM urunler WHERE adi='" + GrowfirstCharacter(adi.ToLower()) + "'", BaglantiOlustur());

            if (mySqlCommand.ExecuteNonQuery() != 0)
            {
                BaglantiOlustur().Close();
                MessageBox.Show("Ürün başarıyla veritabanından silindi!");
                return true;
            }
            BaglantiOlustur().Close();
            MessageBox.Show("Ürün veritabanından silinirken sorun oluştu");
            return false;
        }

        public bool UrunGuncelle(int urunId, string urunAdi, string urunKategori, double urunFiyat)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("UPDATE urunler SET adi = '" + GrowfirstCharacter(urunAdi.ToLower()) + "', kategori = '" + GrowfirstCharacter(urunKategori.ToLower()) + "', fiyat = " + urunFiyat.ToString().Replace(',','.') + " WHERE id = " + urunId, BaglantiOlustur());

            if (mySqlCommand.ExecuteNonQuery() != 0)
            {
                BaglantiOlustur().Close();
                MessageBox.Show("Ürün başarıyla güncellendi!");
                return true;
            }

            BaglantiOlustur().Close();
            MessageBox.Show("Ürün güncellenirken sorun oluştu!");
            return false;
        }

        public List<string> GecmisGetir(int gun, int ay, int yil)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("Select tutar,odeme_tipi FROM gecmis WHERE tarih_gun=" + gun + " AND tarih_ay=" + ay + " AND tarih_yil=" + yil, BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<string> gecmisList = new List<string>();
            while (mySqlDataReader.Read())
            {
                gecmisList.Add(mySqlDataReader[0].ToString() + " " + mySqlDataReader[1].ToString());
            }
            BaglantiOlustur().Close();
            return gecmisList;
        }

        public List<int> GecmisYilGetir()
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("SELECT tarih_yil FROM gecmis GROUP BY tarih_yil", BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<int> yillar = new List<int>();
            while (mySqlDataReader.Read())
            {
                yillar.Add(Convert.ToInt32(mySqlDataReader[0]));
            }
            BaglantiOlustur().Close();

            return yillar;
        }

        public List<int> GecmisAyGetir(int yil)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("SELECT tarih_ay FROM gecmis WHERE tarih_yil=" + yil + " GROUP BY tarih_ay", BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<int> aylar = new List<int>();
            while (mySqlDataReader.Read())
            {
                aylar.Add(Convert.ToInt32(mySqlDataReader[0]));
            }
            BaglantiOlustur().Close();

            return aylar;
        }

        public List<string> GecmisTutarGetir(int ay, int yil)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("SELECT tutar,tarih_gun FROM gecmis WHERE tarih_ay=" + ay + " AND tarih_yil=" + yil, BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<string> gecmisTutarList = new List<string>();
            while (mySqlDataReader.Read())
            {
                gecmisTutarList.Add(mySqlDataReader[0] + " " + mySqlDataReader[1]);
            }
            BaglantiOlustur().Close();

            return gecmisTutarList;
        }

        public List<string> GecmisTutarToplamGetir(int ay, int yil)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("Select SUM(tutar),tarih_gun FROM gecmis WHERE tarih_ay= " + ay + " AND tarih_yil= " + yil + " GROUP BY tarih_gun", BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            List<string> gecmisTutarList = new List<string>();
            while (mySqlDataReader.Read())
            {
                gecmisTutarList.Add(mySqlDataReader[0] + " " + mySqlDataReader[1]);
            }
            BaglantiOlustur().Close();

            return gecmisTutarList;
        }

        public void AdisyonaUrunEkle(int masaId, int urunId, int urunAdedi)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand();
            string komutMetni;

            if (AdisyonSayisiGetir(masaId, urunId))
            {
                komutMetni = "UPDATE adisyon SET urun_adedi=urun_adedi +" + urunAdedi + " WHERE masa_id=" + masaId + " AND urun_id=" + urunId;
            }
            else
            {
                komutMetni = "INSERT INTO adisyon(masa_id,urun_id,urun_adedi) VALUES(" + masaId + "," + urunId + "," + urunAdedi + ")";
            }
            BaglantiOlustur().Close();
            BaglantiOlustur().Open();
            mySqlCommand.CommandText = komutMetni;
            mySqlCommand.Connection = BaglantiOlustur();
            mySqlCommand.ExecuteNonQuery();
            BaglantiOlustur().Close();
        }

        public bool AdisyonSayisiGetir(int masaId, int urunId)
        {
            mySqlCommand = new MySqlCommand();

            string komutMetni = "SELECT COUNT(*) FROM adisyon WHERE adisyon.masa_id=" + masaId + " AND adisyon.urun_id=" + urunId;

            mySqlCommand.CommandText = komutMetni;
            mySqlCommand.Connection = BaglantiOlustur();

            mySqlDataReader = mySqlCommand.ExecuteReader();
            bool sorguDoluMu = false;
            while (mySqlDataReader.Read())
            {
                if (Convert.ToInt32(mySqlDataReader[0]) != 0)
                    sorguDoluMu = true;
            }
            return sorguDoluMu;
        }

        public List<Adisyon> AdisyonGetir(int masaId)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("SELECT adisyon.masa_id,adisyon.urun_adedi,adisyon.urun_id,urunler.adi,adisyon.id FROM adisyon INNER JOIN urunler ON adisyon.urun_id=urunler.id WHERE adisyon.masa_id=" + masaId, BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();
            List<Adisyon> adisyonList = new List<Adisyon>();

            while (mySqlDataReader.Read())
            {
                adisyonList.Add(new Adisyon(Convert.ToInt32(mySqlDataReader[0]), Convert.ToInt32(mySqlDataReader[1]), Convert.ToInt32(mySqlDataReader[2]), mySqlDataReader[3].ToString(), Convert.ToInt32(mySqlDataReader[4])));
            }
            BaglantiOlustur().Close();

            return adisyonList;
        }

        public void AdisyonSil(int adisyonId)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("DELETE FROM adisyon WHERE id=" + adisyonId, BaglantiOlustur());
            mySqlCommand.ExecuteNonQuery();
            BaglantiOlustur().Close();
        }

        public double MasaFiyatGetir(int masaId)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("SELECT fatura FROM masalar WHERE id=" + masaId, BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();
            double fatura = 0;
            while (mySqlDataReader.Read())
            {
                fatura = Convert.ToDouble(mySqlDataReader[0]);
            }
            BaglantiOlustur().Close();

            return fatura;
        }

        public double UrunFiyatGetir(string urunAdi)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("SELECT fiyat FROM urunler WHERE adi = '" + urunAdi + "'", BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            double fiyat = 0;
            while (mySqlDataReader.Read())
            {
                fiyat = Convert.ToDouble(mySqlDataReader[0]);
            }
            BaglantiOlustur().Close();
            return fiyat;
        }

        public double UrunFiyatGetir(int urunId)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("SELECT fiyat FROM urunler WHERE id = " + urunId, BaglantiOlustur());
            mySqlDataReader = mySqlCommand.ExecuteReader();

            double fiyat = 0;
            while (mySqlDataReader.Read())
            {
                fiyat = Convert.ToDouble(mySqlDataReader[0]);
            }
            BaglantiOlustur().Close();
            return fiyat;
        }

        public void FaturaGuncelle(int masaId, double fatura)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("UPDATE masalar SET fatura=" + fatura.ToString().Replace(',','.') + " WHERE id=" + masaId, BaglantiOlustur());
            mySqlCommand.ExecuteNonQuery();
            BaglantiOlustur().Close();
        }

        public void AdisyonGuncelle(int adisyonId, double urunAdedi)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("UPDATE adisyon SET urun_adedi=" + urunAdedi + " WHERE id=" + adisyonId, BaglantiOlustur());
            mySqlCommand.ExecuteNonQuery();
            BaglantiOlustur().Close();
        }

        public void MasaKapat(int masaId, string odemeTipi)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand();

            string komutMetni = "UPDATE masalar SET fatura=0 WHERE id=" + masaId;

            mySqlCommand.CommandText = komutMetni;
            mySqlCommand.Connection = BaglantiOlustur();

            mySqlCommand.ExecuteNonQuery();
            BaglantiOlustur().Close();
            BaglantiOlustur().Open();

            komutMetni = "SELECT adisyon.urun_adedi,urunler.fiyat FROM adisyon INNER JOIN urunler ON adisyon.urun_id=urunler.id WHERE adisyon.masa_id = " + masaId;
            mySqlCommand.CommandText = komutMetni;
            mySqlDataReader = mySqlCommand.ExecuteReader();

            double tutar = 0;
            while (mySqlDataReader.Read())
            {
                tutar += Convert.ToDouble(mySqlDataReader[0]) * Convert.ToDouble(mySqlDataReader[1]);
            }
            BaglantiOlustur().Close();
            BaglantiOlustur().Open();

            komutMetni = "INSERT INTO gecmis(tarih_gun,tarih_ay,tarih_yil,tutar,odeme_tipi) VALUES('" + DateTime.Now.Day.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Year.ToString() + "'," + tutar.ToString().Replace(',','.') + ",'" + odemeTipi + "')";

            mySqlCommand.CommandText = komutMetni;
            mySqlCommand.ExecuteNonQuery();

            BaglantiOlustur().Close();
            BaglantiOlustur().Open();

            komutMetni = "DELETE FROM adisyon WHERE masa_id=" + masaId;

            mySqlCommand.CommandText = komutMetni;
            mySqlCommand.ExecuteNonQuery();
            BaglantiOlustur().Close();
        }

        public bool GecmisEkle(string odemeTipi, double tutar)
        {
            BaglantiOlustur().Open();
            mySqlCommand = new MySqlCommand("INSERT INTO gecmis(tarih_gun, tarih_ay, tarih_yil, tutar, odeme_tipi) VALUES('" + DateTime.Now.Day.ToString() + "', '" + DateTime.Now.Month.ToString() + "', '" + DateTime.Now.Year.ToString() + "', " + tutar.ToString().Replace(',','.') + ", '" + odemeTipi + "')", BaglantiOlustur());

            mySqlCommand.Connection = BaglantiOlustur();
            if (mySqlCommand.ExecuteNonQuery() != 0)
            {
                BaglantiOlustur().Close();
                return true;
            }

            BaglantiOlustur().Close();
            return false;
        }

        public string GrowfirstCharacter(string Text)
        {
            string yeniKelime = "";
            if (Text.Contains(" "))
            {
                string[] words = Text.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    yeniKelime += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1, words[i].Length - 1).ToLower() + " ";
                }

                Text = yeniKelime.TrimEnd(' ');
            }
            else
            {
                Text = Text.Substring(0, 1).ToUpper() + Text.Substring(1, Text.Length - 1).ToLower();
            }

            return Text;
        }
    }
}

