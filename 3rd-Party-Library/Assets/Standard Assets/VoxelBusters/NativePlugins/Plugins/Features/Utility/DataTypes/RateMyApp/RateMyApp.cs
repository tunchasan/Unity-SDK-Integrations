using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// For getting user reviews, this class provides unique way to prompt user based on configured settings.
	/// </summary>
	public class RateMyApp 
	{
		#region Fields
	
		private 	RateMyAppSettings			m_settings;
		private		IRateMyAppViewController	m_viewController;
		private		IRateMyAppKeysCollection	m_keysCollection;
		private		IRateMyAppEventResponder	m_eventResponder;
		private		IRateMyAppOperationHandler	m_operationHandler;

		#endregion

		#region Properties

		public IRateMyAppDelegate Delegate
		{
			private get;
			set;
		}

		#endregion

		#region Create Methods

		public static RateMyApp Create(IRateMyAppViewController _viewController, IRateMyAppKeysCollection _keysCollection, 
		                               IRateMyAppEventResponder _eventResponder, IRateMyAppOperationHandler _operationHandler, 
		                               RateMyAppSettings _settings)
		{
			RateMyApp _newObject	= new RateMyApp() 
			{
				m_viewController	= _viewController,
				m_keysCollection	= _keysCollection,
				m_eventResponder	= _eventResponder,
				m_operationHandler	= _operationHandler,
				m_settings 			= _settings,
			};
			_newObject.MarkIfLaunchIsFirstTime();

			return _newObject;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Checks if review prompt needs to be shown as per the settings done. This needs to be constantly called to check if conditions are met for showing a review prompt.
		/// </summary>
		public void AskForReview ()
		{
			if (!CanAskForReview())
				return;

			m_operationHandler.Execute(ShowDialogRoutine());
		}

		/// <summary>
		/// Show review prompt now irrespective of settings.
		/// </summary>
		public void AskForReviewNow ()
		{
			ShowDialog();
		}

		public void RecordAppLaunch ()
		{
			int _appUsageCount	= PlayerPrefs.GetInt(m_keysCollection.AppUsageCountKeyName, 0) + 1;

			// Write to disk 
			PlayerPrefs.SetInt(m_keysCollection.AppUsageCountKeyName, _appUsageCount);
			PlayerPrefs.Save();
		}

		#endregion

		#region Private Methods

		private void MarkIfLaunchIsFirstTime ()
		{
			bool	_keyFound			= (PlayerPrefs.GetInt(m_keysCollection.ShowPromptAfterKeyName, -1) != -1);
			bool	_isFirstTimeLaunch	= (!_keyFound) ? true : IsFirstTimeLaunch();

			PlayerPrefs.SetInt(key: m_keysCollection.IsFirstTimeLaunchKeyName, 
			                   value: _isFirstTimeLaunch ? 1 : 0);	
		}

		private int GetAppUsageCount ()
		{
			return PlayerPrefs.GetInt(m_keysCollection.AppUsageCountKeyName, 0);
		}

		private bool IsFirstTimeLaunch ()
		{
			return PlayerPrefs.GetInt(m_keysCollection.IsFirstTimeLaunchKeyName, 0) == 1;
		}

		private bool CanAskForReview ()
		{
			try
			{
				// Check if user has asked not to show rate
				if (PlayerPrefs.GetInt(m_keysCollection.DontShowKeyName, 0) == 1)
					return false;
				
				// Check if this version or previous version was already rated
				string		_lastVersionReviewed	= PlayerPrefs.GetString(m_keysCollection.VersionLastRatedKeyName);
				if (!string.IsNullOrEmpty(_lastVersionReviewed))
				{
					string	_currentVersion			= VoxelBusters.Utility.PlayerSettings.GetBundleVersion();
					if (string.Compare(_currentVersion, _lastVersionReviewed, StringComparison.InvariantCulture) <= 0)
						return false;
				}

				// Find out whether app was just installed and is used for first time
				// If so, set hours after which rate me is prompted for first time
				DateTime 	_utcNow					= DateTime.UtcNow;
				int 		_showPromptAfterHours	= PlayerPrefs.GetInt(m_keysCollection.ShowPromptAfterKeyName, -1);

				if (_showPromptAfterHours == -1)
				{
					_showPromptAfterHours	= m_settings.ShowFirstPromptAfterHours;
						
					PlayerPrefs.SetInt(m_keysCollection.ShowPromptAfterKeyName, m_settings.ShowFirstPromptAfterHours);
					PlayerPrefs.SetString(m_keysCollection.PromptLastShownKeyName, _utcNow.ToString(System.Globalization.CultureInfo.InvariantCulture));
                } 

				// Check for rest of trigger conditions
				string 		_promptLastShownOnString	= PlayerPrefs.GetString(m_keysCollection.PromptLastShownKeyName);
                DateTime    _promptLastShown            = DateTime.Parse(_promptLastShownOnString, System.Globalization.CultureInfo.InvariantCulture);
                int         _hoursSincePromptLastShown  = (int)(_utcNow - _promptLastShown).TotalHours;
				int			_appUsageCount				= GetAppUsageCount();

				// Need to wait until time exceeds
				if (_showPromptAfterHours > _hoursSincePromptLastShown)
					return false;
				
				// Make sure usage count exceeds min count before showing prompt
				if (!IsFirstTimeLaunch())
				{
					if (_appUsageCount <= m_settings.SuccessivePromptAfterLaunches)
						return false;
				}
				
				// Store information in the preference file
				PlayerPrefs.SetInt(m_keysCollection.IsFirstTimeLaunchKeyName, 0);
				PlayerPrefs.SetInt(m_keysCollection.AppUsageCountKeyName, 0);
				PlayerPrefs.SetString(m_keysCollection.PromptLastShownKeyName, _utcNow.ToString(System.Globalization.CultureInfo.InvariantCulture));

				return true;
			}
			finally
			{
				PlayerPrefs.Save(); 
			}
		}

		private IEnumerator ShowDialogRoutine ()
		{
			if (Delegate != null)
			{
				while (!Delegate.CanShowRateMyAppDialog())
					yield return new WaitForSeconds(seconds: 1f);
			}

			ShowDialog();
		}

		private void ShowDialog ()
		{
			if (Delegate != null)
				Delegate.OnBeforeShowingRateMyAppDialog();

			List<string> _buttonList	= new List<string>();
			_buttonList.Add(m_settings.RateItButtonText);
			_buttonList.Add(m_settings.RemindMeLaterButtonText);

			if (!string.IsNullOrEmpty(m_settings.DontAskButtonText))
				_buttonList.Add(m_settings.DontAskButtonText);

			m_viewController.ShowDialog(m_settings.Title, 		
			                            m_settings.Message, 
			                            _buttonList.ToArray(), 	
			                            (_buttonName) =>
			{
				if (string.Equals(_buttonName, m_settings.RemindMeLaterButtonText))
				{
					OnPressingRemindMeLaterButton();
				}
				else if (string.Equals(_buttonName, m_settings.RateItButtonText))
				{
					OnPressingRateItButton();
				}
				else
				{
					OnPressingDontShowButton();
				}

				PlayerPrefs.Save();
			});
		}
		
		private void OnPressingRemindMeLaterButton ()
		{
			PlayerPrefs.SetInt(m_keysCollection.ShowPromptAfterKeyName, m_settings.SuccessivePromptAfterHours);

			m_eventResponder.OnRemindMeLater();
		}

		private void OnPressingRateItButton ()
		{
			// Save current version to main bundle
			string _currentVersion	= VoxelBusters.Utility.PlayerSettings.GetBundleVersion();
			PlayerPrefs.SetString(m_keysCollection.VersionLastRatedKeyName, _currentVersion);

			m_eventResponder.OnRate();

		#if USES_SOOMLA_GROW
			NPBinding.SoomlaGrowService.ReportOnUserRating();
		#endif
		}

		private void OnPressingDontShowButton ()
		{
			PlayerPrefs.SetInt(m_keysCollection.DontShowKeyName, 1);

			m_eventResponder.OnDontShow();
		}

		#endregion
	}
}