using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSAchievement : Achievement 
	{
		#region Constants

		private		const 	string		kAchievementInfoKey		= "achievement-info";
		private 	const 	string 		kIdentifierKey			= "id";
		private 	const 	string 		kPercentCompleteKey		= "percent-complete";
		private 	const 	string 		kCompletedKey			= "completed";
		private 	const 	string 		kLastReportedDateKey	= "last-reported-date";

		#endregion

		#region Properties

		public override	string Identifier
		{
			get;
			protected set;
		}
		
		public override	bool Completed
		{
			get;
			protected set;
		}
		
		public override	DateTime LastReportedDate
		{
			get;
			protected set;
		}
		
		#endregion
		
		#region Constructors

		private iOSAchievement ()
		{}

		public iOSAchievement (string _globalIdentifier, string _identifier, double _percentageCompleted = 0d) 
			: base (_globalIdentifier, _identifier, _percentageCompleted)
		{}
		                     
		public iOSAchievement (IDictionary _dataDict)
		{
			// Parse data dictionary values
			Identifier			= _dataDict.GetIfAvailable<string>(kIdentifierKey);
			Completed			= _dataDict.GetIfAvailable<bool>(kCompletedKey);
			LastReportedDate	= _dataDict.GetIfAvailable<string>(kLastReportedDateKey).ToZuluFormatDateTimeLocal();
			PercentageCompleted	= _dataDict.GetIfAvailable<double>(kPercentCompleteKey);

			// Set global identifier			
			GlobalIdentifier	= GameServicesUtils.GetAchievementGID(Identifier);
		}
		
		#endregion
		
		#region External Methods
		
		[DllImport("__Internal")]
		private static extern void reportProgress (string _achievementInfoJSON, double _percentComplete);
		
		#endregion

		#region Static Methods

		public static iOSAchievement[] ConvertAchievementsList (IList _achievementsJSONList)
		{
			if (_achievementsJSONList == null)
				return null;
			
			int 				_count				= _achievementsJSONList.Count;
			iOSAchievement[]	_achievementsList	= new iOSAchievement[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_achievementsList[_iter]			= new iOSAchievement((IDictionary)_achievementsJSONList[_iter]);

			return _achievementsList;
		}

		#endregion

		#region Methods

		public override void ReportProgress (ReportProgressCompletion _onCompletion)
		{
			base.ReportProgress (_onCompletion);

			// Native method call 
			reportProgress(GetAchievementInfoJSONObject().ToJSON(), PercentageCompleted);
		}

		public IDictionary GetAchievementInfoJSONObject ()
		{
			IDictionary		_JSONDict		= new Dictionary<string, object>();
			_JSONDict[kIdentifierKey]		= Identifier;
			_JSONDict[GameServicesIOS.kObjectInstanceIDKey]	= GetInstanceID();

			return _JSONDict;
		}

		#endregion

		#region Event Callback Methods

		protected override void ReportProgressFinished (IDictionary _dataDict)
		{
			string		_error		= _dataDict.GetIfAvailable<string>(GameServicesIOS.kNativeMessageErrorKey);
			IDictionary _infoDict	= _dataDict.GetIfAvailable<IDictionary>(kAchievementInfoKey);

			if (_infoDict != null)
			{
				// Update properties
				Completed			= _infoDict.GetIfAvailable<bool>(kCompletedKey);
				LastReportedDate	= _infoDict.GetIfAvailable<string>(kLastReportedDateKey).ToZuluFormatDateTimeLocal();
				PercentageCompleted	= _infoDict.GetIfAvailable<double>(kPercentCompleteKey);
			}

			ReportProgressFinished(_error == null, _error);
		}

		#endregion
	}
}
#endif