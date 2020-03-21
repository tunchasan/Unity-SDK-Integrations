using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Represents an object that holds information for a score that was earned by the user.
	/// </summary>
	/// <remarks>
	/// \note Your game must authenticate the local user before using any features.
	/// </remarks>
	public abstract class Score : NPObject
	{
		#region Properties

		/// <summary>
		/// An unified string internally used to identify the leaderboard across all the supported platforms. (read-only)
		/// </summary>
		public string LeaderboardGlobalID
		{
			get;
			protected set;
		}
	
		/// <summary>
		/// A string used to identify the leaderboard in the current platform. (read-only)
		/// </summary>
		public abstract string LeaderboardID
		{
			get;
			protected set;
		}

		/// <summary>
		/// The user that earned the score. (read-only)
		/// </summary>
		public abstract User User
		{
			get;
			protected set;
		}

		/// <summary>
		/// The score earned by the user.
		/// </summary>
		public abstract long Value
		{
			get;
			set;
		}

		/// <summary>
		/// The date and time when score was earned. (read-only)
		/// </summary>
		public abstract DateTime Date
		{
			get;
			protected set;
		}

		/// <summary>
		/// The user’s score  as a localized string. (read-only)
		/// </summary>
		public virtual string FormattedValue
		{
			get
			{
				return Value.ToString("0,0", CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		/// The position of the score in leaderboard. (read-only) 
		/// </summary>
		public abstract int Rank
		{
			get;
			protected set;
		}

		#endregion

		#region Delegates

		/// <summary>
		/// Delegate that will be called when score is reported.
		/// </summary>
		/// <param name="_success">A bool value used to indicate operation status.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void ReportScoreCompletion (bool _success, string _error);

		#endregion

		#region Events

		protected event ReportScoreCompletion ReportScoreFinishedEvent;

		#endregion

		#region Constructor
		
		protected Score () : base (NPObjectManager.eCollectionType.GAME_SERVICES)
		{}

		protected Score (string _leaderboardGlobalID, string _leaderboardID, User _user, long _scoreValue) : base (NPObjectManager.eCollectionType.GAME_SERVICES)
		{
			// Initialize properties
			LeaderboardGlobalID	= _leaderboardGlobalID;
			LeaderboardID		= _leaderboardID;
			User				= _user;
			Value				= _scoreValue;
			Date				= DateTime.Now;
			Rank				= 0;
		}	
		
		#endregion
		
		#region Methods

		/// <summary>
		/// Reports the score to game server.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void ReportScore (ReportScoreCompletion _onCompletion)
		{
			// Cache event
			ReportScoreFinishedEvent = _onCompletion;
		}
		
		public override string ToString ()
		{
			return string.Format("[Score: Rank={0}, UserName={1}, Value={2}]", 
			                     Rank, User.Name, Value);
		}

		#endregion

		#region Event Callback Methods

		protected virtual void ReportScoreFinished (IDictionary _dataDict)
		{}
		
		protected void ReportScoreFinished (bool _success, string _error)
		{
			if (ReportScoreFinishedEvent != null)
				ReportScoreFinishedEvent(_success, _error);

#if USES_SOOMLA_GROW
			NPBinding.SoomlaGrowService.ReportOnLatestScore(User.Identifier, Value);
#endif
		}

		#endregion
	}
}