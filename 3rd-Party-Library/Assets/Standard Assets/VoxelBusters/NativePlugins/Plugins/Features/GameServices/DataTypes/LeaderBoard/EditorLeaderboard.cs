using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class EditorLeaderboard : Leaderboard
	{
		#region Properties

		public override string Identifier
		{
			get;
			protected set;
		}
		
		public override string Title
		{
			get;
			protected set;
		}
		
		public override eLeaderboardUserScope UserScope
		{
			get;
			set;
		}
		
		public override eLeaderboardTimeScope TimeScope
		{
			get;
			set;
		}
		
		public override Score[] Scores
		{
			get;
			protected set;
		}

		public override Score LocalUserScore
		{
			get;
			protected set;
		}
		
		#endregion

		#region Constructors

		private EditorLeaderboard ()
		{}

		public EditorLeaderboard (string _globalIdentifier, string _identifier) : base (_globalIdentifier, _identifier)
		{}

		public EditorLeaderboard (EGCLeaderboard _gcLeaderboardInfo)
		{
			// Set properties from info object
			Identifier			= _gcLeaderboardInfo.Identifier;
			Title 				= _gcLeaderboardInfo.Title;
			Scores				= null;
			LocalUserScore		= null;

			// Set global identifier
			GlobalIdentifier	= GameServicesUtils.GetLeaderboardGID(Identifier);
		}

		#endregion

		#region Methods

		public override	void LoadTopScores (LoadScoreCompletion _onCompletion)
		{
			base.LoadTopScores(_onCompletion);

			// Load scores
			EditorGameCenter.Instance.LoadTopScores(this);
		}
		
		public override	void LoadPlayerCenteredScores (LoadScoreCompletion _onCompletion)
		{
			base.LoadPlayerCenteredScores(_onCompletion);

			// Load scores
			EditorGameCenter.Instance.LoadPlayerCenteredScores(this);
		}
		
		public override	void LoadMoreScores (eLeaderboardPageDirection _pageDirection, LoadScoreCompletion _onCompletion)
		{
			base.LoadMoreScores(_pageDirection, _onCompletion);

			// Load scores
			EditorGameCenter.Instance.LoadMoreScores(this, _pageDirection);
		}

		#endregion

		#region Event Callback Methods
		
		protected override void LoadScoresFinished (IDictionary _dataDict)
		{
			EditorScore[]	_scores				= null;
			EditorScore		_localUserScore		= null;

			// Parse received information
			string			_error				= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			EGCLeaderboard 	_leaderboardInfo	= _dataDict.GetIfAvailable<EGCLeaderboard>(EditorGameCenter.kLeaderboardInfoKey);
			
			if (_leaderboardInfo != null)
			{
				string 		_title				= _leaderboardInfo.Title;
				EGCScore[] 	_egcScores			= _leaderboardInfo.GetLastQueryResults();
				EGCScore 	_egcLocalUserScore	= _leaderboardInfo.LocalUserScore;

				if (_egcScores != null)
				{
					int		_count		= _egcScores.Length;
					_scores				= new EditorScore[_count];
					
					for (int _iter = 0; _iter < _count; _iter++)
						_scores[_iter]	= new EditorScore(_egcScores[_iter]);
				}
				
				if (_egcLocalUserScore != null)
					_localUserScore		= new EditorScore(_egcLocalUserScore);

				// Update leaderboard properties
				this.Title	= _title;
			}

			// Invoke finish handler
			LoadScoresFinished(_scores, _localUserScore, _error);
		}
		
		#endregion
	}
}
#endif