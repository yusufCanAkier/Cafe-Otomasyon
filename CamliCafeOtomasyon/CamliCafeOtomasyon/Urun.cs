using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamliCafeOtomasyon
{
    public class Urun
    {
        int id;
        string ad;
        string kategori;
        double fiyat;

        public Urun(int id, string ad, string kategori, double fiyat)
        {
            this.id = id;
            this.ad = ad;
            this.kategori = kategori;
            this.fiyat = fiyat;
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        public int GetId()
        {
            return id;
        }

        public void SetAd(string ad)
        {
            this.ad = ad;
        }

        public string GetAd()
        {
            return ad;
        }

        public void SetKategori(string kategori)
        {
            this.kategori = kategori;
        }

        public string GetKategori()
        {
            return kategori;
        }

        public void SetFiyat(double fiyat)
        {
            this.fiyat = fiyat;
        }

        public double GetFiyat()
        {
            return fiyat;
        }
    }
}
