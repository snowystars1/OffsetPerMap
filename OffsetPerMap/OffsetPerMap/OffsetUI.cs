using System;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using UnityEngine;


using TMPro;

namespace OffsetPerMap
{
	public class OffsetUI : PersistentSingleton<OffsetUI>
	{

		//public static OffsetUI Instance { get; private set; }

		IPA.Logging.Logger log = Plugin.Log;

		//[UIComponent("NJSButton")]
		//public TextMeshProUGUI njsButtonText { get; set; }
		[UIComponent("NJSButton")]
		private TextMeshProUGUI njsButtonText;

		[UIValue("chosenNJS")] 
		public string chosenNJS { get; set;} = "NJS";

		public float njsBeatOffset { get; set; } = 0.0f;

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
			//Set the player NJSOffset to whatever was chosen

			//Set the chosenNJS string so the player can see the NJS without opening the modal.
			//The chosenNJS string appears on the njsButton from now on
			switch (idx)
            {
				case 0:
					chosenNJS = "Far";
					njsBeatOffset = 0.5f;
					break;
				case 1:
					chosenNJS = "Further";
					njsBeatOffset = 0.25f;
					njsButtonText.fontSize = 2;
					break;
				case 2:
					chosenNJS = "Default";
					njsBeatOffset = 0.0f;
					njsButtonText.fontSize = 2;
					break;
				case 3:
					chosenNJS = "Closer";
					njsBeatOffset = -0.25f;
					njsButtonText.fontSize = 2;
					break;
				case 4:
					chosenNJS = "Close";
					njsBeatOffset = -0.5f;
					njsButtonText.fontSize = 2;
					break;
            }
			this.log.Info("NJS Selected!");
			this.modal.Hide(true, null);
			njsButtonText.text = chosenNJS;

			GameplaySetupViewController gSVC = Resources.FindObjectsOfTypeAll<GameplaySetupViewController>().First<GameplaySetupViewController>();
			PlayerSettingsPanelController pSPC = Resources.FindObjectsOfTypeAll<PlayerSettingsPanelController>().First<PlayerSettingsPanelController>();
			PlayerSpecificSettings oldSettings = gSVC.playerSettings;
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
				njsBeatOffset,
				oldSettings.hideNoteSpawnEffect,
				oldSettings.adaptiveSfx);
			pSPC.SetData(newSettings);

			////save it to file based on level ID
			//PluginConfig config = PluginConfig.Instance;
			//config.njsOffsetList.Add(idx);

			//CustomPreviewBeatmapLevel thisLevel = (standardLevel.selectedDifficultyBeatmap is CustomPreviewBeatmapLevel) 
			//	? standardLevel.selectedDifficultyBeatmap as CustomPreviewBeatmapLevel 
			//	: null;

			//config.songList.Add(thisLevel.levelID);
		}

		internal void Setup()
		{
			//Instance = this;

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

		private StandardLevelDetailViewController standardLevel;

		public CustomPreviewBeatmapLevel level;

		private Sprite farIcon;
		private Sprite furtherIcon;
		private Sprite defaultIcon;
		private Sprite closerIcon;
		private Sprite closeIcon;

		[UIComponent("list")]
		public CustomListTableData njsOptionsList;

		[UIComponent("NJSButton")]
		private Transform njsButtonTransform;

		[UIComponent("modal")]
		private ModalView modal;
	}
}