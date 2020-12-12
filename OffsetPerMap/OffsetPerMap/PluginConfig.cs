using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

namespace OffsetPerMap
{
    public class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        [UseConverter(typeof(ListConverter<SongAndNJS>))]
        public virtual List<SongAndNJS> songAndNJSList { get; set; } = new List<SongAndNJS>();


        //[NonNullable]
        //public virtual SongAndNJS SongAndNJSDictionaryObject { get; set; } = new SongAndNJS();

        public virtual void Changed()
        {
            // this is called whenever one of the virtual properties is changed
            // can be called to signal that the content has been changed
        }

        public virtual void OnReload()
        {
            // this is called whenever the config file is reloaded from disk
            // use it to tell all of your systems that something has changed

            // this is called off of the main thread, and is not safe to interact
            //   with Unity in
        }
    }
    public class SongAndNJS
    {
        public string songID;
        public string njsChoice;
        public int index;
    }
}
