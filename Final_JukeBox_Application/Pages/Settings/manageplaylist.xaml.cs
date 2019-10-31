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
    /// Interaction logic for manageplaylist.xaml
    /// </summary>
    public partial class manageplaylist : UserControl
    {
        private JukeBoxApp ObjectProgram = (App.Current as App).GlobalJukeBox;
        private List<playlist> Playlists = new List<playlist>();
        public manageplaylist()
        {
            InitializeComponent();
            loadplaylists();

        }

        private void loadplaylists()
        {
            listBox.Items.Clear();
            Playlists.Clear();
            Playlists = ObjectProgram.returnAllPlaylists();
            foreach(playlist item in Playlists)
            {
                listBox.Items.Add(item.playListName);
            }
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.GetLineLength(0) > 0)
            {
                bool check = true;
                foreach(playlist item in Playlists)
                {
                    if(textBox.Text == item.getPlayListName())
                    {
                        check = false;
                        System.Windows.MessageBox.Show("Playlist Error: Name already exists.", "Playlist Error 9090A");
                        textBox.Clear();
                        break;
                    }
                    else
                    {
                        check = true;
                    }
                }
                if (check == true)
                {
                    playlist x = new playlist();
                    x.setPlayListName(textBox.Text);
                    ObjectProgram.addPlayList(x);
                    ObjectProgram.save();
                    ObjectProgram.updatePages();
                    ObjectProgram.updatePlayListOptions();
                    listBox.Items.Clear();
                    foreach (playlist item in Playlists)
                    {
                        listBox.Items.Add(item.playListName);
                    }
                    textBox.Clear();
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex>-1)
            {
                ObjectProgram.removePLayList(listBox.SelectedIndex);
                ObjectProgram.save();
                ObjectProgram.updatePages();
                ObjectProgram.updatePlayListOptions();
                listBox.Items.Clear();
                foreach (playlist item in Playlists)
                {
                    listBox.Items.Add(item.playListName);
                }
                textBox.Clear();
            }
        }
    }
}
