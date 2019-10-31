using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WMPLib;
using System.Windows.Controls;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace Final_JukeBox_Application.Pages
{
    public class JukeBoxApp
    {
        private List<playlist> MyPlayLists;
        private List<track> library;
        private WindowsMediaPlayer wplayer = new WindowsMediaPlayer();
        private track CurrentTrackPlaying;

        private DispatcherTimer dt = new DispatcherTimer(); //for auto play
        private Stopwatch stopWatchEventForAutoPLay = new Stopwatch();

        private DispatcherTimer dt2 = new DispatcherTimer();//for stopwatch main (track timers)
        private Stopwatch TrackTimer = new Stopwatch();
        private string currentTime = string.Empty;

        private ListBox listboxNavigation;
        private List<Label> trackPLayingLabels = new List<Label>();
        private List<Label> trackTimerLabels = new List<Label>();

        public artist ArtistObject { get; set; }
        public album AlbumObject { get; set; }
        public genre GenreObject { get; set; }
        public playlists PLaylistsObject { get; set; }
        public addtoplaylist UpdatePLayListObject{get;set;}
        public void updatePages()
        {
            if(ArtistObject != null)
            {
                ArtistObject.load_artists();
            }
            if (AlbumObject != null)
            {
                AlbumObject.load_albums();
            }
            if (GenreObject != null)
            {
                GenreObject.load_genres();
            }
            if (PLaylistsObject != null)
            {
                PLaylistsObject.load_playlists();
            }
        }

        public void updatePlayListOptions()
        {
            if (UpdatePLayListObject != null)
            {
                UpdatePLayListObject.updateComboBox();
            }
        }
        //add interface items
        public void addLibraryListBox(ListBox x)
        {
            listboxNavigation = x;
        }
        public void AddTrackPlayingLabel(Label x)
        {
            trackPLayingLabels.Add(x);
        }
        public void AddTrackTimerLabels(Label x)
        {
            trackTimerLabels.Add(x);
        }
        //end

        public JukeBoxApp()
        {
            //event auto track timer
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dt.Start();
            //end

            //stopclock
            dt2.Tick += new EventHandler(dt_Tick2);
            dt2.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dt2.Start();
            //stopclock 

            wplayer.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(wplayer_PlayStateChange);

            this.MyPlayLists = this.loadPlayLists();
            this.library = this.loadLibrary();
            bool val_check = false;
            bool val_check2 = false;

            if (this.MyPlayLists == null)
            {
                this.MyPlayLists = new List<playlist>();

               // System.Windows.MessageBox.Show("Error Loading Playlists - playlists.XML Missing. Don't worry though your programmer re-made it for you! :)","XML Error 101");
            }
            else
            {
                val_check = this.validate_playlists();
            }

            if (this.library == null)
            {
                this.library = new List<track>();
                //System.Windows.MessageBox.Show("Error Loading Library - track_library.XML Missing. Don't worry though your programmer re-made it for you! :)", "XML Error 102");
            }
            else
            {
                val_check2 = this.validate_track_paths();
            }

            if (val_check == true || val_check2==true)
            {
                System.Windows.MessageBox.Show("Error Loading Library - Some track(s) have been removed from your library as they not longer exist; the track(s) may have been renamed, moved or deleted.", "Library Error 109");
                this.save();
            }
        }

        private void wplayer_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                stopWatchEventForAutoPLay.Start();
            }
        }

        void dt_Tick2(object sender, EventArgs e)
        {
            if (TrackTimer.IsRunning)
            {
                TimeSpan ts = TrackTimer.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
                foreach (Label item in trackTimerLabels)
                {
                    item.Content = currentTime;
                }
                foreach (Label item in trackPLayingLabels)
                {
                    item.Content = "Playing - " + CurrentTrackPlaying.track_name;
                }
            }
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (stopWatchEventForAutoPLay.IsRunning)
            {
                stopWatchEventForAutoPLay.Stop();
                AutoPlayNextTrack();
            }
        }

        public void stopTrack()
        {
            wplayer.controls.stop();
            foreach (Label item in trackPLayingLabels)
            {
                item.Content = "";
            }
            foreach (Label item in trackTimerLabels)
            {
                item.Content = "";
            }
            TrackTimer.Stop();
            TrackTimer.Reset();
        }
        public void pauseTrack()
        {
            wplayer.controls.pause();
            TrackTimer.Stop();
        }
        public void playTrack(track x)
        {
            try
            {
                if (CurrentTrackPlaying == null)
                {
                    wplayer.URL = x.path;
                    wplayer.controls.play();
                    CurrentTrackPlaying = x;
                    foreach (Label item in trackPLayingLabels)
                    {
                        item.Content = "Playing - " + CurrentTrackPlaying.track_name;
                    }
                    TrackTimer.Stop();
                    TrackTimer.Reset();
                    TrackTimer.Start();
                }
                else if (CurrentTrackPlaying.path != x.path ) {
                    wplayer.URL = x.path;
                    wplayer.controls.play();
                    CurrentTrackPlaying = x;
                    foreach (Label item in trackPLayingLabels)
                    {
                        item.Content = "Playing - " + CurrentTrackPlaying.track_name;
                    }
                    TrackTimer.Stop();
                    TrackTimer.Reset();
                    TrackTimer.Start();
                }
                else
                {
                    foreach (Label item in trackPLayingLabels)
                    {
                        item.Content = "Playing - " + CurrentTrackPlaying.track_name;
                    }
                    wplayer.controls.play();
                    TrackTimer.Start();
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Error: Cannot play track", "Library Error 109AB");
            }
        }

        public void playTrack()
        {
            if (CurrentTrackPlaying !=null) {
                wplayer.controls.play();
                TrackTimer.Start();
                updateListBoxPlaying();
            }
        }

        public void updateListBoxPlaying()
        {
            if (CurrentTrackPlaying != null) {
                foreach (String item in listboxNavigation.Items)
                {
                    if (item == CurrentTrackPlaying.track_name)
                    {
                        listboxNavigation.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public void AutoPlayNextTrack()
        {
            if (CurrentTrackPlaying != null)
            {
                int count = -1;
                foreach (string item in listboxNavigation.Items)
                {
                    if (item == CurrentTrackPlaying.track_name)
                    {
                        count = listboxNavigation.SelectedIndex + 1;
                        break;
                    }
                }
                if (listboxNavigation.Items.Count > 1 && listboxNavigation.Items.Count > count && count != -1)
                {
                    String name = listboxNavigation.Items[count].ToString();
                    listboxNavigation.SelectedIndex = listboxNavigation.SelectedIndex + 1;
                    var listBoxItem = (ListBoxItem)listboxNavigation.ItemContainerGenerator.ContainerFromItem(listboxNavigation.SelectedItem);
                    try {
                        listBoxItem.Focus();
                    }
                    catch
                    {

                    }
                    track next = this.return_track_by_name(name);
                    CurrentTrackPlaying = next;
                    foreach (Label item in trackPLayingLabels)
                    {
                        item.Content = "Playing - " + CurrentTrackPlaying.track_name;
                    }
                    TrackTimer.Stop();
                    TrackTimer.Reset();
                    TrackTimer.Start();
                    wplayer.URL = next.path;
                    wplayer.controls.play();
                }
                else
                {
                    TrackTimer.Stop();
                }
            }
        }


        public void PlayPreviousTrack()
        {
            if (CurrentTrackPlaying != null)
            {
                int count = -1;
                foreach (string item in listboxNavigation.Items)
                {
                    if (item == CurrentTrackPlaying.track_name)
                    {
                        count = listboxNavigation.SelectedIndex - 1;
                        break;
                    }
                }
                if (listboxNavigation.Items.Count > 1 && count > -1)
                {
                    String name = listboxNavigation.Items[count].ToString();
                    listboxNavigation.SelectedIndex = listboxNavigation.SelectedIndex - 1;
                    var listBoxItem = (ListBoxItem)listboxNavigation.ItemContainerGenerator.ContainerFromItem(listboxNavigation.SelectedItem);
                    listBoxItem.Focus();
                    track previous = this.return_track_by_name(name);
                    wplayer.URL = previous.path;
                    wplayer.controls.play();
                    CurrentTrackPlaying = previous;
                    foreach (Label item in trackPLayingLabels)
                    {
                        item.Content = "Playing - " + CurrentTrackPlaying.track_name;
                    }
                    TrackTimer.Stop();
                    TrackTimer.Reset();
                    TrackTimer.Start();
                }
            }
        }
        public bool add_track_to_library(track x)
        {
            bool check = true;
            foreach (track item in this.library)
            {
                if (item.path == x.path)
                {
                    check = false;
                    break;
                }
            }
            if (check == true)
            {
                foreach (track item in this.library)
                {
                    if (item.track_name == x.track_name)
                    {
                        Random Gen = new Random();
                        x.track_name = x.track_name + " - DUPE NAME FIX - " + Gen.Next(1,1000000);
                        break;
                    }
                }
                this.library.Add(x);
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<track> return_all_tracks_from_library()
        {
            return this.library;
        }

        public List<track> return_specific_tracks_from_library(String SearchText)
        {
            SearchText = SearchText.ToLower();
            List<track> templist = new List<track>();
            foreach(track item in this.library)
            {
                if ((item.track_name.ToLower()).Contains(SearchText))
                {
                    templist.Add(item);
                }
            }
            return templist;
        }

        public void changeVolume(int i)
        {
            wplayer.settings.volume = i * 10;
        }
       public track return_track_by_name(String track_name) 
       {
            foreach (track item in this.library)
            {
                if (item.track_name.Equals(track_name))
                {
                    return item;
                }
            }
            return null;
        }

        public void remove_track_from_library(track x)
        {
            this.library.Remove(x);
        }

        public void addPlayList(playlist x)
        {
            this.MyPlayLists.Add(x);
        }
        public void removePLayList(int x)
        {
            this.MyPlayLists.RemoveAt(x);
        }

        public void renamePlatList(String name, int x)
        {
            this.MyPlayLists[x].playListName = name;
        }

        public List<playlist> returnAllPlaylists()
        {
            return this.MyPlayLists;
        }

        public List<track> loadLibrary()
        {
            try {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<track>));
                TextReader textReader = new StreamReader(Directory.GetCurrentDirectory() + @"\track_library.xml");
                List<track> tempList = (List<track>)deserializer.Deserialize(textReader);
                textReader.Close();
                return tempList;
            }
            catch
            {
                return null;
            }
        }

        public List<playlist> loadPlayLists()
        {
            try {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<playlist>));
                TextReader textReader = new StreamReader(Directory.GetCurrentDirectory() + @"\playlists.xml");
                List<playlist> tempList = (List<playlist>)deserializer.Deserialize(textReader);
                textReader.Close();
                return tempList;
            }
            catch
            {
                return null;
            }
        }

        public bool save()
        {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(List<playlist>));
                TextWriter textWriter = new StreamWriter(Directory.GetCurrentDirectory() + @"\playlists.xml");
                serializer.Serialize(textWriter, this.MyPlayLists);
                textWriter.Close();

                XmlSerializer serializer2 = new XmlSerializer(typeof(List<track>));
                TextWriter textWriter2 = new StreamWriter(Directory.GetCurrentDirectory() + @"\track_library.xml");
                serializer2.Serialize(textWriter2, this.library);
                textWriter2.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool validate_track_paths()
        {
            bool check = false;
            List<track> tracksToRemove = new List<track>();
            foreach (track item in this.library)
            {
                bool x = File.Exists(item.path);
                if (x == false)
                {
                    tracksToRemove.Add(item);
                }
            }
            foreach (track item2 in tracksToRemove)
            {
                this.library.Remove(item2);
            }
            if (tracksToRemove.Count > 0)
            {
                check = true;
            }
            return check;
        }

        public playlist returnSpecificPlayList(String name)
        {
            foreach(playlist item in MyPlayLists)
            {
                if (item.playListName == name)
                {
                    return item;
                }
            }
            return null;
        }
        private bool validate_playlists()
        {
            bool check = false;
            foreach (playlist item in this.MyPlayLists)
            {
                List<track> tracksToRemove = new List<track>();
                foreach (track item2 in item.returnPlayListTracks())
                {
                    bool x = File.Exists(item2.path);
                    if (x == false)
                    {
                        tracksToRemove.Add(item2);
                    }
                }
                foreach(track item3 in tracksToRemove)
                {
                    item.remove_track_from_playlist(item3);
                }
                if (tracksToRemove.Count>0)
                {
                    check = true;
                }
            }
            return check;
        }
    }
}
