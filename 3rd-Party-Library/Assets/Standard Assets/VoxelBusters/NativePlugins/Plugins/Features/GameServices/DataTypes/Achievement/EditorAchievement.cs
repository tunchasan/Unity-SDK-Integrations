using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class EditorAchievement : Achievement
	{
		#region Properties

		public override string Identifier
		{
			get;
			protected set;
		}
		
		public override bool Completed
		{
			get;
			protected set;
		}
		
		public override DateTime LastReportedDate
		{
			get;
			protected set;
		}

		#endregion

		#region Constructors

		private EditorAchievement ()
		{}

		public EditorAchievement (string _globalIdentifier, string _identifier, double _percentageCompleted = 0) 
			: base (_globalIdentifier, _identifier, _percentageCompleted)
		{}

		public EditorAchievement (EGCAchievement _gcAchievementInfo)
		{
			// Set properties from info object
			Identifier				= _gcAchievementInfo.Identifier;
			PercentageCompleted		= _gcAchievementInfo.PercentageCompleted;
			Completed				= _gcAchievementInfo.Completed;
			LastReportedDate		= _gcAchievementInfo.LastReportedDate;

			// Set global identifier			
			GlobalIdentifier		= GameServicesUtils.GetAchievementGID(Identifier);
		}
		
		#endregion

		#region Static Methods
		
		public static EditorAchievement[] ConvertAchievementsList (IList _gcAchievements)
		{
			if (_gcAchievements == null)
				return null;
			
			int 				_count				= _gcAchievements.Count;
			EditorAchievement[]	_achievementsList	= new EditorAchievement[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_achievementsList[_iter]			= new EditorAchievement((EGCAchievement)_gcAchievements[_iter]);
			
			return _achievementsList;
		}
		
		#endregion

		#region Methods

		public override void ReportProgress (ReportProgressCompletion _onCompletion)
		{
			base.ReportProgress(_onCompletion);

			// Make call for reporting
			EditorGameCenter.Instance.ReportProgress(this);
		}

		#endregion

		#region Event Callback Methods
		
		protected override void ReportProgressFinished (IDictionary _dataDict)
		{
			// Update properties
			string			_error				= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			EGCAchievement 	_gcAchievementInfo	= _dataDict.GetIfAvailable<EGCAchievement>(EditorGameCenter.kAchievementInfoKey);
			
			if (_gcAchievementInfo != null)
			{
				PercentageCompleted		= _gcAchievementInfo.PercentageCompleted;
				Completed				= _gcAchievementInfo.Completed;
				LastReportedDate		= _gcAchievementInfo.LastReportedDate;
			}

			// Invoke handler
			ReportProgressFinished(_error == null, _error);
		}
		
		#endregion
	}
}
#endif