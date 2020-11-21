using System;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using UnityEngine;

namespace OffsetPerMap
{
	public class OffsetUI : PersistentSingleton<OffsetUI>
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002A00 File Offset: 0x00000C00
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002A08 File Offset: 0x00000C08
		[UIValue("interactable")]
		public bool Interactable { get; set; } = true;

		// Token: 0x0600001B RID: 27 RVA: 0x00002A14 File Offset: 0x00000C14
		[UIAction("button-click")]
		internal void ShowPlaylists()
		{
			//Plugin.Log.Debug("Show Playlists");
			//bool flag = this.customListTableData.data != null;
			//if (flag)
			//{
			//	this.customListTableData.data.Clear();
			//}
			//this.playlists = PlaylistCollectionOverride.GetPlaylists();
			//Plugin.Log.Debug("after get  Playlists");
			//Plugin.Log.Debug("Playlist count: " + this.playlists.Length.ToString());
			//bool flag2 = this.playlists.Length != 0;
			//if (flag2)
			//{
			//	foreach (IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection in this.playlists)
			//	{
			//		this.customListTableData.data.Add(new CustomListTableData.CustomCellInfo(annotatedBeatmapLevelCollection.collectionName, null, annotatedBeatmapLevelCollection.coverImage));
			//	}
			//	this.customListTableData.tableView.ReloadData();
			//	this.customListTableData.tableView.ScrollToCellWithIdx(0, TableViewScroller.ScrollPositionType.Beginning, false);
			//}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B13 File Offset: 0x00000D13
		[UIAction("select-playlist")]
		internal void SelectPlaylist(TableView tableView, int idx)
		{
			//this.selectedPlaylist = (CustomBeatmapLevelCollectionSO)this.playlists[idx].beatmapLevelCollection;
			//Plugin.Log.Debug("selected playlist: " + this.selectedPlaylist.name);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B50 File Offset: 0x00000D50
		[UIAction("add-to-playlist")]
		internal void AddToPlaylist()
		{
			//LoadPlaylistScript.AddSongToPlaylist(this.selectedPlaylist.playlistPath, this.level.levelID);
			//Plugin.Log.Debug("Added playlist: " + this.selectedPlaylist.name);
			//this.modal.Hide(true, null);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002BA8 File Offset: 0x00000DA8
		internal void Setup()
		{
			this.standardLevel = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().First<StandardLevelDetailViewController>();
			GameObject gameObject = this.standardLevel.transform.Find("LevelDetail").gameObject;
			PersistentSingleton<BSMLParser>.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "OffsetPerMap.Views.View1.bsml"), gameObject.transform.Find("ActionButtons").gameObject, this);
			this.addButtonTransform.localScale *= 0.7f;
		}

		// Token: 0x0400000D RID: 13
		private StandardLevelDetailViewController standardLevel;

		// Token: 0x0400000E RID: 14
		private IAnnotatedBeatmapLevelCollection[] playlists;

		// Token: 0x0400000F RID: 15
		//private CustomBeatmapLevelCollectionSO selectedPlaylist;

		// Token: 0x04000010 RID: 16
		public CustomPreviewBeatmapLevel level;

		// Token: 0x04000011 RID: 17
		[UIComponent("list")]
		public CustomListTableData customListTableData;

		// Token: 0x04000012 RID: 18
		[UIComponent("add-button")]
		private Transform addButtonTransform;

		// Token: 0x04000013 RID: 19
		[UIComponent("modal")]
		private ModalView modal;
	}
}