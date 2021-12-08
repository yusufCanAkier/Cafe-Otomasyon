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
using DevExpress.XtraCharts;

namespace CamliCafeOtomasyon
{
    public partial class Anasayfa : DevExpress.XtraEditors.XtraForm
    {
        DbBaglanti dbBaglanti = new DbBaglanti();
        List<SimpleButton> buttonlar;
        Urun urun;
        bool tabPage1Changing = true;
        bool tabPage2Changing = true;
        public Anasayfa()
        {
            InitializeComponent();
        }

        private void Anasayfa_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            buttonlar = tabNavigationPage1.Controls.OfType<SimpleButton>().ToList();
            tabNavigationPage1.Controls.OfType<GroupBox>().ToList().ForEach(groupBox =>
            buttonlar = buttonlar.Concat(groupBox.Controls.OfType<SimpleButton>().ToList()).ToList());
            buttonlar.ForEach(sButton => sButton.Click += HesapGetir);

            dbBaglanti.KategoriGetir().ForEach(item =>
            {
                comboBoxEdit1.Properties.Items.Add(item);
                comboBoxEdit2.Properties.Items.Add(item);
            });
            comboBoxEdit3.Enabled = false;
            dbBaglanti.UrunGetir().ForEach(item =>
            {
                listBoxControl1.Items.Add(item.GetKategori() + " - " + item.GetAd() + " - " + item.GetFiyat() + " TL");
            });

            dateEdit1.EditValue = DateTime.Today;
            comboBoxEdit6.Enabled = false;

            dbBaglanti.GecmisYilGetir().ForEach(item => comboBoxEdit5.Properties.Items.Add(item));
        }

        public void HesapGetir(object sender, EventArgs e)
        {
            try
            {
                AdisyonHesap adisyonHesap = new AdisyonHesap()
                {
                    formAnasayfa = this,
                    masaId = buttonlar.IndexOf((SimpleButton)sender),
                    dbBaglanti = this.dbBaglanti
                };
                this.Hide();
                adisyonHesap.ShowDialog();
            }
            catch(Exception err)
            {
                Console.WriteLine(err);
                MessageBox.Show("Sorun oluştu! Bu sayfa görüntülenemiyor.");
                Application.Exit();
            }
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit3.Enabled = true;
            comboBoxEdit3.Properties.Items.Clear();
            comboBoxEdit3.Text = "";
            dbBaglanti.UrunGetirKategori(comboBoxEdit2.Text).ForEach(item => comboBoxEdit3.Properties.Items.Add(item.GetAd().ToString()));
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(textEdit1.Text) && !string.IsNullOrWhiteSpace(comboBoxEdit1.Text) && !string.IsNullOrWhiteSpace(textEdit2.Text))
            {
                if(dbBaglanti.UrunEkle(textEdit1.Text, comboBoxEdit1.Text, Convert.ToDouble(textEdit2.Text)))
                {
                    listBoxControl1.Items.Add(GrowfirstCharacter(comboBoxEdit1.Text.ToLower()) + " - " + GrowfirstCharacter(textEdit1.Text.ToLower()) + " - " + textEdit2.Text + " TL");
                    tabPage1Changing = true;
                }
            }
            comboBoxEdit1.Text = "";
            textEdit1.Text = "";
            textEdit2.Text = "";
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBoxEdit3.Text))
            {
                if (dbBaglanti.UrunSil(comboBoxEdit3.Text))
                {
                    listBoxControl1.Items.Cast<string>().ToList().ForEach(item =>
                    {
                        if (item.Contains(comboBoxEdit3.Text))
                            listBoxControl1.Items.Remove(item);
                    });
                }
            }
            comboBoxEdit3.Text = "";
        }

        private void textEdit2_Properties_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tabPane1_SelectedPageChanged(object sender, DevExpress.XtraBars.Navigation.SelectedPageChangedEventArgs e)
        {
            if (tabPage1Changing && tabPane1.SelectedPageIndex == 1)
            {
                comboBoxEdit1.Properties.Items.Clear();
                comboBoxEdit2.Properties.Items.Clear();
                listBoxControl1.Items.Clear();
                dbBaglanti.KategoriGetir().ForEach(item =>
                {
                    comboBoxEdit1.Properties.Items.Add(item);
                    comboBoxEdit2.Properties.Items.Add(item);
                });
                comboBoxEdit3.Enabled = false;
                dbBaglanti.UrunGetir().ForEach(item =>
                {
                    listBoxControl1.Items.Add(item.GetKategori() + " - " + item.GetAd() + " - " + item.GetFiyat() + " TL");
                });
                tabPage1Changing = false;
            }

            if(tabPage2Changing && tabPane1.SelectedPageIndex == 2)
            {
                textEdit3.Enabled = false;
                textEdit5.Enabled = false;
                comboBoxEdit4.Enabled = false;
                listBoxControl2.Items.Clear();
                simpleButton22.Enabled = false;
                dbBaglanti.UrunGetir().ForEach(item =>
                {
                    listBoxControl2.Items.Add(item.GetKategori() + " - " + item.GetAd() + " - " + item.GetFiyat() + " TL");
                });
                dbBaglanti.KategoriGetir().ForEach(item => comboBoxEdit4.Properties.Items.Add(item));
                tabPage2Changing = false;
            }
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            if(listBoxControl2.SelectedIndex != -1)
            {
                textEdit3.Enabled = true;
                textEdit5.Enabled = true;
                comboBoxEdit4.Enabled = true;
                simpleButton21.Enabled = false;
                simpleButton22.Enabled = true;

                urun = dbBaglanti.UrunGetir()[listBoxControl2.SelectedIndex];

                textEdit3.Text = urun.GetAd();
                textEdit5.Text = urun.GetFiyat().ToString();
                comboBoxEdit4.Text = urun.GetKategori();
                
                listBoxControl2.Enabled = false;
            }
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(textEdit3.Text) && !string.IsNullOrWhiteSpace(textEdit5.Text) && !string.IsNullOrWhiteSpace(comboBoxEdit4.Text))
            {
                if(dbBaglanti.UrunGuncelle(urun.GetId(), textEdit3.Text, comboBoxEdit4.Text, Convert.ToDouble(textEdit5.Text)))
                {
                    textEdit3.Enabled = false;
                    textEdit5.Enabled = false;
                    comboBoxEdit4.Enabled = false;
                    simpleButton21.Enabled = true;
                    simpleButton22.Enabled = false;

                    listBoxControl2.Items[listBoxControl2.SelectedIndex] = comboBoxEdit4.Text + " - " + textEdit3.Text + " - " + textEdit5.Text + " TL";
                    listBoxControl2.Enabled = true;
                    tabPage2Changing = true;
                    tabPage1Changing = true;
                }
            }

            textEdit3.Text = "";
            textEdit5.Text = "";
            comboBoxEdit4.Text = "";
        }

        private void textEdit5_KeyPress(object sender, KeyPressEventArgs e)
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

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            double nakitTutar = 0;
            double kartTutar = 0;
            double toplamTutar = 0;
            listBoxControl3.Items.Clear();

            if (!string.IsNullOrWhiteSpace(dateEdit1.Text))
            {
                dbBaglanti.GecmisGetir(Convert.ToInt32(dateEdit1.Text.Split('.')[0]),
                                       Convert.ToInt32(dateEdit1.Text.Split('.')[1]),
                                       Convert.ToInt32(dateEdit1.Text.Split('.')[2]))
                                       .ForEach(item => {
                        listBoxControl3.Items.Add(item.Split(' ')[0] + " TL - " + item.Split(' ')[1] + " ile ödenmiştir.");
                        if(item.Split(' ')[1] == "Nakit")
                        {
                            nakitTutar += Convert.ToDouble(item.Split(' ')[0]);
                        }
                        else
                        {
                            kartTutar += Convert.ToDouble(item.Split(' ')[0]);
                        }
                        toplamTutar += Convert.ToDouble(item.Split(' ')[0]);
                    });
                labelControl12.Text = "Nakit Ödeme : " + nakitTutar + " TL";
                labelControl13.Text = "Kredi Kartı : " + kartTutar + " TL";
                labelControl14.Text = "Toplam Ciro : " + toplamTutar + " TL";
            }
        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            listBoxControl4.Items.Clear();
            chartControl1.Series.Clear();
            if (!string.IsNullOrWhiteSpace(comboBoxEdit6.Text))
            {
                List<string> seriesList;
                Series series = new Series("Günlük Gelir", ViewType.StackedArea);
                dbBaglanti.GecmisTutarGetir(Convert.ToInt32(comboBoxEdit6.Text), Convert.ToInt32(comboBoxEdit5.Text)).ForEach(item =>
                {
                    listBoxControl4.Items.Add(item.Split(' ')[1] + '.' + comboBoxEdit6.Text + '.' + comboBoxEdit5.Text + " - " + item.Split(' ')[0] + " TL");
                });

                seriesList = dbBaglanti.GecmisTutarToplamGetir(Convert.ToInt32(comboBoxEdit6.Text), Convert.ToInt32(comboBoxEdit5.Text));

                int seriesListIndex = 0;
                for(int i = 1; i <= 31; i++)
                {
                    if(Convert.ToInt32(seriesList[seriesListIndex].Split(' ')[1]) != i)
                    {
                        series.Points.Add(new SeriesPoint(i + ". Gün", 0));
                    }
                    else
                    {
                        series.Points.Add(new SeriesPoint(i + ". Gün", Convert.ToDouble(seriesList[seriesListIndex].Split(' ')[0])));
                        if(seriesListIndex != seriesList.Count() - 1)
                        {
                            seriesListIndex++;
                        }
                    }
                }
                chartControl1.Series.Add(series);
            }
        }

        private void comboBoxEdit5_Properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit6.Enabled = true;
            comboBoxEdit6.Properties.Items.Clear();

            dbBaglanti.GecmisAyGetir(Convert.ToInt32(comboBoxEdit5.Text)).ForEach(item => comboBoxEdit6.Properties.Items.Add(item));
        }
    }
}