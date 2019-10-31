using HundredMilesSoftware.UltraID3Lib;
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

namespace Final_JukeBox_Application.Pages.Settings
{
    /// <summary>
    /// Interaction logic for updateTrackInfo.xaml
    /// </summary>
    public partial class updateTrackInfo : UserControl
    {
        private JukeBoxApp ObjectProgram = (App.Current as App).GlobalJukeBox;
        private List<track> TempSearch = new List<track>();
        public updateTrackInfo()
        {
            InitializeComponent();
            listBox.SelectionMode = SelectionMode.Extended;
            listBox.Items.Clear();
            foreach (track item in ObjectProgram.return_all_tracks_from_library())
            {
                listBox.Items.Add(item.track_name);
            }
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

        private void searchTrack_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<track> TempSearch = new List<track>();
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
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex > -1)
            {
                track x = ObjectProgram.return_track_by_name(listBox.Items[listBox.SelectedIndex].ToString());
                textBox.Text = x.artist_name;
                textBox2.Text = x.album_name;
                textBox3.Text = x.genre;
            }
            else
            {
                textBox.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex > -1)
            {
                foreach(String item in listBox.SelectedItems)
                {
                    track x = ObjectProgram.return_track_by_name(item);
                    x.album_name = textBox2.Text;
                    x.artist_name = textBox.Text;
                    x.genre = textBox3.Text;
                    ObjectProgram.save();
                    try {
                        TagLib.File file = TagLib.File.Create(x.path);
                        file.Tag.Album = x.album_name;
                        file.Tag.Performers = new[] { x.artist_name };
                        file.Tag.Genres = new[] { x.genre };
                        file.Save();
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("Error Saving Meta Data: Cannot update track information.", "Meta Data Error 12.13HGHB");
                    }
                }
                listBox.SelectedIndex = -1;
                ObjectProgram.updatePages();
                textBox.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }
    }
}