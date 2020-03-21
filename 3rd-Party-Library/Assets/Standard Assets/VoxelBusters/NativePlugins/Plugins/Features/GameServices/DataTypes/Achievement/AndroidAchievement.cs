using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_ANDROID
using System;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	internal sealed class AndroidAchievement : Achievement 
	{
		#region Constants
		private	const 		string				kAchievementInfoKey		= "achievement-info";
		
		private const 		string				kIdentifier				= "identifier";
		private const 		string				kCurrentSteps			= "points-scored";
		private const 		string				kMaxSteps				= "maximum-points";
		private const 		string				kCompleted				= "is-completed";
		private const 		string				kLastReportDate			= "last-report-date";
		private const 		string				kDescription			= "description";
		
		#endregion

		#region Fields
		
		private 			int					m_pointsScored;

		#endregion

		#region Properties

		public override	string Identifier
		{
			get;
			protected set;
		}

		public override bool Completed
		{
			get;
			protected set;
		}
		
		public override	DateTime LastReportedDate
		{
			get;
			protected set;
		}

		public override double PercentageCompleted
		{
			get
			{
				return m_percentageCompleted;
			}
			
			set
			{
				m_percentageCompleted	= System.Math.Min(100d, value);
				m_pointsScored			= Mathf.FloorToInt((float)(m_percentageCompleted * MaximumPoints/100d));
				m_pointsScored			= Mathf.Min(m_pointsScored, MaximumPoints);
			}
		}

		private int MaximumPoints
		{
			get
			{
				return NPBinding.GameServices.GetNoOfStepsForCompletingAchievement(GlobalIdentifier);
			}
		}

		#endregion
		
		#region Constructors

		private AndroidAchievement ()
		{}

		public AndroidAchievement (string _globalIdentifier, string _identifier, double _percentageCompleted = 0d) 
			: base (_globalIdentifier, _identifier, _percentageCompleted)
		{}
		
		internal AndroidAchievement (IDictionary _achievementData)
		{
			SetDetails(_achievementData);
		}
		
		internal void SetDetails(IDictionary _achievementData)
		{
			Identifier				= _achievementData.GetIfAvailable<string>(kIdentifier);	
			Completed				= _achievementData.GetIfAvailable<bool>(kCompleted);
			int _stepsFinished		= _achievementData.GetIfAvailable<int>(kCurrentSteps);
			int _maxSteps			= _achievementData.GetIfAvailable<int>(kMaxSteps);
			
			PercentageCompleted		= ((double)_stepsFinished / _maxSteps) * 100d;

			long _timeInMillis		= _achievementData.GetIfAvailable<long>(kLastReportDate);
			LastReportedDate 		= _timeInMillis.ToDateTimeFromJavaTime();
			
			GlobalIdentifier		= GameServicesUtils.GetAchievementGID(Identifier);
		}

		#endregion

		#region Static Methods

		internal static AndroidAchievement[] ConvertAchievementList (IList _achievementList)
		{
			if (_achievementList == null)
				return null;

			int 					_count					= _achievementList.Count;
			AndroidAchievement[]	_androidAchievementList	= new AndroidAchievement[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
			{
				_androidAchievementList[_iter]				= new AndroidAchievement(_achievementList[_iter] as IDictionary);
			}

			return _androidAchievementList;
		}

		#endregion

		#region Methods

		public override void ReportProgress (Achievement.ReportProgressCompletion _onCompletion)
		{
			base.ReportProgress(_onCompletion);
	
			int _noOfStepsFromDescription = GetMaximumPointsFromAchievementDescription();
			
			if (MaximumPoints != _noOfStepsFromDescription)
   			{
				Debug.LogError("[GameServices] Please make sure number of steps set in NPSettings and Maximum points for incremental achivement configured in Game Play services are same.");
			}
			
			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.REPORT_PROGRESS, GetInstanceID(), Identifier , m_pointsScored, _onCompletion != null);	
		}

		#endregion


		#region Event Callback Methods
		
		protected override void ReportProgressFinished (IDictionary _dataDict)
		{
			string		_error		= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			IDictionary _infoDict	= _dataDict.GetIfAvailable<IDictionary>(kAchievementInfoKey);
			
			if (_infoDict != null)
			{
				// Update properties
				SetDetails(_infoDict);
			}
			
			ReportProgressFinished(_error == null, _error);
		}
		
		#endregion


		#region Private Methods

		private int GetMaximumPointsFromAchievementDescription()
		{
			AchievementDescription _description = AchievementHandler.GetAchievementDescriptionWithID(Identifier);
			
			if (_description == null)
			{
				return 0;
			}
			else
			{
#pragma warning disable				
				return _description.MaximumPoints;
#pragma warning restore				
			}	
		}

		#endregion
	}
}
#endif