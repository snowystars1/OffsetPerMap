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
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
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
            }
        }
    }
}
