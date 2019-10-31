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
    /// Interaction logic for addtoplaylist.xaml
    /// </summary>
    public partial class addtoplaylist : UserControl
    {
        private JukeBoxApp ObjectProgram = (App.Current as App).GlobalJukeBox;
        private List<track> TempSearch = new List<track>();
        public addtoplaylist()
        {
            InitializeComponent();
            ObjectProgram.UpdatePLayListObject = this;
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
            listBox.SelectionMode = SelectionMode.Extended;
            listBox_Copy.SelectionMode = SelectionMode.Extended;
            updateComboBox();
        }
        public void updateComboBox()
        {
            comboBox.Items.Clear();
            foreach (playlist item in ObjectProgram.returnAllPlaylists())
            {
                comboBox.Items.Add(item.getPlayListName());
            }
            listBox.Items.Clear();
            listBox_Copy.Items.Clear();
        }
        public void updatelistboxes()
        {
            listBox.Items.Clear();
            listBox_Copy.Items.Clear();
            playlist x = ObjectProgram.returnSpecificPlayList(comboBox.SelectedItem.ToString());
            foreach (track item in ObjectProgram.return_all_tracks_from_library())
            {
                if ((x.returnSingleTrackFromPlaylist(item.track_name)) != null)
                {
                    listBox_Copy.Items.Add(item.track_name);
                }
                else
                {
                    listBox.Items.Add(item.track_name);
                }
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex > -1)
            {
                playlist x = ObjectProgram.returnSpecificPlayList(comboBox.SelectedItem.ToString());
                foreach (String item in listBox.SelectedItems)
                {
                    listBox_Copy.Items.Add(item);
                    track z = ObjectProgram.return_track_by_name(item);
                    x.add_track_to_playlist(z);
                }
                listBox_Copy.UnselectAll();
                foreach (String item in listBox_Copy.Items)
                {
                    listBox.Items.Remove(item);
                }
                listBox.UnselectAll();

                ObjectProgram.save();
                ObjectProgram.updatePages();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Copy.SelectedIndex > -1)
            {
                playlist x = ObjectProgram.returnSpecificPlayList(comboBox.SelectedItem.ToString());
                foreach (String item in listBox_Copy.SelectedItems)
                {
                    listBox.Items.Add(item);
                    track z = x.returnSingleTrackFromPlaylist(item);
                    x.remove_track_from_playlist(z);
                }
                listBox_Copy.UnselectAll();
                foreach (String item in listBox.Items)
                {
                    listBox_Copy.Items.Remove(item);
                }
                listBox.UnselectAll();

                ObjectProgram.save();
                ObjectProgram.updatePages();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex != -1)
            {
                updatelistboxes();
            }
        }

        private void searchTrack_TextChanged(object sender, TextChangedEventArgs e)
        {
         TempSearch.Clear();
            listBox.Items.Clear();
               if (searchTrack.GetLineLength(0) > 0 && comboBox.SelectedIndex > -1)
                {
                  
                  TempSearch = ObjectProgram.return_specific_tracks_from_library(searchTrack.Text);
                    playlist x = ObjectProgram.returnSpecificPlayList(comboBox.SelectedItem.ToString());
                    foreach (track item in x.returnPlayListTracks())
                    {
                        foreach (track item2 in TempSearch)
                        {
                            if (item.track_name == item2.track_name)
                            {
                                TempSearch.Remove(item2);
                                break;
                            }
                        }
                    }
                    foreach (track item in TempSearch)
                    {
                        listBox.Items.Add(item.track_name);
                    }
                }
                else
                {
                if (comboBox.SelectedIndex > -1)
                {
                    playlist x = ObjectProgram.returnSpecificPlayList(comboBox.SelectedItem.ToString());
                    foreach (track item in ObjectProgram.return_all_tracks_from_library())
                    {
                        if ((x.returnSingleTrackFromPlaylist(item.track_name)) != null)
                        {
                            // listBox_Copy.Items.Add(item.track_name);
                        }
                        else
                        {
                            listBox.Items.Add(item.track_name);
                        }
                    }
                }
                }
            
        }


    }
}
