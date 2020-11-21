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
using System.Reflection;
using BS_Utils.Utilities;
using HarmonyLib;

namespace OffsetPerMap
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        internal static Harmony harmony;

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

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
            Plugin.ApplyHarmonyPatches();
            BSEvents.lateMenuSceneLoadedFresh += this.BSEvents_menuSceneLoadedFresh;
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");
        }
        public static void ApplyHarmonyPatches()
        {
            try
            {
                Plugin.Log.Debug("Applying Harmony patches.");
                Plugin.harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Plugin.Log.Critical("Error applying Harmony patches: " + ex.Message);
                Plugin.Log.Debug(ex);
            }
        }

        private void BSEvents_menuSceneLoadedFresh(ScenesTransitionSetupDataSO data)
        {
            Plugin.Log.Info("OffsetPerMap - Menu Scene Was Loaded");
            PersistentSingleton<OffsetUI>.instance.Setup();
            new GameObject("OffsetPerMapController").AddComponent<OffsetPerMapController>();
        }
    }
}
