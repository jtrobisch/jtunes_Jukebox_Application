using HundredMilesSoftware.UltraID3Lib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;

namespace Final_JukeBox_Application.Pages
{
    /// <summary>
    /// Interaction logic for library.xaml
    /// </summary>
    public partial class library : UserControl
    {

        private JukeBoxApp ObjectProgram = (App.Current as App).GlobalJukeBox;
        private List<track> TempSearch = new List<track>();

        
        public library()
        {
            InitializeComponent();
            ObjectProgram.addLibraryListBox(listBox);
            ObjectProgram.AddTrackPlayingLabel(PlayLabel);
            ObjectProgram.AddTrackTimerLabels(timerLabel);

            listBox.SelectionMode = SelectionMode.Extended;
            listBox.Items.Clear();

            foreach (track item in ObjectProgram.return_all_tracks_from_library())
            {
                listBox.Items.Add(item.track_name);
            }

            try {
                var uri = new Uri("pack://application:,,,/Images/SearchFinal.png");
                var bitmap = new BitmapImage(uri);
                image.Source = bitmap;
            }
            catch
            {
                System.Windows.MessageBox.Show("Error Loading Icons: Search icon missing.", "Image Error 1.13H");
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            String[] choosen_files;
            String[] track_names;
            //try {
            //    openFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            //}
            //catch
            //{
            //    openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            //}
           

            openFileDialog.Title = "Choose MP3's or Mpeg4";
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Select Tracks (MP3s, Mpeg4s) | *.mp3; *.m4a;";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                choosen_files = openFileDialog.FileNames;
                track_names = openFileDialog.SafeFileNames;
                int counter = 0;
                foreach (string item in choosen_files)
                {
                    track x = new track();
                    x.path = item;
                    x.track_name = track_names[counter];

                    try {
                        TagLib.File file = TagLib.File.Create(item);
                        if (file.Tag.Album != null && file.Tag.Album.Length > 0)
                        {
                            x.album_name = file.Tag.Album;
                        }
                        else
                        {
                            x.album_name = "";
                        }
                        if (file.Tag.Performers != null && file.Tag.Performers.Length > 0)
                        {
                            x.artist_name = file.Tag.Performers[0];
                        }
                        else
                        {
                            x.artist_name = "";
                        }
                        x.year_published = file.Tag.Year.ToString();
                        if (file.Tag.Genres != null && file.Tag.Genres.Length > 0)
                        {
                            x.genre = file.Tag.Genres[0];
                        }
                        else
                        {
                            x.genre = "";
                        }
                        x.comments = file.Tag.Comment;
                        x.duration = file.Properties.Duration;
                        //UltraID3 u = new UltraID3();
                        //u.Read(item);
                        //x.album_name = u.Album;
                        //x.artist_name = u.Artist;
                        //x.year_published = u.Year.ToString();
                        //x.genre = u.Genre;
                        //x.comments = u.Comments;
                        //x.duration = u.Duration;

                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("Error Loading Track: Meta Data Read Error :)", "Meta Data Error 11.1BV");
                    }
                    bool check = ObjectProgram.add_track_to_library(x);
                    if (check == false)
                    {
                        System.Windows.MessageBox.Show("Error Loading Track: " + track_names[counter] + " is already in your library! :)", "Library Error 1.1");
                    }
                    counter++;
                }
                ObjectProgram.save();
                ObjectProgram.updatePages();
                ObjectProgram.updatePlayListOptions();
            }
            listBox.Items.Clear();
            foreach (track item in ObjectProgram.return_all_tracks_from_library())
            {
                listBox.Items.Add(item.track_name);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex > -1)
            {
                List<track> deleteList = new List<track>();
                foreach (string item in listBox.SelectedItems)
                {
                    track check = ObjectProgram.return_track_by_name(item);//path better
                    if (item == check.track_name)
                    {
                        deleteList.Add(check);
                    }
                }
                foreach (track item in deleteList)
                {
                    ObjectProgram.remove_track_from_library(item);
                    foreach (playlist item2 in ObjectProgram.returnAllPlaylists())
                    {
                        track er = item2.returnSingleTrackFromPlaylist(item.track_name);
                        item2.remove_track_from_playlist(er);
                    }
                }
                ObjectProgram.save();
                listBox.Items.Clear();
                if (searchTrack.Text != "")
                {
                    searchTrack.Clear();
                }
                else
                {
                    foreach (track item in ObjectProgram.return_all_tracks_from_library())
                    {
                        listBox.Items.Add(item.track_name);
                    }
                }

            }
            ObjectProgram.updatePages();
            ObjectProgram.updatePlayListOptions();

        }
        private void searchTrack_TextChanged(object sender, TextChangedEventArgs e)
        {
            TempSearch.Clear();
            listBox.Items.Clear();
            if (searchTrack.GetLineLength(0) > 0)
            {
                TempSearch = ObjectProgram.return_specific_tracks_from_library(searchTrack.Text);
                foreach (track item in TempSearch)
                {
                    listBox.Items.Add(item.track_name);
                }
            }
            else
            {
                foreach (track item in ObjectProgram.return_all_tracks_from_library())
                {
                    listBox.Items.Add(item.track_name);
                }
            }
            ObjectProgram.updateListBoxPlaying();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex>-1)
            {
                ObjectProgram.addLibraryListBox(listBox);
                track play = new track();
                play = ObjectProgram.return_track_by_name(listBox.Items[listBox.SelectedIndex].ToString());
                ObjectProgram.playTrack(play);
            }
            else
            {
                 ObjectProgram.playTrack();
            }
        }
        private void PlayEventDoubleClick(object sender, RoutedEventArgs e)
        {
            ObjectProgram.addLibraryListBox(listBox);
            track play = new track();
            play = ObjectProgram.return_track_by_name(listBox.Items[listBox.SelectedIndex].ToString());
            ObjectProgram.playTrack(play);
        }

        private void button1_copy_Click(object sender, RoutedEventArgs e)
        {
            listBox.SelectedIndex = -1;
            ObjectProgram.pauseTrack();
        }

        private void button1_copy_2(object sender, RoutedEventArgs e)
        {
            listBox.SelectedIndex = -1;
            ObjectProgram.stopTrack();
        }

        private void button1_copy_2_3(object sender, RoutedEventArgs e)
        {
            ObjectProgram.addLibraryListBox(listBox);
            ObjectProgram.PlayPreviousTrack();
        }

        private void button1_copy_2_4(object sender, RoutedEventArgs e)
        {
            ObjectProgram.addLibraryListBox(listBox);
            ObjectProgram.AutoPlayNextTrack();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ObjectProgram.changeVolume((int)slider.Value);
        }
    }
}
