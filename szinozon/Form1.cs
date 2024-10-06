using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace szinozon
{
    public partial class Form1 : Form
    {
        private List<Color> szinek; // A választható színek listája
        private Color[] helyesSzinek; // A generált helyes szín sorozat
        private int tippSzam; // Tippelések száma
        private int maxKivalaszthatoSzin; // Maximum kiválasztható színek száma
        private Color kiválasztottSzin; // Aktuálisan kiválasztott szín

        public Form1()
        {
            InitializeComponent();
            buttonJatekInditasa.Click += new EventHandler(buttonJatekInditasa_Click);
            buttonOK.Click += new EventHandler(buttonOK_Click);
            numericUpDownSzinszam.ValueChanged += new EventHandler(numericUpDownSzinszam_ValueChanged);
            szinek = new List<Color>(); // Inicializálás
            tippSzam = 0;
            maxKivalaszthatoSzin = 3; // Alapértelmezett érték 3
            kiválasztottSzin = Color.Empty; // Nincs szín kiválasztva alapértelmezésben

            // TextBoxok kattintás eseményének kezelése
            textBoxTipp1.Click += new EventHandler(TextBoxTipp_Click);
            textBoxTipp2.Click += new EventHandler(TextBoxTipp_Click);
            textBoxTipp3.Click += new EventHandler(TextBoxTipp_Click);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicializáld a színpanelt
            InicializalSzinek();
        }

        private void InicializalSzinek()
        {
            // Példa színek hozzáadása a választható színekhez
            szinek.Add(Color.Red);
            szinek.Add(Color.Blue);
            szinek.Add(Color.Green);
            szinek.Add(Color.Yellow);
            szinek.Add(Color.Orange);
            szinek.Add(Color.Purple);
            szinek.Add(Color.MidnightBlue);
            szinek.Add(Color.Cyan);
            szinek.Add(Color.SaddleBrown);
            szinek.Add(Color.Pink);
            szinek.Add(Color.Lime);
            szinek.Add(Color.Coral);

            // Színek megjelenítése a flowLayoutPanelSzinek-ben
            foreach (var szin in szinek)
            {
                Button szinGomb = new Button();
                szinGomb.BackColor = szin;
                szinGomb.Size = new Size(50, 50);
                szinGomb.Click += SzinevalasztasElsoLepes; // Szín kiválasztás kezelése
                flowLayoutPanelSzinek.Controls.Add(szinGomb);
            }
        }

        private void SzinevalasztasElsoLepes(object sender, EventArgs e)
        {
            // Frissítjük a maxKivalaszthatoSzin értékét
            maxKivalaszthatoSzin = (int)numericUpDownSzinszam.Value;

            // Ellenőrzés, hogy hány színt választottak már ki
            if (flowLayoutPanelValasztottSzinek.Controls.Count >= maxKivalaszthatoSzin)
            {
                MessageBox.Show($"Nem választhatsz több színt! Maximum {maxKivalaszthatoSzin} színt választhatsz.");
                return;
            }

            if (sender is Button gomb)
            {
                // Kiválasztott szín hozzáadása a jobb oldali panelhez
                Button ujGomb = new Button
                {
                    BackColor = gomb.BackColor,
                    Size = new Size(50, 50),
                    Enabled = false // Ekkor még nem lehet használni
                };
                ujGomb.Click += SzinevalasztasMasodikLepes; // Második lépés: szín választása tippeléskor
                flowLayoutPanelValasztottSzinek.Controls.Add(ujGomb);
            }
        }

        private void buttonJatekInditasa_Click(object sender, EventArgs e)
        {

            if (flowLayoutPanelValasztottSzinek.Controls.Count < maxKivalaszthatoSzin)
            {
                MessageBox.Show($"Kérjük, válassz ki {maxKivalaszthatoSzin} színt a játék indítása előtt.");
                return; // Ha nincs elegendő szín kiválasztva, megszakítjuk a folyamatot
            }
            // Új játék indítása
            tippSzam = 0;
            label5.Text = "Eddigi tippek:";
            GeneraltHelyesSzinek();

            // Elrejtjük a feladvány színeit, de mutatjuk őket
            textBoxFeladvany1.BackColor = helyesSzinek[0];
            textBoxFeladvany2.BackColor = helyesSzinek[1];
            textBoxFeladvany3.BackColor = helyesSzinek[2];

            flowLayoutPanelSzinek.Enabled = false; // Tiltja a színválasztást
            numericUpDownSzinszam.Enabled = false; // Tiltja a szám beállítását

            // Engedélyezzük a jobb oldali színgombokat tippeléshez
            foreach (Button gomb in flowLayoutPanelValasztottSzinek.Controls)
            {
                gomb.Enabled = true;
            }

            // Lehetővé tesszük a tippelések elvégzését
            buttonOK.Enabled = true;
        }

        private void SzinevalasztasMasodikLepes(object sender, EventArgs e)
        {
            // Második lépés: Tippeléshez szín kiválasztása és megjelenítése
            if (sender is Button gomb)
            {
                kiválasztottSzin = gomb.BackColor;
                pictureBoxKivalasztottSzin.BackColor = kiválasztottSzin; // Szín megjelenítése a PictureBoxban
            }
        }

        private void TextBoxTipp_Click(object sender, EventArgs e)
        {
            // Tipp TextBox-ba helyezzük a kiválasztott színt, ha van kiválasztva
            if (kiválasztottSzin != Color.Empty && sender is TextBox tippTextBox)
            {
                tippTextBox.BackColor = kiválasztottSzin;
            }
        }

        private void GeneraltHelyesSzinek()
        {
            // Ellenőrzés, hogy a helyes színek már generálva lettek-e
            if (helyesSzinek != null && helyesSzinek.Length == 3)
            {
                return; // Ha a helyes színek már generálva lettek, akkor nem változtatjuk meg őket
            }

            // Helyes színek generálása a kiválasztott színekből
            Random rand = new Random();
            helyesSzinek = new Color[3]; // Mindig 3 helyes szín

            for (int i = 0; i < helyesSzinek.Length; i++)
            {
                // A jobb oldali panelen (flowLayoutPanelValasztottSzinek) kiválasztott színekből választunk
                int randomIndex = rand.Next(flowLayoutPanelValasztottSzinek.Controls.Count);
                if (flowLayoutPanelValasztottSzinek.Controls[randomIndex] is Button valasztottSzinGomb)
                {
                    helyesSzinek[i] = valasztottSzinGomb.BackColor;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            tippSzam++;

            Color[] tipp = new Color[3];
            // Tipp beállítása
            for (int i = 0; i < 3; i++)
            {
                TextBox tippTextBox = this.Controls.Find($"textBoxTipp{i + 1}", true).FirstOrDefault() as TextBox;
                if (tippTextBox != null)
                {
                    tipp[i] = tippTextBox.BackColor; // A tippbe mentjük a színt
                }
            }

            // Ellenőrizni, hogy mennyi találat van helyes helyen
            int helyesHelyen = 0;
            int rosszHelyen = 0;

            bool[] helyesSzinekTalalat = new bool[3]; // A helyes színek találatainak követésére
            bool[] tippTalalat = new bool[3]; // A tipp találatainak követésére

            // StringBuilder eredmények kiírására
            string[] eredmenyek = new string[3]; // Helyes válaszok tárolására

            // Első kör: ellenőrizzük a helyes helyen lévő színeket
            for (int i = 0; i < 3; i++)
            {
                if (tipp[i] == helyesSzinek[i])
                {
                    helyesHelyen++;
                    helyesSzinekTalalat[i] = true; // Jegyezzük meg, hogy ez a szín már helyes helyen van
                    tippTalalat[i] = true; // Jegyezzük meg, hogy ezt a tippet már feldolgoztuk
                    eredmenyek[i] = $"Szín {i + 1}: Helyes és jó helyen van."; // Eredmény tárolása
                }
                else
                {
                    eredmenyek[i] = $"Szín {i + 1}: "; // Kezdeti üzenet
                }
            }

            // Második kör: ellenőrizzük a rossz helyen lévő színeket
            for (int i = 0; i < 3; i++)
            {
                if (!tippTalalat[i]) // Csak azokat a tippeket vizsgáljuk, amelyek még nem voltak helyes helyen
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // Ellenőrizzük, hogy a szín benne van-e a helyes színekben, és még nem találtuk-e
                        if (!helyesSzinekTalalat[j] && tipp[i] == helyesSzinek[j])
                        {
                            rosszHelyen++;
                            helyesSzinekTalalat[j] = true; // Jegyezzük meg, hogy ezt a helyes színt már használtuk
                            tippTalalat[i] = true; // Ez a tipp már feldolgozva
                            eredmenyek[i] += "Helyes, de rossz helyen van."; // Eredmény frissítése
                            break;
                        }
                    }

                    if (!tippTalalat[i]) // Ha nem találtunk semmit
                    {
                        eredmenyek[i] += "Nincs benne."; // Eredmény frissítése
                    }
                }
            }

            // Eredmények kiírása
            label5.Text = $"Eredmény:\n{string.Join("\n", eredmenyek)}";

            // Ellenőrzés a győzelemre
            if (helyesHelyen == 3)
            {
                MessageBox.Show($"Gratulálok! Kitaláltad a színeket {tippSzam} tippből!");
            }
            else
            {
                MessageBox.Show($"Nem találtad el! Tippjeid:\n{string.Join("\n", eredmenyek)}");
            }
        }

        private void numericUpDownSzinszam_ValueChanged(object sender, EventArgs e)
        {
            // A numericUpDown értéke alapján frissítjük a max kiválasztható színek számát
            maxKivalaszthatoSzin = (int)numericUpDownSzinszam.Value;

            // Töröljük a korábbi kiválasztott színeket
            flowLayoutPanelValasztottSzinek.Controls.Clear();
            pictureBoxKivalasztottSzin.BackColor = Color.Transparent; // Töröljük a kiválasztott színt

            // Informáljuk a felhasználót, ha kevesebb színt választottak
            if (flowLayoutPanelSzinek.Controls.Count < maxKivalaszthatoSzin)
            {
                MessageBox.Show($"Jelenleg csak {flowLayoutPanelSzinek.Controls.Count} szín érhető el a választásra. Kérlek, válassz több színt.");
                numericUpDownSzinszam.Value = flowLayoutPanelSzinek.Controls.Count; // Állítsuk vissza az értéket
                maxKivalaszthatoSzin = flowLayoutPanelSzinek.Controls.Count; // Frissítjük a max értéket
            }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
