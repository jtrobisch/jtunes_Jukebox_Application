using System;
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

namespace Final_JukeBox_Application.Pages
{
    /// <summary>
    /// Interaction logic for album.xaml
    /// </summary>
    public partial class album : UserControl
    {
        private JukeBoxApp ObjectProgram = (App.Current as App).GlobalJukeBox;
        private List<String> Albums = new List<String>();
        private List<track> TempSearch = new List<track>();
        public album()
        {
            InitializeComponent();
            ObjectProgram.AddTrackPlayingLabel(PlayLabel);
            ObjectProgram.AddTrackTimerLabels(timerLabel);
            ObjectProgram.AlbumObject = this;
            load_albums();
            try
            {
                var uri = new Uri("pack://application:,,,/Images/SearchFinal.png");
                var bitmap = new BitmapImage(uri);
                image.Source = bitmap;
            }
            catch
            {
                System.Windows.MessageBox.Show("Error Loading Icons: Search icon missing.", "Image Error 1.13H");
            }
        }

        public void load_albums()
        {
            Albums.Clear();
            TempSearch.Clear();
            listBox.Items.Clear();
            listBox1.Items.Clear();

            List<track> TempLibrary = ObjectProgram.return_all_tracks_from_library();

            foreach (track xItem in TempLibrary)
            {
                if ((Albums.Contains(xItem.album_name)) == false && xItem.album_name != "")
                {
                    Albums.Add(xItem.album_name);
                }
            }
            Albums.Sort();
            Albums.Add("Misc - No Meta Data Tracks");
            foreach (String item in Albums)
            {
                listBox.Items.Add(item);
            }
            listBox.SelectedIndex = 0;
            tracksForAlbums(Albums[0]);
        }

        public List<track> tracksForAlbums(String Album)
        {
            List<track> tempX = new List<track>();
            listBox1.Items.Clear();
            String Album2 = "";
            if (Album == "Misc - No Meta Data Tracks")
            {
                Album2 = "";
            }
            else
            {
                Album2 = Album;
            }

            List<track> TempLibrary = ObjectProgram.return_all_tracks_from_library();
            foreach (track item in TempLibrary)
            {
                if (item.album_name == Album2)
                {
                    listBox1.Items.Add(item.track_name);
                    tempX.Add(item);
                }
            }
            return tempX;
        }



        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                ObjectProgram.addLibraryListBox(listBox1);
                track play = new track();
                play = ObjectProgram.return_track_by_name(listBox1.Items[listBox1.SelectedIndex].ToString());
                ObjectProgram.playTrack(play);
            }
            else
            {
                ObjectProgram.playTrack();
            }
        }

        private void button1_copy_Click(object sender, RoutedEventArgs e)
        {
            listBox1.SelectedIndex = -1;
            ObjectProgram.pauseTrack();
        }

        private void button1_copy_2(object sender, RoutedEventArgs e)
        {
            listBox1.SelectedIndex = -1;
            ObjectProgram.stopTrack();
        }

        private void button1_copy_2_3(object sender, RoutedEventArgs e)
        {
            ObjectProgram.addLibraryListBox(listBox1);
            ObjectProgram.PlayPreviousTrack();
        }

        private void button1_copy_2_4(object sender, RoutedEventArgs e)
        {
            ObjectProgram.addLibraryListBox(listBox1);
            ObjectProgram.AutoPlayNextTrack();
        }

        private void searchTrack_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempSearch.Clear();

            if (searchTrack.GetLineLength(0) > 0)
            {
                TempSearch = return_specific_tracks_from_library_albums(searchTrack.Text);
                listBox1.Items.Clear();
                foreach (track item in TempSearch)
                {
                    listBox1.Items.Add(item.track_name);
                }
            }
            else
            {
                TempSearch.Clear();
                TempSearch = tracksForAlbums(Albums[listBox.SelectedIndex]);
                listBox1.Items.Clear();
                foreach (track item in TempSearch)
                {
                    listBox1.Items.Add(item.track_name);
                }
            }
            ObjectProgram.updateListBoxPlaying();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex > -1)
            {
                tracksForAlbums(Albums[listBox.SelectedIndex]);
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ObjectProgram.addLibraryListBox(listBox1);
            track play = new track();
            play = ObjectProgram.return_track_by_name(listBox1.Items[listBox1.SelectedIndex].ToString());
            ObjectProgram.playTrack(play);
        }

        public List<track> return_specific_tracks_from_library_albums(String SearchText)
        {
            SearchText = SearchText.ToLower();
            List<track> templist = new List<track>();
            foreach (track item in tracksForAlbums(Albums[listBox.SelectedIndex]))
            {
                if ((item.track_name.ToLower()).Contains(SearchText))
                {
                    templist.Add(item);
                }
            }
            return templist;
        }
    }
}
