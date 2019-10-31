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
    /// Interaction logic for playlists.xaml
    /// </summary>
    public partial class playlists : UserControl
    {

        private JukeBoxApp ObjectProgram = (App.Current as App).GlobalJukeBox;
        private List<playlist> PlaylistsList = new List<playlist>();
        private List<track> TempSearch = new List<track>();
        public playlists()
        {
            InitializeComponent();
            ObjectProgram.AddTrackPlayingLabel(PlayLabel);
            ObjectProgram.AddTrackTimerLabels(timerLabel);
            ObjectProgram.PLaylistsObject = this;
            load_playlists();
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

        public void load_playlists()
        {
            PlaylistsList.Clear();
            PlaylistsList = ObjectProgram.returnAllPlaylists().OrderBy(x => x.playListName).ToList();
            TempSearch.Clear();
            listBox.Items.Clear();
            listBox1.Items.Clear();

            foreach (playlist xItem in PlaylistsList)
            {
                listBox.Items.Add(xItem.playListName);
            }
            
            if (listBox.Items.Count > 0)
            {
                listBox.SelectedIndex = 0;
                tracksForplaylists(listBox.SelectedIndex);
            }
        }

        public void tracksForplaylists(int i)
        {
            listBox1.Items.Clear();        
            foreach (track item in PlaylistsList[i].returnPlayListTracks())
            {
                    listBox1.Items.Add(item.track_name);
            }
        }



        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ObjectProgram.addLibraryListBox(listBox1);
            track play = new track();
            play = ObjectProgram.return_track_by_name(listBox1.Items[listBox1.SelectedIndex].ToString());
            ObjectProgram.playTrack(play);
        }

        private void searchTrack_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempSearch.Clear();
            if (searchTrack.GetLineLength(0) > 0)
            {
                TempSearch = return_specific_tracks_from_library_playlists(searchTrack.Text);
                listBox1.Items.Clear();
                foreach (track item in TempSearch)
                {
                    listBox1.Items.Add(item.track_name);
                }
            }
            else
            {
                tracksForplaylists(listBox.SelectedIndex);
            }
            ObjectProgram.updateListBoxPlaying();
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

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex > -1)
            {
                tracksForplaylists(listBox.SelectedIndex);
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public List<track> return_specific_tracks_from_library_playlists(String SearchText)
        {
            SearchText = SearchText.ToLower();
            List<track> templist = new List<track>();
            foreach (track item in PlaylistsList[listBox.SelectedIndex].returnPlayListTracks())
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
