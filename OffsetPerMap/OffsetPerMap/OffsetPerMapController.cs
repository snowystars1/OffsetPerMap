using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace OffsetPerMap
{
    public class OffsetPerMapController : MonoBehaviour
    {
        public static OffsetPerMapController Instance { get; private set; }

        private void Start()
        {
            //Checks to see whether the "Solo Mode" button exists
            Button button = Resources.FindObjectsOfTypeAll<Button>().First((Button x) => x.name == "SoloButton");
            if (button == null)
            {
                return;
            }

            //Register a listener for the button so that when the player clicks the button, Initialize() will be called.
            button.onClick.AddListener(delegate ()
            {
                this.Initialize();
            });
        }

        private void Initialize()
        {
            BSUI.Initialize();

            BSUI.LevelDetailViewController.didChangeContentEvent -= this.OnLevelSelectChange;
            BSUI.LevelDetailViewController.didChangeContentEvent += this.OnLevelSelectChange;
        }

        private void OnLevelSelectChange(StandardLevelDetailViewController sldvc, StandardLevelDetailViewController.ContentType contentType)
        {
            if (contentType != StandardLevelDetailViewController.ContentType.Loading)
            {
                this.Refresh();
            }
        }

        private void Refresh()
        {
            IPA.Logging.Logger log = Plugin.Log;
            if (log != null)
            {
                log.Info("This is where we will load new data from the config file!");
                //Read the NJS from the file and set the new player settings
                //PluginConfig config = PluginConfig.Instance;
                //OffsetUI offsetUI = OffsetUI.Instance;

                //CustomPreviewBeatmapLevel thisLevel = (sldvc.selectedDifficultyBeatmap is CustomPreviewBeatmapLevel)
                //? sldvc.selectedDifficultyBeatmap as CustomPreviewBeatmapLevel
                //: null;

                //int index = config.songList.IndexOf(thisLevel.levelID);
                //if(index == -1)
                //{
                //    //Defaults
                //    offsetUI.SelectNJS(2);
                //}
                //else
                //{
                //    //Set new shit
                //    int newIndex = config.njsOffsetList.ElementAt(index);
                //    offsetUI.SelectNJS(newIndex);
                //}
            }
        }
    }
}
