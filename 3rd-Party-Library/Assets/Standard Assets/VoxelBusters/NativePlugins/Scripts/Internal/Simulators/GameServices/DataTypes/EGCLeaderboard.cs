using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms;

namespace VoxelBusters.NativePlugins.Internal
{
	[Serializable]
	public sealed class EGCLeaderboard
	{
		#region Fields
		
		[SerializeField]
		private				string					m_identifier;
		[SerializeField]
		private				string					m_title;
		[SerializeField]
		private				List<EGCScore>			m_scores;	
		[NonSerialized]
		private				EGCScore[]				m_lastQueryResults;	
		
		#endregion
		
		#region Properties
		
		public string Identifier
		{
			get
			{
				return m_identifier;
			}
			
			private set
			{
				m_identifier	= value;
			}
		}
		
		public string Title
		{
			get
			{
				return m_title;
			}
			
			private set
			{
				m_title		= value;
			}
		}
		
		public Range Range
		{
			get;
			set;
		}

		public EGCScore LocalUserScore
		{
			get
			{
				EGCUser _localUserInfo	= EditorGameCenter.Instance.GetLocalUserInfo();

				if (_localUserInfo == null)
					return null;

				return GetScoreWithUserID(_localUserInfo.Identifier);
			}
		}

		#endregion
		
		#region Constructor
		
		public EGCLeaderboard (string _identifier, string _title) 
		{
			// Initialize properties
			Identifier	= _identifier;
			Title		= _title;
			m_scores	= new List<EGCScore>();
			Range		= new Range(0, 0);
		}
		
		#endregion

		#region Methods
		
		public EGCScore GetScoreWithUserID (string _userID)
		{
			if (_userID == null)
				return null;

			return m_scores.FirstOrDefault(_score => _score.User.Identifier.Equals(_userID));
		}

		public void AddNewScoreInfo (EGCScore _newScore)
		{
			// Add this new score info
			string	_userID				= _newScore.User.Identifier;
			int 	_existingScoreEntry	= m_scores.FindIndex(_curScore => _curScore.User.Identifier.Equals(_userID));

			// Already records exist
			if (_existingScoreEntry != -1)
				m_scores.RemoveAt(_existingScoreEntry);
			
			// Add new entry and sort the list
			m_scores.Add(_newScore);
			m_scores.Sort(CompareScore);
			
			// Update Ranks
			for (int _iter = 0; _iter < m_scores.Count; _iter++)
				m_scores[_iter].Rank	= (_iter + 1);
		}

		public void LoadTopScores (eLeaderboardTimeScope _timeScope, eLeaderboardUserScope _userScope, int _maxResults, out string _error)
		{
			// Initial value
			_error	= null;

			// Set score fetch range
			Range 	_newRange			= new Range(1, _maxResults);
			
			// Load scores
			FilterScoreList(_timeScope, _userScope, _newRange);
		}

		public void LoadPlayerCenteredScores (eLeaderboardTimeScope _timeScope, eLeaderboardUserScope _userScope, int _maxResults, out string _error)
		{
			// Initial value
			_error	= null;

			// Compute range based on player rank
			EGCUser		_localUserInfo		= EditorGameCenter.Instance.GetLocalUserInfo();
			EGCScore	_localUserScore		= GetScoreWithUserID(_localUserInfo.Identifier);
			
			if (_localUserScore == null)
			{
				_error	= "The operation could not be completed because local user score info not found.";
				return;
			}
			
			int 	_localPlayerRank	= _localUserScore.Rank;
			int 	_loadFrom			= Mathf.Max(1, _localPlayerRank - Mathf.FloorToInt(_maxResults * 0.5f));
			Range 	_newRange			= new Range(_loadFrom, _maxResults);
			
			// Load scores
			FilterScoreList(_timeScope, _userScope, _newRange);
		}

		public void LoadMoreScores (eLeaderboardTimeScope _timeScope, eLeaderboardUserScope _userScope, int _maxResults, eLeaderboardPageDirection _pageDirection, out string _error)
		{
			// Initial value
			_error	= null;
			
			// Compute score fetch range
			Range	_curRange	= Range;
			
			if (_curRange.from == 0)
			{
				LoadTopScores(_timeScope, _userScope, _maxResults, out _error);
				return;
			}
			
			// Based on page direction, compute range of score to be loaded
			Range	_newRange	= new Range(0, _maxResults);
			
			if (_pageDirection == eLeaderboardPageDirection.PREVIOUS)
			{
				if (_curRange.from == 1)
				{
					_error 		= "The operation could not be completed because there are no more score records.";
					return;
				}
				
				_newRange.from	= Mathf.Max(1, _curRange.from - _maxResults);
			}
			else if (_pageDirection == eLeaderboardPageDirection.NEXT)
			{
				_newRange.from	= _curRange.from + _maxResults;
				
				if (_newRange.from > m_scores.Count)
				{
					_error 		= "The operation could not be completed because there are no more score records.";
					return;
				}
			}
			
			// Filter existing score list
			FilterScoreList(_timeScope, _userScope, _newRange);
		}

		public void FilterScoreList (eLeaderboardTimeScope _timeScope, eLeaderboardUserScope _userScope, Range _range)
		{	
			// Update range
			Range							= _range;

			// User scope based filtering
			List<EGCScore>	_usScoreList	= new List<EGCScore>();
			EGCUser			_localUserInfo	= EditorGameCenter.Instance.GetLocalUserInfo();

			if (_userScope == eLeaderboardUserScope.GLOBAL)
			{
				_usScoreList.AddRange(m_scores);
			}
			else
			{
				string[]	_friendIDList	= _localUserInfo.Friends;
				
				foreach (EGCScore _curScore in m_scores)
				{
					string	_curUserID		= _curScore.User.Identifier;
					
					if (_friendIDList.Any(_curFriendID => _curFriendID.Equals(_curUserID)))
						_usScoreList.Add(_curScore);
				}
			}
			
			// Time scope based filtering
			List<EGCScore>	_tsScoreList	= new List<EGCScore>();
			
			if (_timeScope == eLeaderboardTimeScope.ALL_TIME)
			{
				_tsScoreList.AddRange(_usScoreList);
			}
			else
			{
				TimeSpan 	_timespan;
				
				if (_timeScope == eLeaderboardTimeScope.TODAY)
					_timespan	= TimeSpan.FromDays(1);
				else
					_timespan	= TimeSpan.FromDays(7);
				
				long		_intervalStartTick	= DateTime.Now.Subtract(_timespan).Ticks;
				long		_intervalEndTick	= DateTime.Now.Ticks;
				
				foreach (EGCScore _curScore in _usScoreList)
				{
					long	_curScoreTicks		= _curScore.Date.Ticks;
					
					if (_curScoreTicks >= _intervalStartTick && _curScoreTicks <= _intervalEndTick)
						_tsScoreList.Add(_curScore);
				}
			}
			
			// Now get elements based on range
			List<EGCScore>	_finalScore			= new List<EGCScore>();
			int				_startIndex			= _range.from - 1;
			int				_endIndex			= _startIndex + _range.count;
			
			for (int _iter = _startIndex; (_iter < _tsScoreList.Count && _iter < _endIndex); _iter++)
				_finalScore.Add(_tsScoreList[_iter]);

			m_lastQueryResults	= _finalScore.ToArray();
		}

		public EGCScore[] GetLastQueryResults ()
		{
			return m_lastQueryResults;
		}

		private int CompareScore (EGCScore _score1, EGCScore _score2)
		{
			if (_score1.Value > _score2.Value)
				return -1;
			else if (_score2.Value > _score1.Value)
				return 0;
			else
				return _score1.User.Name.CompareTo(_score2.User.Name);
		}
		
		#endregion
	}
}
#endif