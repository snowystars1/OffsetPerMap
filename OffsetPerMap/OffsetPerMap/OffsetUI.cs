using System;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using UnityEngine;
using System.Collections.Generic;


using TMPro;

namespace OffsetPerMap
{
	public class OffsetUI : BSMLResourceViewController
	{
		
		public override string ResourceName {
            get
            {
				return "OffsetPerMap.Views.View1.bsml";
			}
		}
		
		[UIComponent("modal")]
		private ModalView modal;

		[UIComponent("list")]
		public CustomListTableData njsOptionsList;

        [UIComponent("NJSButton")]
        private Transform njsButtonTransform;

        [UIComponent("NJSButton")]
		public TextMeshProUGUI njsButtonText;

		[UIComponent("SaveButton")]
		private Transform saveButtonTransform;

		[UIComponent("SaveButton")]
		public TextMeshProUGUI saveButtonText;

		[UIValue("chosenNJS")]
		public string chosenOffsetString { get; set; } = "Default";

		public float offsetNumber { get; set; } = 0.0f;

		public static OffsetUI Instance { get; set; }
        IPA.Logging.Logger log = Plugin.Log;
		private StandardLevelDetailViewController standardLevel;
		private GameplaySetupViewController gameplaySetup;
		private PlayerSettingsPanelController playerSettingsPanel;
		public CustomPreviewBeatmapLevel level;
		private Sprite farIcon;
		private Sprite furtherIcon;
		private Sprite defaultIcon;
		private Sprite closerIcon;
		private Sprite closeIcon;

        [UIAction("njs-button-click")]
		internal void ShowNJSOptions()
		{
            Plugin.Log.Debug("Show Playlists");
            bool flag = this.njsOptionsList.data != null;
            if (flag)
            {
                this.njsOptionsList.data.Clear();
            }
			this.njsOptionsList.data.Add(new CustomListTableData.CustomCellInfo("Far", null, farIcon));
			this.njsOptionsList.data.Add(new CustomListTableData.CustomCellInfo("Further", null, furtherIcon));
			this.njsOptionsList.data.Add(new CustomListTableData.CustomCellInfo("Default", null, defaultIcon));
			this.njsOptionsList.data.Add(new CustomListTableData.CustomCellInfo("Closer", null, closerIcon));
			this.njsOptionsList.data.Add(new CustomListTableData.CustomCellInfo("Close", null, closeIcon));
			this.njsOptionsList.tableView.ReloadData();
            this.njsOptionsList.tableView.ScrollToCellWithIdx(2, TableViewScroller.ScrollPositionType.Center, false);
		}

		[UIAction("select-NJS")]
		internal void SelectNJS(TableView tableView, int idx)
		{
			if(standardLevel == null)
            {
				return;
            }
            //Set the player NJSOffset to whatever was chosen

            //Set the chosenNJS string so the player can see the NJS without opening the modal.
            //The chosenNJS string appears on the njsButton from now on
            try
            {
				switch (idx)
				{
					case 0:
						chosenOffsetString = "Far";
						offsetNumber = 0.5f;
						break;
					case 1:
						chosenOffsetString = "Further";
						offsetNumber = 0.25f;
						break;
					case 2:
						chosenOffsetString = "Default";
						offsetNumber = 0.0f;
						break;
					case 3:
						chosenOffsetString = "Closer";
						offsetNumber = -0.25f;
						break;
					case 4:
						chosenOffsetString = "Close";
						offsetNumber = -0.5f;
						break;
				}
				this.log.Info("NJS Selected!");
				this.modal.Hide(true, null);
				njsButtonText.text = chosenOffsetString;
				saveButtonText.text = "Save";
				saveButtonText.fontSize = 4;

				//Apply new player settings
				ApplyPlayerSettings();

			}
            catch(NullReferenceException e)
            {
				this.log.Info(e.StackTrace);
            }
        }

		[UIAction("save-to-file")]
		internal void SaveToFile()
        {
            try
            {
				IDifficultyBeatmap beatmap = standardLevel.selectedDifficultyBeatmap;
				if (beatmap is null)
				{
					this.log.Info("Beatmap was null in OffsetUI.SelectNJS()");
					return;
				}

				//Search to see if this level is already in the song list
				PluginConfig config = PluginConfig.Instance;
				int listIndex;
				SongAndNJS obj;
				if (OffsetPerMapController.songs.TryGetValue(beatmap.level.levelID, out obj))
				{
					listIndex = obj.index;
					//Alter the dictionary
					obj.njsChoice = chosenOffsetString;

					//Alter the plugin config
					SongAndNJS temp = config.songAndNJSList.ElementAt(listIndex);
					temp.njsChoice = chosenOffsetString;
				}
				else
				{
					//Add new song to the plugin config
					SongAndNJS songInfo = new SongAndNJS();
					songInfo.songID = beatmap.level.levelID;
					songInfo.njsChoice = chosenOffsetString;
					songInfo.index = config.songAndNJSList.Count;
					config.songAndNJSList.Add(songInfo);

					//Add new song to the dictionary
					OffsetPerMapController.songs.Add(beatmap.level.levelID, songInfo);
				}
				saveButtonText.text = "Saved!";
				saveButtonText.fontSize = 3;
			}
			catch(Exception e)
            {
				this.log.Info(e.StackTrace);
            }
        }

		public void ApplyPlayerSettings()
        {
			if(gameplaySetup is null || playerSettingsPanel is null)
            {
				return;
            }
			PlayerSpecificSettings oldSettings = gameplaySetup.playerSettings;
			PlayerSpecificSettings newSettings = new PlayerSpecificSettings(
				oldSettings.staticLights,
				oldSettings.leftHanded,
				oldSettings.playerHeight,
				oldSettings.automaticPlayerHeight,
				oldSettings.sfxVolume,
				oldSettings.reduceDebris,
				oldSettings.noTextsAndHuds,
				oldSettings.noFailEffects,
				oldSettings.advancedHud,
				oldSettings.autoRestart,
				oldSettings.saberTrailIntensity,
				offsetNumber,
				oldSettings.hideNoteSpawnEffect,
				oldSettings.adaptiveSfx);
			playerSettingsPanel.SetData(newSettings);
		}

		internal void Setup()
		{
			//Get references to controllers
			gameplaySetup = Resources.FindObjectsOfTypeAll<GameplaySetupViewController>().First<GameplaySetupViewController>();
			playerSettingsPanel = Resources.FindObjectsOfTypeAll<PlayerSettingsPanelController>().First<PlayerSettingsPanelController>();

			Color farColor = new Color(0.827f, 0.164f, 0.172f);
			Color furtherColor = new Color(0.921f, 0.611f, 0.615f);
			Color defaultColor = new Color(0.588f, 0.588f, 0.588f);
			Color closerColor = new Color(0.611f, 0.921f, 0.917f);
			Color closeColor = new Color(0.164f, 0.827f, 0.819f);

			//Create Sprites for the icons in the NJS modal list
			Texture2D farTexture = CreateTexture(farColor);
			farIcon = Sprite.Create(farTexture, new Rect(0.0f, 0.0f, farTexture.width, farTexture.height), new Vector2(0.0f, 0.0f));

			Texture2D furtherTexture = CreateTexture(furtherColor);
			furtherIcon = Sprite.Create(furtherTexture, new Rect(0.0f, 0.0f, furtherTexture.width, furtherTexture.height), new Vector2(0.0f, 0.0f));

			Texture2D defaultTexture = CreateTexture(defaultColor);
			defaultIcon = Sprite.Create(defaultTexture, new Rect(0.0f, 0.0f, defaultTexture.width, defaultTexture.height), new Vector2(0.0f, 0.0f));

			Texture2D closerTexture = CreateTexture(closerColor);
			closerIcon = Sprite.Create(closerTexture, new Rect(0.0f, 0.0f, closerTexture.width, closerTexture.height), new Vector2(0.0f, 0.0f));

			Texture2D closeTexture = CreateTexture(closeColor);
			closeIcon = Sprite.Create(closeTexture, new Rect(0.0f, 0.0f, closeTexture.width, closeTexture.height), new Vector2(0.0f, 0.0f));

			//Instantiate the BSML
			this.standardLevel = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().First<StandardLevelDetailViewController>();
			GameObject gameObject = this.standardLevel.transform.Find("LevelDetail").gameObject;
			PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "OffsetPerMap.Views.View1.bsml"), gameObject.transform.Find("ActionButtons").gameObject, this);
			this.njsButtonTransform.localScale *= 0.7f;
			this.saveButtonTransform.localScale *= 0.7f;
		}

		private Texture2D CreateTexture(Color color)
        {
			Texture2D texture = new Texture2D(128, 128);
			// colors used to tint the first 3 mip levels
			//var mipCount = Mathf.Min(1, farTexture.mipmapCount);
			var mipCount = texture.mipmapCount;
			// tint each mip level
			for (var mip = 0; mip < mipCount; ++mip)
			{
				var cols = texture.GetPixels32(mip);
				for (var i = 0; i < cols.Length; ++i)
				{
					cols[i] = Color32.Lerp(cols[i], color, 1f);
				}
				texture.SetPixels32(cols, mip);
			}
			texture.Apply(false);
			return texture;
		}
	}
}