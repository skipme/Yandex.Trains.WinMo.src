using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Fluid.Controls;
using System.Drawing;

namespace uiTest
{
    public class MainPanel : NavigationPanel
    {
        //SearchPanel sp = new SearchPanel();

        private PageControl HistoryPage;
        private PageControl DirectionsPage;
        private PageControl StationsFromPage;
        private PageControl StationsToPage;
        private PageControl TripsPage;

        protected override void InitControl()
        {
            base.InitControl();

            int h = Form1.GlobalBounds.Height / 9;
            this.Bounds = new Rectangle(0, 0, Form1.GlobalBounds.Width, Form1.GlobalBounds.Height);
            header.Bounds = new Rectangle(0, 0, Form1.GlobalBounds.Width, h);
            navigation.Bounds = new Rectangle(0, h, Form1.GlobalBounds.Width, Form1.GlobalBounds.Height - h);
            header.ButtonsWidth = (int)(Form1.GlobalBounds.Width * 0.15);

            Rectangle rb = header.backButton.Bounds;
            rb.Width = (int)(Form1.GlobalBounds.Width * 0.15);
            header.backButton.Bounds = rb;

            rb = header.rightButtons.Bounds;
            rb.Width = (int)(Form1.GlobalBounds.Width * 0.15);
            header.rightButtons.Bounds = rb;

            BackColor = System.Drawing.Color.Empty;

            HistoryPage = new PageControl("История");
            StationsFromPage = new PageControl("Отправление");
            StationsToPage = new PageControl("Назначение");
            TripsPage = new PageControl("Табло");
            DirectionsPage = new PageControl("Направления");

            FluidButton tohistpage = TripsPage.Buttons.AddNew();
            tohistpage.Text = "FAV";
            tohistpage.BackColor = Color.FromArgb(40, 90, 110);
            tohistpage.Width = (int)(Form1.GlobalBounds.Width * 0.2);
            tohistpage.Height = (int)(h * 0.8);
            tohistpage.Shape = ButtonShape.Next;

            FluidButton syncbutton = new FluidButton(); //TripsPage.Buttons.AddNew();
            syncbutton.Text = "sync";
            syncbutton.Visible = true;
            syncbutton.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            syncbutton.Width = (int)(Form1.GlobalBounds.Width * 0.15);
            syncbutton.Height = (int)(h * 0.8);
            syncbutton.BackColor = Color.FromArgb(40, 90, 110);
            syncbutton.Shape = ButtonShape.Rounded;

            syncbutton.Click += new EventHandler(suncbutton_Click);
            tohistpage.Click += new EventHandler(tohistpage_Click);
            TripsPage.BackButton = syncbutton;

            StationsFromPage.Control = new StationsList();
            StationsToPage.Control = new StationsList();
            HistoryPage.Control = new HistoryList();
            TripsPage.Control = new TripList();
            DirectionsPage.Control = new DirectionList();

            FluidButton HNewButton = new FluidButton("Новый", Color.FromArgb(40, 90, 110));
            HNewButton.Shape = ButtonShape.Rounded;
            HNewButton.Visible = true;
            HNewButton.Width = (int)(Form1.GlobalBounds.Width * 0.15);
            HNewButton.Height = (int)(h * 0.8);
            HNewButton.Click += new EventHandler(HNewButton_Click);
            HistoryPage.Buttons.Add(HNewButton);

            FluidButton HStatSrchButton = new FluidButton("Поиск", Color.FromArgb(40, 90, 110));
            HStatSrchButton.Shape = ButtonShape.Rounded;
            HStatSrchButton.Visible = true;
            HStatSrchButton.Width = (int)(Form1.GlobalBounds.Width * 0.15);
            HStatSrchButton.Height = (int)(h * 0.8);
            HStatSrchButton.Click += new EventHandler(HStatSrchButton_Click);
            StationsFromPage.Buttons.Add(HStatSrchButton);

            FluidButton HStatSrchButtonTo = new FluidButton("Поиск", Color.FromArgb(40, 90, 110));
            HStatSrchButtonTo.Shape = ButtonShape.Rounded;
            HStatSrchButtonTo.Visible = true;
            HStatSrchButtonTo.Width = (int)(Form1.GlobalBounds.Width * 0.15);
            HStatSrchButtonTo.Height = (int)(h * 0.8);
            HStatSrchButtonTo.Click += new EventHandler(HStatSrchButtonTo_Click);
            StationsToPage.Buttons.Add(HStatSrchButtonTo);

            Pages.Add(HistoryPage);
            Pages.Add(DirectionsPage);
            Pages.Add(StationsFromPage);
            Pages.Add(StationsToPage);
            Pages.Add(TripsPage);

            SelectedIndex = 0;

            StationsList StationsFromListBox = StationsFromPage.Control as StationsList;
            StationsList StationsToListBox = StationsToPage.Control as StationsList;
            HistoryList HistoryListBox = HistoryPage.Control as HistoryList;
            DirectionList DirectListBox = DirectionsPage.Control as DirectionList;
            TripList TripListBox = TripsPage.Control as TripList;

            StationsFromListBox.OnStationChanged += new StationsList.StationChanged(StationsFromListBox_OnStationChanged);
            HistoryListBox.OnHistorySelected += new HistoryList.HistorySelected(HistoryListBox_OnHistorySelected);
            DirectListBox.OnDirectionSelected += new DirectionList.DirectionSelected(DirectListBox_OnDirectionSelected);
            StationsToListBox.OnStationChanged += new StationsList.StationChanged(StationsToListBox_OnStationChanged);
            TripListBox.OnTripSelected += new TripList.TripSelected(TripListBox_OnTripSelected);

            HistoryListBox.Populate();
            HistoryListBox.OnClickRemove += new HistoryList.RemoveItem(HistoryListBox_OnClickRemove);
        }

        void HStatSrchButtonTo_Click(object sender, EventArgs e)
        {
            if (UiShowStationSearcher != null)
                UiShowStationSearcher(false);
        }

        void HStatSrchButton_Click(object sender, EventArgs e)
        {
            if (UiShowStationSearcher != null)
                UiShowStationSearcher(true);
        }

        void HistoryListBox_OnClickRemove(HistoryItem hi)
        {
            data.HistorySlots.Remove(hi);
            (HistoryPage.Control as HistoryList).Populate();
        }

        public void RefreshDirections()
        {
            (DirectionsPage.Control as DirectionList).Populate();
        }

        void suncbutton_Click(object sender, EventArgs e)
        {
            UpdateTrips(true);
            //throw new NotImplementedException();
        }

        void TripListBox_OnTripSelected(TripItem trip)
        {
            //string msg = string.Format("{0}[{3}]({1}){2}", trip.Title, trip.Stops, trip.Days, trip.Platform);
            //MessageDialog.Show(msg, "", null);
        }

        void tohistpage_Click(object sender, EventArgs e)
        {
            Switch(0);
        }

        void StationsToListBox_OnStationChanged(StationItem esr)
        {
            SuburbanContext.SetEnd(esr);
            UpdateTrips();
            Forward();
        }
        public void UpdateTrips() { UpdateTrips(false); }
        private void UpdateTrips(bool sync)
        {
            (TripsPage.Control as TripList).PopulateWithSearch(sync);
            TripsPage.Title = SuburbanContext.CurrentRequest;
            (HistoryPage.Control as HistoryList).Populate();

            if (SuburbanContext.NetworkNA)
                MessageDialog.Show("Сеть недоступна", "OK", null);
            else if (sync)
            {
                MessageDialog.Show("Синхронизация успешна", "OK", null);
            }
        }

        void DirectListBox_OnDirectionSelected(string dir)
        {
            (StationsToPage.Control as StationsList).PopulateWithDirection(dir);
            (StationsFromPage.Control as StationsList).PopulateWithDirection(dir);
            Switch(2);
        }

        void HNewButton_Click(object sender, EventArgs e)
        {
            DirectionList dl = (DirectionsPage.Control as DirectionList);
            if (dl.DataSource == null || ((List<string>)dl.DataSource).Count == 0)
                RefreshDirections();
            Switch(1);
        }

        void HistoryListBox_OnHistorySelected(HistoryItem esr)
        {
            SuburbanContext.SetStart(esr.Start);
            SuburbanContext.SetEnd(esr.End);
            Switch(4);

            UpdateTrips();
        }

        void StationsFromListBox_OnStationChanged(StationItem esr)
        {
            SuburbanContext.SetStart(esr);
            Forward();
        }
        public void ShowSearchPanel()
        {

        }

        public delegate void ToSearchStation(bool FromList);
        public ToSearchStation UiShowStationSearcher;
    }
}
