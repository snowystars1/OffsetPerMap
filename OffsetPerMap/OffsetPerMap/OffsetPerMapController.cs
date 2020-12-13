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
        public static Dictionary<string, SongAndNJS> songs = new Dictionary<string, SongAndNJS>();
        private StandardLevelDetailViewController standardLevel;
        IPA.Logging.Logger log = Plugin.Log;

        private void Start()
        {
            //Load in all the levels from the config file to the songs dictionary.
            foreach (SongAndNJS obj in PluginConfig.Instance.songAndNJSList)
            {
                songs.Add(obj.songID, obj);
            }

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
                standardLevel = sldvc;
                this.Refresh();
            }
        }

        private void Refresh()
        {
            if (standardLevel is null)
            {
                return;
            }

            //Check to make sure its not null
            IPA.Logging.Logger log = Plugin.Log;
            if (log is IPA.Logging.Logger)
            {

                //Read the NJS from the file and set the new player settings
                PluginConfig config = PluginConfig.Instance;
                OffsetUI offsetUI = OffsetUI.Instance;

                try
                {
                    //Get a reference to the song we just switched to
                    IDifficultyBeatmap beatmap = standardLevel.selectedDifficultyBeatmap;
                    if (beatmap is null)
                    {
                        this.log.Info("Beatmap was null in OffsetPerMapController.Refresh()");
                        return;
                    }

                    //Read from the dictionary to find the NJS of the song we just switched to
                    SongAndNJS obj;
                    songs.TryGetValue(beatmap.level.levelID, out obj);
                    if (obj != null)
                    {
                        offsetUI.njsButtonText.text = obj.njsChoice;
                        offsetUI.chosenOffsetString = obj.njsChoice;
                        switch (obj.njsChoice)
                        {
                            case "Far":
                                offsetUI.offsetNumber = 0.5f;
                                break;
                            case "Further":
                                offsetUI.offsetNumber = 0.25f;
                                break;
                            case "Default":
                                offsetUI.offsetNumber = 0.0f;
                                break;
                            case "Closer":
                                offsetUI.offsetNumber = -0.25f;
                                break;
                            case "Close":
                                offsetUI.offsetNumber = -0.5f;
                                break;
                        }
                        offsetUI.ApplyPlayerSettings();
                        offsetUI.saveButtonText.text = "Saved!";
                        offsetUI.saveButtonText.fontSize = 3;
                    }
                    else
                    {
                        offsetUI.saveButtonText.text = "Save";
                        offsetUI.saveButtonText.fontSize = 4;
                        //offsetUI.njsButtonText.text = "NJS";
                        //offsetUI.njsButtonText.fontSize = 4;
                        //offsetUI.chosenOffsetString = "Default";
                        //offsetUI.offsetNumber = 0.0f;
                        //offsetUI.applyPlayerSettings();
                    }
                }
                catch (Exception e)
                {
                    this.log.Info(e.StackTrace);
                    return;
                }
            }
        }
    }
}
