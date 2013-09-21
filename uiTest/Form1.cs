using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Fluid.Controls;

namespace uiTest
{
    public partial class Form1 : Form
    {
        SearchPanel fpanel;
        CitySearchPanel cityies;
        MainPanel MainBoard;

        bool CloseSearch = false;

        public static Rectangle GlobalBounds;

        public Form1()
        {
            InitializeComponent();
            Host.Bounds = Host.ClientBounds;
            Host.BackColor = Color.Empty;

            GlobalBounds = Host.Bounds;
            Fluid.Classes.Globals.bounds = GlobalBounds;

            MainBoard = new MainPanel();
            MainBoard.Bounds = this.ClientRectangle;
            MainBoard.Focus();
            MainBoard.ShowSearchPanel();
            MainBoard.UiShowStationSearcher = StationSearch;

            Host.Add(MainBoard);
            if (!Program.FirstStart)
                MainBoard.RefreshDirections();
        }

        void StationSearch(bool From)
        {
            CloseSearch = true;
            menuItem3.Enabled = false;

            SuburbanContext.SearchStation(From);
            fpanel = new SearchPanel();
            fpanel.backward += new SearchPanel.ExecBack(fpanel_backward);
            MainBoard.Visible = false;
            fpanel.ShowMaximized(ShowTransition.FromBottom);
            menuItem1.Text = "Закрыть";
        }

        void fpanel_backward(StationItem s)
        {
            SuburbanContext.SetStationFromSearch(s);
            if (!SuburbanContext.SearchListStationFrom)
            {
                MainBoard.UpdateTrips();
            }

            MainBoard.Visible = true;

            fpanel.Close();
            fpanel = null;

            MainBoard.Forward();
            menuItem3.Enabled = true;
            CloseSearch = false;
            menuItem1.Text = "Выход";
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Program.FirstStart)
            {
                ShowCities();
            }
        }

        private void ShowCities()
        {
            if (cityies == null)
            {
                cityies = new CitySearchPanel();
                cityies.OnOkay += new CitySearchPanel.ExecBack(cityies_OnOkay);
                MainBoard.Visible = false;
                cityies.ShowMaximized(ShowTransition.FromBottom);
            }
            else
            {
                MainBoard.Visible = true;

                cityies.Close();
                cityies = null;
            }
        }

        void cityies_OnOkay(int cityid)
        {
            SuburbanContext.SetCity(cityid);
            data.Config cfg = new uiTest.data.Config() { City = cityid };
            cfg.Save();

            MainBoard.RefreshDirections();
            MainBoard.Visible = true;
            MainBoard.Switch(0);

            cityies.Close();
            cityies = null;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (CloseSearch)
            {
                MainBoard.Visible = true;
                fpanel.Close();
                fpanel = null;
                menuItem3.Enabled = true;
                CloseSearch = false;
                menuItem1.Text = "Выход";
            }
            else
            {
                this.Close();
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Host.Visible = false;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            Host.Visible = true;
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            ShowCities();
        }

    }
}