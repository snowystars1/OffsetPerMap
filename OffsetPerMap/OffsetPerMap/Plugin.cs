using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using BS_Utils;
using BS_Utils.Gameplay;
using BS_Utils.Utilities;

namespace OffsetPerMap
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            Log.Info("OffsetPerMap initialized.");
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnStart()
        {
            Log.Debug("OnApplicationStart");
            BSEvents.lateMenuSceneLoadedFresh += delegate (ScenesTransitionSetupDataSO o)
            {
                Plugin.Log.Info("OffsetPerMap - Menu Scene Was Loaded");
                new GameObject("OffsetPerMapController").AddComponent<OffsetPerMapController>();
            };

        }

        [OnExit]
        public void OnExit()
        {
            Log.Debug("OnApplicationQuit");

        }
    }
}
