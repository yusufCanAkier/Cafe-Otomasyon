using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CamliCafeOtomasyon
{
    public partial class AdisyonHesap : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm formAnasayfa = null;
        public XtraForm faturaForm = new FaturaForm();
        public int masaId;
        double toplamTutar;
        public DbBaglanti dbBaglanti;
        List<Adisyon> adisyon;
        List<Urun> urunler;
        List<Adisyon> adisyonFatura = new List<Adisyon>();
        SimpleButton buttonKartEvent;
        SimpleButton buttonNakitEvent;

        public AdisyonHesap()
        {
            InitializeComponent();
        }

        private void AdisyonHesap_Load(object sender, EventArgs e)
        {
            dbBaglanti.KategoriGetir().ForEach(item => comboBoxEdit1.Properties.Items.Add(item));
            AdisyonYukle();
            MasaFiyatYukle();
        }

        public XtraForm GetForm()
        {
            return faturaForm;
        }

        public void MasaFiyatYukle()
        {
            labelControl5.Text = dbBaglanti.MasaFiyatGetir(masaId).ToString("0.##") + " TL";
        }

        public void AdisyonYukle()
        {
            listBoxControl2.Items.Clear();
            adisyon = dbBaglanti.AdisyonGetir(masaId);
            adisyon.ForEach(item =>
            {
                listBoxControl2.Items.Add(item.GetUrunAdi() + " - " + item.GetUrunAdedi() + " Tane");
            });
        }

        private void AdisyonHesap_FormClosing(object sender, FormClosingEventArgs e)
        {
            GetForm().Close();
            formAnasayfa.Show();
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxControl1.Items.Clear();
            urunler = dbBaglanti.UrunGetirKategori(comboBoxEdit1.Text);
            urunler.ForEach(item => listBoxControl1.Items.Add(item.GetAd()));
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (listBoxControl2.SelectedIndex != -1 && adisyon[listBoxControl2.SelectedIndex].GetUrunAdedi() >= Convert.ToInt32(textBox1.Text))
            {
                dbBaglanti.AdisyonaUrunEkle(masaId, adisyon[listBoxControl2.SelectedIndex].GetUrunId(), - Convert.ToInt32(textBox1.Text));
                if(adisyon[listBoxControl2.SelectedIndex].GetUrunAdedi() == Convert.ToInt32(textBox1.Text))
                {
                    dbBaglanti.AdisyonSil(adisyon[listBoxControl2.SelectedIndex].GetAdisyonId());
                    dbBaglanti.FaturaGuncelle(masaId, Convert.ToDouble(labelControl5.Text.Split(' ')[0]) - dbBaglanti.UrunFiyatGetir(adisyon[listBoxControl2.SelectedIndex].GetUrunId()) * Convert.ToInt32(textBox1.Text));
                    AdisyonYukle();
                }
                else
                adisyon.ForEach(item =>
                {
                    if (item.GetUrunId() == adisyon[listBoxControl2.SelectedIndex].GetUrunId())
                    {
                        item.SetUrunAdedi(item.GetUrunAdedi() - Convert.ToInt32(textBox1.Text));
                        dbBaglanti.FaturaGuncelle(masaId, Convert.ToDouble(labelControl5.Text.Split(' ')[0]) - dbBaglanti.UrunFiyatGetir(item.GetUrunId()) * Convert.ToInt32(textBox1.Text));
                    }
                });
                listBoxControl2.Items.Clear();
                adisyon.ForEach(item => listBoxControl2.Items.Add(item.GetUrunAdi() + " - " + item.GetUrunAdedi() + " Tane"));
            }
            textBox1.Text = "1";
            MasaFiyatYukle();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int textBoxSayi = Convert.ToInt32(textBox1.Text);
            if (textBoxSayi != 1)
            {
                textBoxSayi--;
            }
            textBox1.Text = textBoxSayi.ToString();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            textBox1.Text = (Convert.ToInt32(textBox1.Text) + 1).ToString();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if(listBoxControl1.SelectedIndex != -1)
            {
                bool urunVarMi = false;
                dbBaglanti.AdisyonaUrunEkle(masaId, urunler[listBoxControl1.SelectedIndex].GetId(), Convert.ToInt32(textBox1.Text));
                listBoxControl2.Items.Clear();
                adisyon.ForEach(item =>
                {
                    if(item.GetUrunId() == urunler[listBoxControl1.SelectedIndex].GetId())
                    {
                        item.SetUrunAdedi(item.GetUrunAdedi() + Convert.ToInt32(textBox1.Text));
                        dbBaglanti.FaturaGuncelle(masaId, Convert.ToDouble(labelControl5.Text.Split(' ')[0]) + dbBaglanti.UrunFiyatGetir(item.GetUrunId()) * Convert.ToInt32(textBox1.Text));
                        urunVarMi = true;
                    }
                    listBoxControl2.Items.Add(item.GetUrunAdi() + " - " + item.GetUrunAdedi() + " Tane");
                });

                if (!urunVarMi)
                {
                    AdisyonYukle();
                    dbBaglanti.FaturaGuncelle(masaId, Convert.ToDouble(labelControl5.Text.Split(' ')[0]) + dbBaglanti.UrunFiyatGetir(adisyon[listBoxControl2.ItemCount - 1].GetUrunId()) * Convert.ToInt32(textBox1.Text));
                }
            }
            else if (listBoxControl2.SelectedIndex != -1)
            {
                dbBaglanti.AdisyonaUrunEkle(masaId, adisyon[listBoxControl2.SelectedIndex].GetUrunId(), Convert.ToInt32(textBox1.Text));
                adisyon.ForEach(item =>
                {
                    if (item.GetUrunId() == adisyon[listBoxControl2.SelectedIndex].GetUrunId())
                    {
                        item.SetUrunAdedi(item.GetUrunAdedi() + Convert.ToInt32(textBox1.Text));
                        dbBaglanti.FaturaGuncelle(masaId, Convert.ToDouble(labelControl5.Text.Split(' ')[0]) + dbBaglanti.UrunFiyatGetir(item.GetUrunId()) * Convert.ToInt32(textBox1.Text));
                    }
                });
                listBoxControl2.Items.Clear();
                adisyon.ForEach(item => listBoxControl2.Items.Add(item.GetUrunAdi() + " - " + item.GetUrunAdedi() + " Tane"));
            }
            textBox1.Text = "1";
            MasaFiyatYukle();
        }

        private void listBoxControl1_DoubleClick(object sender, EventArgs e)
        {
            listBoxControl1.SelectedIndex = -1;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void listBoxControl2_DoubleClick(object sender, EventArgs e)
        {
            listBoxControl2.SelectedIndex = -1;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if(listBoxControl2.SelectedIndex != -1)
            {
                bool urunVarMi = false;
                if (GetForm().Visible == false)
                {
                    adisyonFatura = new List<Adisyon>();
                    toplamTutar = 0;
                }
                GetForm().Show();

                adisyonFatura.ForEach(item => { 
                    if(item.GetUrunId() == adisyon[listBoxControl2.SelectedIndex].GetUrunId())
                    {
                        item.SetUrunAdedi(item.GetUrunAdedi() + Convert.ToInt32(textBox1.Text));
                        urunVarMi = true;
                    }
                });
                if (!urunVarMi)
                {
                    adisyonFatura.Add(new Adisyon(masaId,
                                                  Convert.ToInt32(textBox1.Text),
                                                  adisyon[listBoxControl2.SelectedIndex].GetUrunId(),
                                                  adisyon[listBoxControl2.SelectedIndex].GetUrunAdi(),
                                                  adisyon[listBoxControl2.SelectedIndex].GetAdisyonId()));
                }
                    
                
                GetForm().Controls.Cast<Control>().ToList().ForEach(item =>
                {
                    if (item.GetType().ToString() == "DevExpress.XtraEditors.ListBoxControl")
                    {
                        ((ListBoxControl)item).Items.Clear();
                        adisyonFatura.ForEach(iterator => ((ListBoxControl)item).Items.Add(iterator.GetUrunAdi() + " - " + iterator.GetUrunAdedi() + " Tane"));
                    }
                    if(item.GetType().ToString() == "DevExpress.XtraEditors.LabelControl" && ((LabelControl)item).Name == "labelControl2")
                    {
                        toplamTutar = 0;
                        adisyonFatura.ForEach(iterator => toplamTutar += dbBaglanti.UrunFiyatGetir(iterator.GetUrunId()) * iterator.GetUrunAdedi());
                        ((LabelControl)item).Text = toplamTutar.ToString() + " TL";
                    }
                    if(item.GetType().ToString() == "DevExpress.XtraEditors.SimpleButton" && ((SimpleButton)item).Text == "Nakit olarak ödendi.")
                    {
                        if(buttonNakitEvent == null)
                        {
                            buttonNakitEvent = (SimpleButton)item;
                            buttonNakitEvent.Click += ButtonClickNakit;
                        }
                    }
                    if (item.GetType().ToString() == "DevExpress.XtraEditors.SimpleButton" && ((SimpleButton)item).Text == "Kart ile ödendi.")
                    {
                        if (buttonKartEvent == null)
                        {
                            buttonKartEvent = (SimpleButton)item;
                            buttonKartEvent.Click += ButtonClickKart;
                        }
                    }
                });
                textBox1.Text = "1";
            }
        }

        private void ButtonClickNakit(object sender, EventArgs e)
        {
            FaturaOdeYanEkran("Nakit");
            if (adisyon == adisyonFatura)
            {
                dbBaglanti.MasaKapat(masaId, "Nakit");
            }
        }

        private void ButtonClickKart(object sender, EventArgs e)
        {
            FaturaOdeYanEkran("Kart");
            if(adisyon == adisyonFatura)
            {
                dbBaglanti.MasaKapat(masaId, "Kart");
            }
        }

        public void FaturaOdeYanEkran(string odemeYontemi)
        {
            printDocument1.Print();
            adisyonFatura.ForEach(item =>
            {
                adisyon.ForEach(iterator =>
                {
                    if(iterator.GetUrunId() == item.GetUrunId())
                    {
                        iterator.SetUrunAdedi(iterator.GetUrunAdedi() - item.GetUrunAdedi());
                        if(iterator.GetUrunAdedi() == 0)
                        {
                            dbBaglanti.AdisyonSil(iterator.GetAdisyonId());
                        }
                        else
                        {
                            dbBaglanti.AdisyonGuncelle(iterator.GetAdisyonId(), iterator.GetUrunAdedi());
                        }
                    }
                });
            });

            dbBaglanti.GecmisEkle(odemeYontemi, toplamTutar);
            AdisyonYukle();
            GetForm().Close();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
            dbBaglanti.MasaKapat(masaId, "Kart");
            this.Close();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if(xtraTabControl1.SelectedTabPageIndex == 1)
            {
                labelControl7.Text = labelControl5.Text;
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            double masaFiyat = dbBaglanti.MasaFiyatGetir(masaId);
            if(masaFiyat >= Convert.ToDouble(textEdit1.Text))
            {
                dbBaglanti.FaturaGuncelle(masaId, masaFiyat - Convert.ToDouble(textEdit1.Text));
                labelControl5.Text = dbBaglanti.MasaFiyatGetir(masaId) + " TL";
                labelControl7.Text = labelControl5.Text;
            }
            textEdit1.Text = "";
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            double masaFiyat = dbBaglanti.MasaFiyatGetir(masaId);
            if (Convert.ToDouble(textEdit2.Text) <= 100 && Convert.ToDouble(textEdit2.Text) > 0)
            {
                masaFiyat = masaFiyat - masaFiyat * Convert.ToDouble(textEdit2.Text) / 100.0;
                dbBaglanti.FaturaGuncelle(masaId, masaFiyat);
                labelControl5.Text = dbBaglanti.MasaFiyatGetir(masaId) + " TL";
                labelControl7.Text = labelControl5.Text;
            }
            textEdit2.Text = "";
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == ',') && ((sender as TextEdit).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void textEdit2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == ',') && ((sender as TextEdit).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("*******************************************************************************************************", new Font("Sans-Serif", 5, FontStyle.Regular), Brushes.Black, new Point(0, 10));

            string text = "ULAŞ SARAL";
            Font font = new Font("Sans-Serif", 12, FontStyle.Regular);
            float width = e.Graphics.MeasureString(text, font).Width;
            int boundsWidth = e.PageBounds.Width / 2;
            int pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 20));

            text = "ÇAMLI KAFE";
            font = new Font("Chiller", 26, FontStyle.Regular);
            width = e.Graphics.MeasureString(text, font).Width;
            boundsWidth = e.PageBounds.Width / 2;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 50));

            text = "T.C. 29540414290";
            font = new Font("Sans-Serif", 6, FontStyle.Regular);
            width = e.Graphics.MeasureString(text, font).Width;
            boundsWidth = e.PageBounds.Width / 2;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 100));


            text = "Çamlı Mah. Hacı Mehmet Bahattin Ulusoy Cad.";
            font = new Font("Sans-Serif", 8, FontStyle.Regular);
            width = e.Graphics.MeasureString(text, font).Width;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 130));

            text = "148/2, Of/TRABZON";
            font = new Font("Sans-Serif", 8, FontStyle.Regular);
            width = e.Graphics.MeasureString(text, font).Width;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 145));

            text = "TLF : 0538 555 96 96";
            font = new Font("Sans-Serif", 10, FontStyle.Regular);
            width = e.Graphics.MeasureString(text, font).Width;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 160));


            e.Graphics.DrawString(DateTime.Now.ToString("dd / MM / yyyy"), new Font("Sans-Serif", 6, FontStyle.Regular), Brushes.Black, new Point(20, 190));
            e.Graphics.DrawString("SAAT: " + DateTime.Now.ToString("HH : mm"), new Font("Sans-Serif", 6, FontStyle.Regular), Brushes.Black, new Point(20, 200));
            e.Graphics.DrawString("*******************************************************************************************************", new Font("Sans-Serif", 5, FontStyle.Regular), Brushes.Black, new Point(0, 220));

            text = "ADISYON";
            font = new Font("Chiller", 15, FontStyle.Regular);
            width = e.Graphics.MeasureString(text, font).Width;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 240));

            int hesapSayisi = 0;
            double toplamTutar = 0;
            adisyonFatura.ForEach(item =>
            {
                text = item.GetUrunAdi() + " x" + item.GetUrunAdedi().ToString();
                font = new Font("Sans-Serif", 10, FontStyle.Regular);
                e.Graphics.DrawString(text, font, Brushes.Black, new Point(20, 270 + hesapSayisi * 20));

                text = (dbBaglanti.UrunFiyatGetir(item.GetUrunId()) * item.GetUrunAdedi()).ToString() + " TL";
                font = new Font("Sans-Serif", 10, FontStyle.Regular);
                e.Graphics.DrawString(text, font, Brushes.Black, new Point(200, 270 + hesapSayisi * 20));
                hesapSayisi++;
                toplamTutar += dbBaglanti.UrunFiyatGetir(item.GetUrunId()) * item.GetUrunAdedi();
            });

            if (dbBaglanti.MasaFiyatGetir(masaId) < toplamTutar)
            {
                toplamTutar = dbBaglanti.MasaFiyatGetir(masaId);
            }

            e.Graphics.DrawString("*******************************************************************************************************", new Font("Sans-Serif", 5, FontStyle.Regular), Brushes.Black, new Point(0, 280 + hesapSayisi * 20));

            text = "TOPLAM TUTAR";
            font = new Font("Chiller", 15, FontStyle.Regular);
            width = e.Graphics.MeasureString(text, font).Width;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 290 + hesapSayisi * 20));

            dbBaglanti.FaturaGuncelle(masaId, Convert.ToDouble(labelControl5.Text.Split(' ')[0]) - toplamTutar);
            labelControl5.Text = dbBaglanti.MasaFiyatGetir(masaId).ToString() + " TL";
            text = toplamTutar.ToString() + " TL";
            font = new Font("Sans-Serif", 18, FontStyle.Bold);
            width = e.Graphics.MeasureString(text, font).Width;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 310 + hesapSayisi * 20));

            text = "Yine Bekleriz.";
            font = new Font("Sans-Serif", 8, FontStyle.Italic);
            width = e.Graphics.MeasureString(text, font).Width;
            pointX = boundsWidth - (int)width / 2;
            e.Graphics.DrawString(text, font, Brushes.Black, new Point(pointX, 350 + hesapSayisi * 20));
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (GetForm().Visible == false)
            {
                adisyonFatura = new List<Adisyon>();
                toplamTutar = 0;
            }
            GetForm().Show();

            adisyonFatura = adisyon;
            
            GetForm().Controls.Cast<Control>().ToList().ForEach(item =>
            {
                if (item.GetType().ToString() == "DevExpress.XtraEditors.ListBoxControl")
                {
                    ((ListBoxControl)item).Items.Clear();
                    adisyonFatura.ForEach(iterator => ((ListBoxControl)item).Items.Add(iterator.GetUrunAdi() + " - " + iterator.GetUrunAdedi() + " Tane"));
                }
                if (item.GetType().ToString() == "DevExpress.XtraEditors.LabelControl" && ((LabelControl)item).Name == "labelControl2")
                {
                    toplamTutar = 0;
                    adisyonFatura.ForEach(iterator => toplamTutar += dbBaglanti.UrunFiyatGetir(iterator.GetUrunId()) * iterator.GetUrunAdedi());
                    ((LabelControl)item).Text = dbBaglanti.MasaFiyatGetir(masaId).ToString() + " TL";
                }
                if (item.GetType().ToString() == "DevExpress.XtraEditors.SimpleButton" && ((SimpleButton)item).Name == "simpleButton6")
                {
                    if (buttonNakitEvent == null)
                    {
                        buttonNakitEvent = (SimpleButton)item;
                        buttonNakitEvent.Click += ButtonClickNakit;
                    }
                }
                if (item.GetType().ToString() == "DevExpress.XtraEditors.SimpleButton" && ((SimpleButton)item).Name == "simpleButton7")
                {
                    if (buttonKartEvent == null)
                    {
                        buttonKartEvent = (SimpleButton)item;
                        buttonKartEvent.Click += ButtonClickKart;
                    }
                }
            });
        }
    }
}