using System;
using System.Linq;

namespace YZ_ABC_Algoritmasi
{
    public class ABCAlgoritmasi
    {
        private int koloniBoyutu;
        private int maksIterasyon;
        private int limit;
        private int boyutlar;
        private double[] altSinirlar;
        private double[] ustSinirlar;
        private Random rastgele;

        public ABCAlgoritmasi(int koloniBoyutu, int maksIterasyon, int limit, int boyutlar, double[] altSinirlar, double[] ustSinirlar)
        {
            this.koloniBoyutu = koloniBoyutu;
            this.maksIterasyon = maksIterasyon;
            this.limit = limit;
            this.boyutlar = boyutlar;
            this.altSinirlar = altSinirlar;
            this.ustSinirlar = ustSinirlar;
            this.rastgele = new Random();
        }

        // Amaç fonksiyonunu tanımlayın
        private double AmacFonksiyonu(double[] cozum)
        {
            double sonuc = 0.0;
            for (int i = 0; i < boyutlar - 1; i++)
            {
                sonuc += 100 * Math.Pow(cozum[i + 1] - cozum[i] * cozum[i], 2) + Math.Pow(cozum[i] - 1, 2);
            }
            return sonuc;
        }

        // Rastgele çözüm oluşturma
        private double[] RastgeleCozumUret()
        {
            double[] cozum = new double[boyutlar];
            for (int i = 0; i < boyutlar; i++)
            {
                cozum[i] = altSinirlar[i] + rastgele.NextDouble() * (ustSinirlar[i] - altSinirlar[i]);
            }
            return cozum;
        }

        // Komşu çözüm oluşturma
        private double[] KomsuCozumUret(double[] mevcutCozum)
        {
            double[] yeniCozum = new double[boyutlar];
            Array.Copy(mevcutCozum, yeniCozum, boyutlar);
            int indeks = rastgele.Next(boyutlar);
            yeniCozum[indeks] = altSinirlar[indeks] + rastgele.NextDouble() * (ustSinirlar[indeks] - altSinirlar[indeks]);
            return yeniCozum;
        }

        public (double[] enIyiCozum, double enIyiUygunluk, double[] uygunlukGecmisi) OptimizasyonYap()
        {
            double[][] popülasyon = new double[koloniBoyutu][];
            double[] uygunluk = new double[koloniBoyutu];
            int[] denemeSayaci = new int[koloniBoyutu];
            double[] enIyiCozum = null;
            double enIyiUygunluk = double.MaxValue;
            double[] uygunlukGecmisi = new double[maksIterasyon];

            // Popülasyonu başlatma
            for (int i = 0; i < koloniBoyutu; i++)
            {
                popülasyon[i] = RastgeleCozumUret();
                uygunluk[i] = AmacFonksiyonu(popülasyon[i]);
                if (uygunluk[i] < enIyiUygunluk)
                {
                    enIyiUygunluk = uygunluk[i];
                    enIyiCozum = popülasyon[i];
                }
            }

            for (int iter = 0; iter < maksIterasyon; iter++)
            {
                // Çalışan arı aşaması
                for (int i = 0; i < koloniBoyutu; i++)
                {
                    double[] komsuCozum = KomsuCozumUret(popülasyon[i]);
                    double komsuUygunluk = AmacFonksiyonu(komsuCozum);
                    if (komsuUygunluk < uygunluk[i])
                    {
                        popülasyon[i] = komsuCozum;
                        uygunluk[i] = komsuUygunluk;
                        denemeSayaci[i] = 0;
                    }
                    else
                    {
                        denemeSayaci[i]++;
                    }
                }

                // Gözlemci arı aşaması
                double uygunlukToplami = uygunluk.Sum();
                for (int i = 0; i < koloniBoyutu; i++)
                {
                    double olasilik = uygunluk[i] / uygunlukToplami;
                    if (rastgele.NextDouble() < olasilik)
                    {
                        double[] komsuCozum = KomsuCozumUret(popülasyon[i]);
                        double komsuUygunluk = AmacFonksiyonu(komsuCozum);
                        if (komsuUygunluk < uygunluk[i])
                        {
                            popülasyon[i] = komsuCozum;
                            uygunluk[i] = komsuUygunluk;
                            denemeSayaci[i] = 0;
                        }
                        else
                        {
                            denemeSayaci[i]++;
                        }
                    }
                }

                // İzci arı aşaması
                for (int i = 0; i < koloniBoyutu; i++)
                {
                    if (denemeSayaci[i] >= limit)
                    {
                        popülasyon[i] = RastgeleCozumUret();
                        uygunluk[i] = AmacFonksiyonu(popülasyon[i]);
                        denemeSayaci[i] = 0;
                    }
                }

                // En iyi çözümü güncelleme
                for (int i = 0; i < koloniBoyutu; i++)
                {
                    if (uygunluk[i] < enIyiUygunluk)
                    {
                        enIyiUygunluk = uygunluk[i];
                        enIyiCozum = popülasyon[i];
                    }
                }

                // Yakınsama grafiği için uygunluk geçmişini kaydetme
                uygunlukGecmisi[iter] = enIyiUygunluk;
            }

            return (enIyiCozum, enIyiUygunluk, uygunlukGecmisi);
        }
    }
}
