using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamliCafeOtomasyon
{
    public class Adisyon
    {
        int masaId;
        int urunAdedi;
        int urunId;
        string urunAdi;
        int adisyonId;

        public Adisyon(int masaId, int urunAdedi, int urunId, string urunAdi, int adisyonId)
        {
            this.masaId = masaId;
            this.urunId = urunId;
            this.urunAdedi = urunAdedi;
            this.urunAdi = urunAdi;
            this.adisyonId = adisyonId;
        }

        public void SetMasaId(int masaId)
        {
            this.masaId = masaId;
        }

        public int GetMasaId()
        {
            return masaId;
        }

        public void SetAdisyonId(int adisyonId)
        {
            this.adisyonId = adisyonId;
        }

        public int GetAdisyonId()
        {
            return adisyonId;
        }

        public void SetUrunId(int urunId)
        {
            this.urunId = urunId;
        }

        public int GetUrunId()
        {
            return urunId;
        }

        public void SetUrunAdedi(int urunAdedi)
        {
            this.urunAdedi = urunAdedi;
        }

        public int GetUrunAdedi()
        {
            return urunAdedi;
        }

        public void SetUrunAdi(string urunAdi)
        {
            this.urunAdi = urunAdi;
        }

        public string GetUrunAdi()
        {
            return urunAdi;
        }
    }
}
