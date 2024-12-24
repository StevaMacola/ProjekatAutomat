using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace Projekat
{
    public partial class Form1 : Form
    {
        protected Kasa kasa;
        private Automat automat;
        private PictureBox[,] ekran;
        private Dictionary<string, Artikal> artikli;

        public Form1()
        {
            InitializeComponent();
            kasa = new Kasa();

            artikli = new Dictionary<string, Artikal>() {
                {"kokakola",   new Artikal(1001, "Kokakola", "2026-12-12", 60, null, 0, Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slike", "kokakola.jfif"))) },
                {"fanta",   new Artikal(1002, "Fanta", "2026-12-12", 60, null, 0, Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slike", "fanta.jfif"))) },
                {"hidroaktiv",   new Artikal(1003, "Hidroaktiv", "2026-12-12", 75, null, 0, Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slike", "hidroaktiv.jfif"))) },
                {"smoki",   new Artikal(1001, "Smoki", "2026-12-12", 100, null, 0, Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slike", "smoki.jfif"))) },
                {"cips",   new Artikal(1001, "Cips", "2026-12-12", 110, null, 0, Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slike", "cipsi.jfif"))) }
            };

            automat = new Automat(kasa, 5, 3);
            ekran = new PictureBox[5, 3];
            ekran[0, 0] = pictureBox1_1;
            ekran[1, 0] = pictureBox2_1;
            ekran[2, 0] = pictureBox3_1;
            ekran[3, 0] = pictureBox4_1;
            ekran[4, 0] = pictureBox5_1;
            ekran[0, 1] = pictureBox1_2;
            ekran[1, 1] = pictureBox2_2;
            ekran[2, 1] = pictureBox3_2;
            ekran[3, 1] = pictureBox4_2;
            ekran[4, 1] = pictureBox5_2;
            ekran[0, 2] = pictureBox1_3;
            ekran[1, 2] = pictureBox2_3;
            ekran[2, 2] = pictureBox3_3;
            ekran[3, 2] = pictureBox4_3;
            ekran[4, 2] = pictureBox5_3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {            

            comboBoxArtikli.Items.Add("kokakola");
            comboBoxArtikli.Items.Add("fanta");
            comboBoxArtikli.Items.Add("hidroaktiv");
            comboBoxArtikli.Items.Add("smoki");
            comboBoxArtikli.Items.Add("cips");
            numericPolica.Minimum = 1;
            numericPolica.Maximum = 5;
            numericPolje.Minimum = 1;            
            numericPolje.Maximum = 3;
            numericOdabranaPolica.Minimum = 1;
            numericOdabranaPolica.Maximum = 5;
            numericOdabranoPolje.Minimum = 1;
            numericOdabranoPolje.Maximum = 3;

        }

        private void button1_Click(object sender, EventArgs e)
        {   
            if (RB10.Checked){ kasa.DodajKredit(10); }
            if (RB20.Checked) { kasa.DodajKredit(20); ; }
            if (RB50.Checked) { kasa.DodajKredit(50); }
            if (RB100.Checked) { kasa.DodajKredit(100); }
            if (RB200.Checked) { kasa.DodajKredit(200); }
            labelKredit.Text = "Balans u dinarima = " + kasa.Kredit;
            RB10.Checked = false;
            RB20.Checked = false;
            RB50.Checked = false;
            RB100.Checked = false;
            RB200.Checked = false;
        }

        private void bOdaberi_Click(object sender, EventArgs e)
        {
            int odabranaPolica = (int)numericOdabranaPolica.Value;
            int odabranoPolje = (int)numericOdabranoPolje.Value;

            automat.Kupi(odabranaPolica, odabranoPolje);
            labelKredit.Text = "Balans u dinarima = " + kasa.Kredit;

            Artikal artikalNaVrhu = automat.ArtikalNaVrhu(odabranaPolica, odabranoPolje);
            if (artikalNaVrhu == null)
                ekran[odabranaPolica - 1, odabranoPolje - 1].Image = null;
            else
                ekran[odabranaPolica - 1, odabranoPolje - 1].Image = artikalNaVrhu.slika;

            IzaberiArtikal();
        }

        private void bKusur_Click(object sender, EventArgs e)
        {
            kasa.vratiKusur();
            labelKredit.Text = "Balans u dinarima = " + kasa.Kredit;
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (!PanelArtikl.Visible && textBoxArtikli.Text != "1234") 
                return;
            PanelArtikl.Visible = !PanelArtikl.Visible;
        }

        private void buttonDodajArtikal_Click(object sender, EventArgs e)
        {
            string odabraniArtikal = comboBoxArtikli.SelectedItem.ToString();

            int odabranaPolica = (int)numericPolica.Value;
            int odabranoPolje = (int)numericPolje.Value;

            if (!artikli.ContainsKey(odabraniArtikal)){
                return;
            }

            automat.PostaviArtikal(artikli[odabraniArtikal], odabranaPolica, odabranoPolje);
            ekran[odabranaPolica - 1, odabranoPolje - 1].Image = automat.ArtikalNaVrhu(odabranaPolica, odabranoPolje).slika;
        }

        private void IzaberiArtikal()
        {
            int odabranaPolica = (int)numericOdabranaPolica.Value;
            int odabranoPolje = (int)numericOdabranoPolje.Value;

            Artikal OdabraniArtikal = automat.ArtikalNaVrhu(odabranaPolica, odabranoPolje);

            if (OdabraniArtikal == null)
            {
                labelOdabraniArtikal.Text = "Izaberite Artikal";
                pictureBoxOdabraniArtikal.Image = null;
            }
            else
            {
                int kolicina = automat.Kolicina(odabranaPolica, odabranoPolje);
                labelOdabraniArtikal.Text = $"{OdabraniArtikal.naziv} - {OdabraniArtikal.cena} dinara - Preostalo: {kolicina}";
                pictureBoxOdabraniArtikal.Image = OdabraniArtikal.slika;
            }
        }

        private void numericOdabranaPolica_ValueChanged(object sender, EventArgs e)
        {
            IzaberiArtikal();
        }

        private void numericOdabranoPolje_ValueChanged(object sender, EventArgs e)
        {
            IzaberiArtikal();
        }

        private void buttonUbaciNovac_Click(object sender, EventArgs e)
        {
            if (radioButton10Din.Checked) { kasa.UbaciNovcanicu(10); }
            if (radioButton20Din.Checked) { kasa.UbaciNovcanicu(20); ; }
            if (radioButton50Din.Checked) { kasa.UbaciNovcanicu(50); }
            if (radioButton100Din.Checked) { kasa.UbaciNovcanicu(100); }
            if (radioButton200Din.Checked) { kasa.UbaciNovcanicu(200); }
            
            radioButton10Din.Checked = false;
            radioButton20Din.Checked = false;
            radioButton50Din.Checked = false;
            radioButton100Din.Checked = false;
            radioButton200Din.Checked = false;

            kasa.PrikaziNovcanice();
        }
    }
    public class Artikal
    {
        public int sifra;
        public string naziv;
        public string rok;
        public int cena;
        public string danPopusta;
        public int procenatPopusta;
        public Image slika;

        public Artikal(int sifra, string naziv, string rok, int cena, string danPopusta, int procenatPopusta, Image slika)
        {
            this.sifra = sifra;
            this.naziv = naziv;
            this.rok = rok;
            this.cena = cena;
            this.danPopusta = danPopusta;
            this.procenatPopusta = procenatPopusta;
            this.slika = slika;
        }
        public int vratiCenu()
        {
            return cena;
        }
    }
    public class Kasa 
    {
        public int Kredit = 0;
        private Dictionary<int, int> Novcanice;

        public Kasa()
        {
            Novcanice = new Dictionary<int, int>();
            Novcanice[10] = 0;
            Novcanice[20] = 0;
            Novcanice[50] = 0;
            Novcanice[100] = 0;
            Novcanice[200] = 0;
        }

        public void UbaciNovcanicu(int novcanica)
        {
            if(Novcanice.ContainsKey(novcanica))
                Novcanice[novcanica]++;
            else
                MessageBox.Show("Neispravna novčanica");
        }

        public void DodajKredit(int novcanica)
        {
            UbaciNovcanicu(novcanica);
            Kredit += novcanica;
        }      

        public void PrikaziNovcanice()
        {
            MessageBox.Show($"Trenutno stanje u kasi: 10: {Novcanice[10]}, 20: {Novcanice[20]}, 50: {Novcanice[50]}, 100: {Novcanice[100]}, 200: {Novcanice[200]}");
        }
        
        public void UmanjiKredit(int iznos)
        {
            Kredit -= iznos;
        }
        
        public void vratiKusur()
        {            
            Dictionary<int, int> kusurNovcanice = new Dictionary<int, int>();
            kusurNovcanice[10] = 0;
            kusurNovcanice[20] = 0;
            kusurNovcanice[50] = 0;
            kusurNovcanice[100] = 0;
            kusurNovcanice[200] = 0;

            int[] vrednostNovcanica = Novcanice.Keys.OrderBy(k => k).Reverse().ToArray();           
            foreach(int vrednostNovcanice in vrednostNovcanica)
            {
                while (Kredit >= vrednostNovcanice && Novcanice[vrednostNovcanice] > 0)
                {
                    Kredit -= vrednostNovcanice;
                    kusurNovcanice[vrednostNovcanice]++;
                    Novcanice[vrednostNovcanice]--; // Smanjujemo broj dostupnih novčanica
                }
            }

            // Ispisivanje stanja novčanica nakon vraćanja kusura
            MessageBox.Show($"Kusur ce biti vracen sledecim novcanicama: 10: {kusurNovcanice[10]}, 20: {kusurNovcanice[20]}, 50: {kusurNovcanice[50]}, 100: {kusurNovcanice[100]}, 200: {kusurNovcanice[200]}");
        }
    }
    
    public class Automat 
    {
        private Queue<Artikal>[,] inventar;
        Kasa kasa;
        public Automat(Kasa kasa, int brojPolica, int brojPolja)
        {
            this.kasa = kasa;
            inventar = new Queue<Artikal>[brojPolica, brojPolja];            
            for (int i = 0; i < brojPolica; i++)
            {
                for (int j = 0; j < brojPolja; j++)
                {
                    inventar[i, j] = new Queue<Artikal>();
                }
            }
        }

        public void PostaviArtikal(Artikal artikal, int polica, int polje)
        {
            inventar[polica-1, polje-1].Enqueue(artikal);
        }
        public void Kupi(int polica, int polje)
        {

            Artikal odabraniArtikal = ArtikalNaVrhu(polica, polje);
            if (Kolicina(polica, polje) > 0 && kasa.Kredit >= odabraniArtikal.cena)
            {
                kasa.UmanjiKredit(odabraniArtikal.cena);
                inventar[polica-1, polje-1].Dequeue();
            }
            else if (Kolicina(polica, polje) == 0)
            {
                MessageBox.Show($"Zao nam je, taj artikal je potrosen");
            }
            else if (kasa.Kredit < odabraniArtikal.cena)
            {
                MessageBox.Show($"Nemate dovoljno kredita.");
            }
        }
        
        public int Kolicina(int polica, int polje)
        {
            return inventar[polica-1, polje-1].Count();
        }
        public Artikal ArtikalNaVrhu(int polica, int polje)
        {
            if (Kolicina(polica, polje) == 0)
                return null;
            
            return inventar[polica-1, polje-1].Peek();
        }
    }
}
