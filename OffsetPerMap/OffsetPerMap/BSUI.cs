using System;
using System.Linq;
using BS_Utils.Utilities;
using HMUI;
using UnityEngine;

namespace OffsetPerMap
{
	internal static class BSUI
	{
		public static ResultsViewController ResultsViewController { get; private set; }

		public static LevelStatsView LevelStatsView { get; private set; }

		public static StandardLevelDetailViewController LevelDetailViewController { get; private set; }

		public static LevelCollectionTableView LevelCollectionTableView { get; private set; }

		public static void Initialize()
		{

			SoloFreePlayFlowCoordinator obj = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().First<SoloFreePlayFlowCoordinator>();
			BSUI.ResultsViewController = obj.GetPrivateField<ResultsViewController>("_resultsViewController");
			BSUI.LevelStatsView = obj.GetPrivateField<PlatformLeaderboardViewController>("_platformLeaderboardViewController").GetPrivateField<LevelStatsView>("_levelStatsView");
			LevelCollectionNavigationController privateField = obj.GetPrivateField<LevelSelectionNavigationController>("levelSelectionNavigationController").GetPrivateField<LevelCollectionNavigationController>("_levelCollectionNavigationController");
			BSUI.LevelDetailViewController = privateField.GetPrivateField<StandardLevelDetailViewController>("_levelDetailViewController");
			BSUI.LevelCollectionTableView = privateField.GetPrivateField<LevelCollectionViewController>("_levelCollectionViewController").GetPrivateField<LevelCollectionTableView>("_levelCollectionTableView");
		}
	}
}
