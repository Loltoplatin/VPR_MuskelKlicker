﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Timers;

namespace MuskelKlicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Random rnd = new Random();

        Clicker clicker = new Clicker(0, 1);
        List<ShopItem> ListItems = new List<ShopItem>();

        int points = 100;
        int multiplyer = 1;
        int clicksPerSecond = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ToDo: -Klicks per sec             Done: Andrew John / Dennis
            //      -Reset von klicks per sec   Done: Andrew John / Dennis
            //      -Klickbonus                 Done: Andrew John
            //      -Grafiken einfügen          Done: Andrew John
            //      -Shopitems aus der DB       
            //      -Fortschritt speichern      fehlt Prestige
            //      -Fortschritt aufrufen       fehlt Prestige

            #region Shop und Item Update
            //Items | Clicker
            lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
            lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);

            //Aktive Clicks
            ShopItem hanteln = new ShopItem(10, "Hanteln", "Erhöht dein Klick um 1", 0, 1);
            ShopItem gHanteln = new ShopItem(20, "Goldene-Hanteln", "Erhöht dein Klick um 2", 0, 2);
            ShopItem protein = new ShopItem(200, "Protein", "Erhöht den Passive und Aktiven Klick um 10", 10, 10);

            //Passive Clicks
            ShopItem schlaf = new ShopItem(100, "Schlafen", "Erhöht den Passive um 10", 10, 0);

            //Add Items
            ListItems.Add(hanteln);
            ListItems.Add(gHanteln);
            ListItems.Add(protein);
            ListItems.Add(schlaf);


            //Shop mit Items erstellen
            foreach (ShopItem items in ListItems)
            {
                lstbx_shopitems.Items.Add(items);
            }
            lbl_Points.Content = points.ToString();
            #endregion

            #region Gespeicherten Fortschritt aufrufen
            // SpielstandDTB spielstand = new SpielstandDTB();

            List<int> countList = new List<int>();
            //countList = spielstand.GetSpielstand();

            if (countList.Count > 0)
            {
                //Übernimmt Punkte
                points = countList[0];

                //Übernimmt Hantel
                ShopItem item = (ShopItem)lstbx_shopitems.Items[0];
                for (int i = 0; i < countList[1]; i++)
                {
                    item.Cost *= 2;
                    clicker.ActiveClick += item.UpgradeA;
                    clicker.PassiveClick += item.UpgradeP;
                }
                lbl_Points.Content = points.ToString();

                //Übernimmt Goldene Hanteln
                item = (ShopItem)lstbx_shopitems.Items[1];
                for (int i = 0; i < countList[2]; i++)
                {
                    item.Cost *= 2;
                    clicker.ActiveClick += item.UpgradeA;
                    clicker.PassiveClick += item.UpgradeP;
                }
                lbl_Points.Content = points.ToString();

                //Übernimmt Protein
                item = (ShopItem)lstbx_shopitems.Items[2];
                for (int i = 0; i < countList[3]; i++)
                {
                    item.Cost *= 2;
                    clicker.ActiveClick += item.UpgradeA;
                    clicker.PassiveClick += item.UpgradeP;
                }
                lbl_Points.Content = points.ToString();

                //Übernimmt Schlaf
                item = (ShopItem)lstbx_shopitems.Items[3];
                for (int i = 0; i < countList[4]; i++)
                {
                    item.Cost *= 2;
                    clicker.ActiveClick += item.UpgradeA;
                    clicker.PassiveClick += item.UpgradeP;
                }
                lbl_Points.Content = points.ToString();

                //zeigt alles nochmal richtig an
                lstbx_shopitems.Items.Refresh();

                lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
                lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);
            }

            #endregion


            #region Timer (1 Sec)

            //Timer für jede Sekunde
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //this.dateText.Content = DateTime.Now.ToString("ss");
                points += clicker.PassiveClick;
                lbl_Points.Content = points.ToString();

                clicksPerSecond = 0;
                lbl_Clicks.Content = clicksPerSecond.ToString();


                // Lässt den PowerUp-Button mit einer 1%-Wahrscheinlichkeit spawnen
                if(rnd.Next(0,2) == 1)
                {
                    bt_powerUP_spawn();
                }
                else
                {
                    bt_powerUP.IsEnabled = false;
                    bt_powerUP.Visibility = Visibility.Hidden;
                }

                
                
            }, this.Dispatcher);
            #endregion
        }
        private void lstbx_shopitems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstbx_shopitems.SelectedItem != null)
            {
                ShopItem item = (ShopItem)lstbx_shopitems.SelectedItem;

                //Check, ob points vorhanden sind | Kosten erhöhen und A/P klick verbessern
                if (item.Cost <= points)
                {                        
                    points -= item.Cost;
                    item.Cost *= 2;
                    clicker.ActiveClick += item.UpgradeA;
                    clicker.PassiveClick += item.UpgradeP;
                    MessageBox.Show(points.ToString());
                    lbl_Points.Content = points.ToString();
                }
                else
                {
                    MessageBox.Show("Nicht genügend Geld");
                }
                lstbx_shopitems.Items.Refresh();

                //Aktiven Clicker und Passiven Clicker darstellen
                lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
                lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WriteToLabel();

            GetBonusPoints(20);
        }

        private void WriteToLabel()
        {
            //Button Aktive Click
            points += clicker.ActiveClick;
            lbl_Points.Content = points.ToString();


            // Clicks per Second
            clicksPerSecond++;
            lbl_Clicks.Content = clicksPerSecond.ToString();
        }

        private void GetBonusPoints(int bonusPoints)
        {
            if (Convert.ToInt32(lbl_Clicks.Content) >= 10)
            {
                points *= bonusPoints;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {   
            SpielstandDTB spielstand = new SpielstandDTB();
            List<int> countList = new List<int>();

            //Zähler für Hanteln
            ShopItem item = (ShopItem)lstbx_shopitems.Items[0];
            int newCost = item.Cost;
            int buyCount = 0;
            for (int i = 0;  newCost >= 10; i++)
            {
                newCost /= 2;
                buyCount = i;
            }
            countList.Add(buyCount);

            //Zähler für Goldene-Hanteln
            item = (ShopItem)lstbx_shopitems.Items[1];
            newCost = item.Cost;
            buyCount = 0;
            for (int i = 0; newCost >= 20; i++)
            {
                newCost /= 2;
                buyCount = i;
            }
            countList.Add(buyCount);

            //Zähler für Protein
            item = (ShopItem)lstbx_shopitems.Items[2];
            newCost = item.Cost;
            buyCount = 0;
            for (int i = 0; newCost >= 200; i++)
            {
                newCost /= 2;
                buyCount = i;
            }
            countList.Add(buyCount);

            //Zähler für Schlafen
            item = (ShopItem)lstbx_shopitems.Items[3];
            newCost = item.Cost;
            buyCount = 0;
            for (int i = 0; newCost >= 100; i++)
            {
                newCost /= 2;
                buyCount = i;
            }
            countList.Add(buyCount);

            spielstand.SaveSpielstand(points, countList[0], countList[1], countList[2], countList[3]);
        }

        private void bt_deleteSpielstand_Click(object sender, RoutedEventArgs e)
        {
            SpielstandDTB spielstand = new SpielstandDTB();
            spielstand.DeleteSpielstand();

            List<int> countList = new List<int>();
            countList = spielstand.GetSpielstand();

            if (countList.Count > 0)
            {
                //Übernimmt Punkte
                points = 100;
                lbl_Points.Content = points.ToString();

                //Übernimmt Hantel
                ShopItem item = (ShopItem)lstbx_shopitems.Items[0];

                item.Cost = 10;
                clicker.ActiveClick = 1;
                clicker.PassiveClick = 0;
              
                //Übernimmt Goldene Hanteln
                item = (ShopItem)lstbx_shopitems.Items[1];

                item.Cost = 20;

                //Übernimmt Protein
                item = (ShopItem)lstbx_shopitems.Items[2];

                item.Cost = 200;

                //Übernimmt Schlaf
                item = (ShopItem)lstbx_shopitems.Items[3];

                item.Cost = 100;

                //zeigt alles nochmal richtig an
                lstbx_shopitems.Items.Refresh();

                lab_ActiveClick.Content = string.Format("Aktiver Klick: " + clicker.ActiveClick);
                lab_PassiveClick.Content = string.Format("Passive Punkte: " + clicker.PassiveClick);
            }

        }

        private void bt_ten_Click(object sender, RoutedEventArgs e)
        {
            multiplyer = 10;
            MultiplyItemCost();

            bt_ten.IsEnabled = false;
            bt_five.IsEnabled = false;
            bt_one.IsEnabled = true;
        }

        private void bt_five_Click(object sender, RoutedEventArgs e)
        {
            multiplyer = 5;
            MultiplyItemCost();

            bt_ten.IsEnabled = false;
            bt_five.IsEnabled = false;
            bt_one.IsEnabled = true;
        }

        private void bt_one_Click(object sender, RoutedEventArgs e)
        {

            ReduceItemCost();
            multiplyer = 1;
            
            bt_ten.IsEnabled = true;
            bt_five.IsEnabled = true;
            bt_one.IsEnabled = false;
        }

        private void MultiplyItemCost()
        {
            foreach (ShopItem item in ListItems)
            {
                for (int i = 0; i < multiplyer; i++)
                {
                    item.Cost *= 2;
                    
                }

                item.UpgradeA *= multiplyer;
                item.UpgradeP *= multiplyer;
            }
            

            lstbx_shopitems.Items.Refresh();
        }

        private void ReduceItemCost()
        {
            foreach (ShopItem item in ListItems)
            {
                for (int i = 0; i < multiplyer; i++)
                {
                    item.Cost /= 2;
                }

                item.UpgradeA /= multiplyer;
                item.UpgradeP /= multiplyer;
            }
            lstbx_shopitems.Items.Refresh();
        }

        private void bt_powerUP_spawn()
        {
            bt_powerUP.Visibility = Visibility.Visible;
            bt_powerUP.IsEnabled = true;
            MainWindow m = new MainWindow();

            //Höhe und Breite des MainWindows
            var width = Window.GetWindow(m).Width;
            var height = Window.GetWindow(m).Height;

            //Zufallskoordinaten für den PowerUP-Button
            double rndWidth = rnd.Next(50, (int)width - 50);
            double rndHeight = rnd.Next(50, (int)height - 50);

            bt_powerUP.Margin = new Thickness(rndWidth, rndHeight, 0, 0);

        }

        private void bt_powerUP_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
