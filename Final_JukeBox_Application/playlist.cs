using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Final_JukeBox_Application.Pages
{
    [Serializable]
    [XmlRoot("PlayList")]
    public class playlist
    {
        [XmlArrayItemAttribute("Play_List_Tracks")]
        public List<track> MyPlayList = new List<track>();  //list of tracks

        [XmlElement("Play_List_Name")]
        public String playListName = ""; //play lsit name
        public playlist()
        {

        }
        public playlist(String name)
        {
            this.playListName = name;  
        }
        public bool add_track_to_playlist(track x)
        {
            bool check = true;
            foreach(track item in this.MyPlayList)
            {
                if (item.path == x.path)
                {
                    check = false;
                    break;
                }
            }
            if (check == true) {
                this.MyPlayList.Add(x);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool add_track_to_playlist(track x, int i)
        {
            bool check = true;
            foreach (track item in this.MyPlayList)
            {
                if (item.path == x.path)
                {
                    check = false;
                    break;
                }
            }
            if (check == true)
            {
                this.MyPlayList.Insert(i,x);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void remove_track_from_playlist(track x)
        {
            MyPlayList.Remove(x);
        }
        public List<track> returnPlayListTracks()
        {
            return this.MyPlayList;
        }

        public track returnSingleTrackFromPlaylist(String trackName)
        {
            foreach (track item in MyPlayList)
            {
                if(item.track_name == trackName)
                {
                    return item;
                }
            }
            return null;
        }
        public void setPlayListName(string x)
        {
            this.playListName = x;
        }
        public string getPlayListName()
        {
            return this.playListName;
        }
    }
}
