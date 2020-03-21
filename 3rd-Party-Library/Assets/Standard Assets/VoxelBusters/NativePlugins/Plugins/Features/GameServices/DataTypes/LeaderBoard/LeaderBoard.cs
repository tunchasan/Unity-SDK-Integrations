using UnityEngine;
using System.Collections;
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{	
	using Internal;

	/// <summary>
	/// Represents an object used to read data from a leaderboard stored on game server.
	/// </summary>
	/// <remarks>
	/// \note Your game must authenticate the local user before using any features.
	/// </remarks>
	public abstract class Leaderboard : NPObject
	{
		#region Constants

		protected	const	int 		kLoadScoresMinResults		= 1;
#if UNITY_ANDROID
		protected	const	int 		kLoadScoresMaxResults		= 25; //On android  max of 25 results can be loaded
#else	
		protected	const	int 		kLoadScoresMaxResults		= 100;
#endif

		#endregion

		#region Fields

		private				int			m_maxResults;

		#endregion

		#region Properties
		
		/// <summary>
		/// An unified string internally used to identify the leaderboard across all the supported platforms. (read-only)
		/// </summary>
		public string GlobalIdentifier
		{
			get;
			protected set;
		}

		/// <summary>
		/// A string used to identify the leaderboard in the current platform. (read-only)
		/// </summary>
		public abstract string Identifier
		{
			get;
			protected set;
		}

		/// <summary>
		/// A localized title for the leaderboard. (read-only)
		/// </summary>
		public abstract string Title
		{
			get;
			protected set;
		}

		/// <summary>
		/// A filter used to restrict the search to a subset of the users on game server.
		/// </summary>
		public abstract eLeaderboardUserScope UserScope
		{
			get;
			set;
		}

		/// <summary>
		/// A filter used to restrict the search to scores that were posted within a specific period of time.
		/// </summary>
		public abstract eLeaderboardTimeScope TimeScope
		{
			get;
			set;
		}

		/// <summary>
		/// The value indicates maximum entries that has to be fetched from search.
		/// </summary>
		public int MaxResults
		{
			get
			{
				return m_maxResults;
			}

			set
			{
				m_maxResults	= Mathf.Clamp(value, kLoadScoresMinResults, kLoadScoresMaxResults);
			}
		}

		/// <summary>
		/// An array of <see cref="Score"/> objects that contain scores returned by search. (read-only)
		/// </summary>
		/// <remarks>
		/// \note This property is invalid until a call to load scores is completed.
		/// </remarks>
		public abstract Score[] Scores
		{
			get;
			protected set;
		}

		/// <summary>
		/// The <see cref="Score"/> earned by the local user. (read-only)
		/// </summary>
		/// <remarks>
		/// \note This property is invalid until a call to load scores is completed.
		/// </remarks>
		public abstract Score LocalUserScore
		{
			get;
			protected set;
		}

		#endregion	

		#region Delegates

		/// <summary>
		/// Delegate that will be called when requested score set is retrieved from game server.
		/// </summary>
		/// <param name="_scores">An array of <see cref="Score"/> objects that holds the requested scores.</param>
		/// <param name="_localUserScore">The score earned by the local user.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void LoadScoreCompletion (Score[] _scores, Score _localUserScore, string _error);

		#endregion
		
		#region Events

		private event LoadScoreCompletion LoadScoreFinishedEvent;
		
		#endregion

		#region Constructors

		protected Leaderboard () : base (NPObjectManager.eCollectionType.GAME_SERVICES)
		{}

		protected Leaderboard (string _globalIdentifer, string _identifier, string _title = null, eLeaderboardUserScope _userScope = eLeaderboardUserScope.GLOBAL, eLeaderboardTimeScope _timeScope = eLeaderboardTimeScope.ALL_TIME, int _maxResults = kLoadScoresMaxResults, Score[] _scores = null, Score _localUserScore = null)
			: base (NPObjectManager.eCollectionType.GAME_SERVICES)
		{
			// Initialize properties
			GlobalIdentifier	= _globalIdentifer;
			Identifier			= _identifier;
			Title				= _title;
			UserScope			= _userScope;
			TimeScope			= _timeScope;
			MaxResults			= _maxResults;
			Scores				= _scores;
			LocalUserScore		= _localUserScore;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Asynchronously loads the top set of scores.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void LoadTopScores (LoadScoreCompletion _onCompletion)
		{
			// Cache callback information
			LoadScoreFinishedEvent	= _onCompletion;
		}

		/// <summary>
		/// Asynchronously loads the player-centered set of scores.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void LoadPlayerCenteredScores (LoadScoreCompletion _onCompletion)
		{
			// Cache callback information
			LoadScoreFinishedEvent	= _onCompletion;
		}

		/// <summary>
		/// Asynchronously loads an additional set of scores.
		/// </summary>
		/// <param name="_pageDirection">The direction of pagination over leaderboard score sets.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void LoadMoreScores (eLeaderboardPageDirection _pageDirection, LoadScoreCompletion _onCompletion)
		{
			// Cache callback information
			LoadScoreFinishedEvent	= _onCompletion;
		}

		protected void CacheLoadScoreCompletionCallback (LoadScoreCompletion _onCompletion)
		{
			LoadScoreFinishedEvent	= _onCompletion;
		}
		
		public override string ToString ()
		{
			return string.Format("[Leaderboard: Identifier={0}, UserScope={1}, TimeScope={2}]", Identifier, UserScope, TimeScope);
		}

		#endregion

		#region Event Callback Methods

		protected virtual void LoadScoresFinished (IDictionary _dataDict)
		{}

		protected void LoadScoresFinished (Score[] _scores, Score _localUserScore, string _error)
		{
			// Set properties
			Scores			= _scores;
			LocalUserScore	= _localUserScore;

			// Send event
			if (LoadScoreFinishedEvent != null)
				LoadScoreFinishedEvent(_scores, _localUserScore, _error);
		}

		#endregion
	}
}