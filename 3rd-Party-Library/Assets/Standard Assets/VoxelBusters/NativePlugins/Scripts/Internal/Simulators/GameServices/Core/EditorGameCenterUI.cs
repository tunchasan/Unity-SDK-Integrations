#if USES_GAME_SERVICES && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorGameCenterUI : GUIModalWindow 
	{
		#region Constants

		private		const	float							kBannerPosAnimTime	= 1f;
		private		const	float							kBannerIdleTime		= 2f;

		#endregion

		#region Properties

		// Related to leaderboard
		private				bool							m_isShowingLeaderboardUI;
		private				EGCScore[]						m_leaderboardScoreList;

		// Related to achievement
		private				bool							m_isShowingAchievementUI;
		private				Dictionary<EGCAchievementDescription, EGCAchievement> m_achievementMap;
		private				bool							m_isShowingAchievementBanner;
		private				string							m_bannerText;
		
		// Related to callback
		private				Action							m_showingUIFinishedEvent		= null;

		#endregion

		#region Unity Methods

		protected override void Start ()
		{
			base.Start();

			// Instantiate resources
			UISkin			= Resources.Load(Constants.kSampleUISkin, typeof(GUISkin)) as GUISkin;
			this.enabled	= m_isShowingAchievementUI || m_isShowingLeaderboardUI || m_isShowingAchievementBanner;
		}

		#endregion

		#region GUI Methods

		protected override void OnGUIWindow ()
		{
			// Show banner
			if (m_isShowingAchievementBanner)
			{
				DrawAchievementBanner();
				return;
			}

			// Show gamecenter leaderboard UI
			if (m_isShowingLeaderboardUI)
			{
				DrawLeaderboardWindow();
				DrawWindowClose();
				return;
			}

			// Show gamecenter achievement UI
			if (m_isShowingAchievementUI)
			{
				DrawAchievementWindow();
				DrawWindowClose();
				return;
			}
		}

		protected override void AdjustFontBasedOnScreen ()
		{
			base.AdjustFontBasedOnScreen ();

			GUI.skin.GetStyle(Constants.kSubTitleStyle).fontSize	= Mathf.Clamp((int)(Screen.width * 0.03f), 0, 40);
		}

		protected override void AdjustWindowBasedOnScreen ()
		{
			if (m_isShowingAchievementUI || m_isShowingLeaderboardUI)
			{
				m_windowRect.xMin	= Screen.width * 0.2f;
				m_windowRect.yMin	= Screen.height * 0.15f;
				m_windowRect.width	= Screen.width * 0.6f;
				m_windowRect.height	= Screen.height * 0.7f;
			}
		}

		private void DrawLeaderboardWindow ()
		{
			GUILayout.Box("Leaderboard");

			// Show score list
			GUILayoutOption		_nameElementWidth	= GUILayout.Width(GetWindowWidth() * 0.5f);
			GUILayoutOption		_rankElementWidth	= GUILayout.Width(GetWindowWidth() * 0.15f);

			RootScrollView.BeginScrollView();
			{
				foreach (EGCScore _curScore in m_leaderboardScoreList)
				{
					EGCUser		_user				= _curScore.User;

					GUILayout.BeginHorizontal();
					{
						GUILayout.Box(_curScore.Rank.ToString(), _rankElementWidth);
						GUILayout.FlexibleSpace();
						GUILayout.Label(_user.Name, _nameElementWidth);
					}
					GUILayout.EndHorizontal();
				}
			}
			RootScrollView.EndScrollView();
		}

		private void DrawAchievementWindow ()
		{
			GUILayout.Box("Achievements");

			// Show achievements list
			GUILayoutOption		_titleElementWidth		= GUILayout.MaxWidth(GetWindowWidth() * 0.5f);
			GUILayoutOption		_percentageElementWidth	= GUILayout.MaxWidth(GetWindowWidth() * 0.15f);

			RootScrollView.BeginScrollView();
			{
				foreach (KeyValuePair<EGCAchievementDescription, EGCAchievement> _currentKeyValue in m_achievementMap)
				{
					float  	_percentageCompleted		= _currentKeyValue.Value == null ? 0f : _currentKeyValue.Value.PercentageCompleted;

					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(_currentKeyValue.Key.Title, _titleElementWidth);
						GUILayout.FlexibleSpace();
						GUILayout.Box(_percentageCompleted.ToString("0.0") + "%", _percentageElementWidth);
					}
					GUILayout.EndHorizontal();
				}
			}
			RootScrollView.EndScrollView();
		}

		private void DrawAchievementBanner ()
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label(m_bannerText, Constants.kSubTitleStyle);
			GUILayout.FlexibleSpace();
		}

		private void DrawWindowClose ()
		{
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Close"))
			{
				m_isShowingAchievementUI	= false;
				m_isShowingLeaderboardUI	= false;
				this.enabled				= false;

				if (m_showingUIFinishedEvent != null)
					m_showingUIFinishedEvent();
			}
		}

		#endregion

		#region Methods

		internal void ShowLeaderboardUI (EGCScore[] _scoreList, Action _onCompletion)
		{
			// Set properties
			m_isShowingLeaderboardUI	= true;
			m_leaderboardScoreList		= _scoreList;
			m_showingUIFinishedEvent	= _onCompletion;

			// Make component ready before showing UI
			OnShowingGameCenterUI();
		}

		internal void ShowAchievementUI (Dictionary<EGCAchievementDescription, EGCAchievement> _gcAchievementMap, Action _onCompletion)
		{
			// Set properties
			m_isShowingAchievementUI	= true;
			m_achievementMap			= _gcAchievementMap;
			m_showingUIFinishedEvent	= _onCompletion;
			
			// Make component ready before showing UI
			OnShowingGameCenterUI();
		}

		internal void ShowAchievementBanner (EGCAchievementDescription _description)
		{
			// Enable this script and set properties
			this.enabled					= true;
			m_isShowingAchievementBanner	= true;
			m_bannerText					= _description.Title + " is completed.";

			// Start coroutine
			StartCoroutine(AnimateWindowRect());
		}

		private IEnumerator AnimateWindowRect ()
		{
			float	_height			= 64f;
			float	_animTime		= kBannerPosAnimTime;
			float	_time			= 0f;

			// Set initial frame
			m_windowRect			= new Rect(0f, -_height, Screen.width, _height);

			// Focus animation
			while (_time < _animTime)
			{
				float _dt			= Time.deltaTime;
				m_windowRect.y		+= _height * _animTime * _dt;
				_time				+= _dt;

				yield return null;
			}

			// Idle
			yield return new WaitForSeconds(kBannerIdleTime);

			// Unfocus animation
			_time					= 0f;

			while (_time < _animTime)
			{
				float _dt			= Time.deltaTime;
				m_windowRect.y		-= _height * _animTime * _dt;
				_time				+= _dt;
				
				yield return null;
			}

			// Reset flag and disable component
			m_isShowingAchievementBanner	= false;
			this.enabled					= false;
		}

		private void OnShowingGameCenterUI ()
		{
			// Enable this component
			this.enabled					= true;

			// Reset properties
			m_isShowingAchievementBanner	= false;

			// Stop coroutine
			StopAllCoroutines();
		}

		#endregion
	}
}
#endif