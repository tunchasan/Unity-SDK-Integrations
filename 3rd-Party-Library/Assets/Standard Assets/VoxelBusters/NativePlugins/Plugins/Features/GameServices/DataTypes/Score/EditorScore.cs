using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class EditorScore : Score
	{
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

		internal EditorScore ()
		{}

		internal EditorScore (string _leaderboardGlobalID, string _leaderboardID, User _user, long _scoreValue = 0L) 
			: base (_leaderboardGlobalID, _leaderboardID, _user, _scoreValue)
		{}

		internal EditorScore (EGCScore _scoreInfo)
		{
			string	_leaderboardID	= _scoreInfo.LeaderboardID;

			// Set properties
			LeaderboardGlobalID		= GameServicesUtils.GetLeaderboardGID(_leaderboardID);
			LeaderboardID			= _leaderboardID;
			User					= new EditorUser(_scoreInfo.User);
			Value					= _scoreInfo.Value;
			Date					= _scoreInfo.Date;
			Rank					= _scoreInfo.Rank;
		}

		#endregion

		#region Methods
		
		public override void ReportScore (ReportScoreCompletion _onCompletion)
		{
			base.ReportScore (_onCompletion);
			
			EditorGameCenter.Instance.ReportScore(this);
		}
		
		#endregion
		
		#region Event Callback Methods
		
		protected override void ReportScoreFinished (IDictionary _dataDict)
		{
			string		_error			= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			EGCScore	_gcScoreInfo	= _dataDict.GetIfAvailable<EGCScore>(EditorGameCenter.kScoreInfoKey);
			
			if (_gcScoreInfo != null)
			{
				// Update properties
				Value	= _gcScoreInfo.Value;
				Date	= _gcScoreInfo.Date;
				Rank	= _gcScoreInfo.Rank;
			}
			
			ReportScoreFinished(_error == null, _error);
		}
		
		#endregion
	}
}
#endif