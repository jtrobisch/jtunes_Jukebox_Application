using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Final_JukeBox_Application.Pages
{
    [Serializable]
    [XmlRoot("Track")]
    public class track
    {
        public track()
        {

        }
        public track(string tname, string pname)//constructor
        {
            this.track_name = tname;
            this.path = pname;
        }

        [XmlElement("Track_Name")]
        public string track_name { get; set; }

        [XmlElement("Track_Path")]
        public string path { get; set; }

        [XmlElement("Artist_Name")]
        public string artist_name { get; set; }

        [XmlElement("Album_Name")]
        public string album_name { get; set; }
        
        [XmlElement("Year_Published")]
        public string year_published { get; set; }

        [XmlElement("Genre")]
        public string genre { get; set; }

        [XmlElement("Tracking_Rating")]
        public string tracking_rating { get; set; }

        [XmlElement("Duration")]
        public TimeSpan duration { get; set; }

        [XmlElement("Comments")]
        public string comments { get; set; }
    }
}
