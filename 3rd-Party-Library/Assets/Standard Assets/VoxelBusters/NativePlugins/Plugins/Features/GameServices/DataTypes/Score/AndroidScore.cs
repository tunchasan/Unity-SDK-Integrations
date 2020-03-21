using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_ANDROID
using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	internal sealed class AndroidScore : Score 
	{
		#region Constants
		private const string 	kScoreInfoKey		= "score-info";
		private const string	kIdentifier			= "identifier";
		private const string	kUser				= "user";
		private const string	kValue				= "value";
		private const string	kDate				= "date";
		private const string	kFormattedValue		= "formatted-value";
		private const string	kRank				= "rank";
		
		#endregion
		
		#region Properties

		public override string LeaderboardID
		{
			get;
			protected set;
		}
		

		public override User User
		{
			get;
			protected set;
		}
		
		public override long Value
		{
			get;
			set;
		}
		
		public override DateTime Date
		{
			get;
			protected set;
		}
		
		public override int Rank
		{
			get;
			protected set;
		}

		#endregion

		#region Constructors

		private AndroidScore ()
		{}

		public AndroidScore (string _leaderboardGlobalID, string _leaderboardID, User _user, long _scoreValue = 0L) 
			: base (_leaderboardGlobalID, _leaderboardID, _user, _scoreValue)
		{}

		internal AndroidScore (IDictionary _scoreData)
		{
			LeaderboardID			= _scoreData.GetIfAvailable<string>(kIdentifier);	
			LeaderboardGlobalID		= GameServicesUtils.GetLeaderboardGID(LeaderboardID);
			
			User					= new AndroidUser(_scoreData.GetIfAvailable<Dictionary<string, object>>(kUser));
			Value					= _scoreData.GetIfAvailable<long>(kValue);

			long _timeInMillis		= _scoreData.GetIfAvailable<long>(kDate);
			Date 					= _timeInMillis.ToDateTimeFromJavaTime();

			Rank					= _scoreData.GetIfAvailable<int>(kRank);
		}

		#endregion

		#region Static Methods

		internal static AndroidScore ConvertScore (IDictionary _score)
		{
			if (_score == null)
				return null;

			return new AndroidScore(_score);
		}

		internal static AndroidScore[] ConvertScoreList (IList _scoreList)
		{
			if (_scoreList == null)
				return null;
			
			int					_count				= _scoreList.Count;
			AndroidScore[]		_androidScoreList	= new AndroidScore[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_androidScoreList[_iter]			= new AndroidScore(_scoreList[_iter] as IDictionary);
			
			return _androidScoreList;
		}

		#endregion

		#region Methods

		public override void ReportScore (Score.ReportScoreCompletion _onCompletion)
		{
			base.ReportScore (_onCompletion);

			GameServicesAndroid.Plugin.Call(GameServicesAndroid.Native.Methods.REPORT_SCORE, GetInstanceID(), LeaderboardID, Value, _onCompletion != null);
		}

		#endregion

		#region Event Callback Methods
		
		protected override void ReportScoreFinished (IDictionary _dataDict)
		{
			string		_error		= _dataDict.GetIfAvailable<string>(GameServicesAndroid.kNativeMessageError);
			IDictionary	_infoDict	= _dataDict.GetIfAvailable<IDictionary>(kScoreInfoKey);
			
			if (_infoDict != null)
			{
				// Update properties
				Value					= _infoDict.GetIfAvailable<long>(kValue);

				long _timeInMillis		= _infoDict.GetIfAvailable<long>(kDate);
				Date 					= _timeInMillis.ToDateTimeFromJavaTime();

			}
			
			ReportScoreFinished(string.IsNullOrEmpty(_error), _error);
		}
		
		#endregion
	}
}
#endif