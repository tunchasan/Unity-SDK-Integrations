using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSScore : Score 
	{
		#region Constants
		
		private 	const 	string 		kScoreInfoKey				= "score-info";
		private 	const 	string 		kDateKey					= "date";
		private 	const 	string 		kFormattedValueKey			= "formatted-value";
		private 	const 	string 		kLeaderboardIdentifierKey	= "leaderboard-id";
		private 	const 	string 		kPlayerKey					= "player";
		private		const 	string		kRankKey					= "rank";
		private		const 	string		kValueKey					= "value";
		
		#endregion

		#region Properties

		public override	string LeaderboardID
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

		private iOSScore ()
		{}
		
		public iOSScore (string _leaderboardGlobalID, string _leaderboardID, User _user, long _scoreValue = 0L) 
			: base (_leaderboardGlobalID, _leaderboardID, _user, _scoreValue)
		{}

		public iOSScore (IDictionary _dataDict) : base ()
		{
			// Parse data dictionary values
			string		_leaderboardID	= _dataDict.GetIfAvailable<string>(kLeaderboardIdentifierKey);
			IDictionary _userDataDict	= _dataDict.GetIfAvailable<IDictionary>(kPlayerKey);

			LeaderboardGlobalID		= GameServicesUtils.GetLeaderboardGID(_leaderboardID);
			LeaderboardID			= _leaderboardID;
			Value					= _dataDict.GetIfAvailable<long>(kValueKey);
			Date					= _dataDict.GetIfAvailable<string>(kDateKey).ToZuluFormatDateTimeLocal();
			Rank					= _dataDict.GetIfAvailable<int>(kRankKey);

			if (_userDataDict != null)
				User				= new iOSUser(_userDataDict);
		}

		#endregion

		#region External Methods
		
		[DllImport("__Internal")]
		private static extern void reportScore (string _scoreInfoJSON, long _value);
		
		#endregion

		#region Methods

		public override void ReportScore (ReportScoreCompletion _onCompletion)
		{
			base.ReportScore (_onCompletion);

			// Native method call
			reportScore(GetScoreInfoJSONObject().ToJSON(), Value);
		}

		public IDictionary GetScoreInfoJSONObject ()
		{
			IDictionary		_JSONDict	= new Dictionary<string, object>();
			_JSONDict[kLeaderboardIdentifierKey]			= LeaderboardID;
			_JSONDict[GameServicesIOS.kObjectInstanceIDKey]	= GetInstanceID();

			return _JSONDict;
		}

		#endregion
		
		#region Event Callback Methods
		
		protected override void ReportScoreFinished (IDictionary _dataDict)
		{
			string		_error		= _dataDict.GetIfAvailable<string>(GameServicesIOS.kNativeMessageErrorKey);
			IDictionary	_infoDict	= _dataDict.GetIfAvailable<IDictionary>(kScoreInfoKey);

			if (_infoDict != null)
			{
				// Update properties
				Value	= _infoDict.GetIfAvailable<long>(kValueKey);
				Date	= _infoDict.GetIfAvailable<string>(kDateKey).ToZuluFormatDateTimeLocal();
				Rank	= _infoDict.GetIfAvailable<int>(kRankKey);
			}

			ReportScoreFinished(_error == null, _error);
		}
		
		#endregion
	}
}
#endif