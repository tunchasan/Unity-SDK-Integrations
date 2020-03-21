#if USES_GAME_SERVICES && UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using VoxelBusters.Utility;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins.Internal
{
	internal sealed class AndroidAchievementDescription : AchievementDescription 
	{
		#region Constants

		private const string	kIdentifier				= "identifier";
		private const string	kTitle					= "title";
		private const string	kAcheivedDescription	= "achieved-description";
		private const string	kUnAcheivedDescription	= "un-achieved-description";
		private const string	kMaximumPoints			= "maximum-points";
		private const string	kImagePath				= "image-path";

		private const string 	kState					= "state";
		private const string 	kStateHidden			= "state-hidden";
		private const string 	kStateUnlocked			= "state-unlocked";
		private const string	kStateRevealed			= "state-revealed";

		private const string	kType					= "type";
		private const string	kTypeStandard			= "type-standard";
		private const string	kTypeIncremental		= "type-incremental";
		
		#endregion

		#region Fields	

		private	string		m_identifier;
		private string		m_title;
		private string 		m_achievedDescription;
		private string 		m_unachievedDescription;
		private	int			m_maximumPoints;
		private	bool		m_isHidden;
		private string 		m_imagePath;

		#endregion

		#region Properties

		public override string Identifier
		{
			get
			{
				return m_identifier;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override	string Title
		{
			get
			{
				return m_title;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override string AchievedDescription
		{
			get
			{
				return m_achievedDescription;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override	string UnachievedDescription
		{
			get
			{
				return m_unachievedDescription;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		[System.Obsolete("This property is deprecated. Instead use NPBinding.GameServices.GetNoOfStepsForCompletingAchievement method.")]
		public override int MaximumPoints
		{
			get
			{
				return m_maximumPoints;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		public override bool IsHidden
		{
			get
			{
				return m_isHidden;
			}
			
			protected set
			{
				throw new Exception("[GameServices] Only getter is supported.");
			}
		}
		
		#endregion
		
		#region Constructors

		private AndroidAchievementDescription ()
		{}

		internal AndroidAchievementDescription (IDictionary _descriptionData)
		{
			// Initialize properties
			m_identifier			= 	_descriptionData.GetIfAvailable<string>(kIdentifier);
			m_title					= 	_descriptionData.GetIfAvailable<string>(kTitle);
			
			m_unachievedDescription	= 	_descriptionData.GetIfAvailable<string>(kUnAcheivedDescription);
			m_achievedDescription	=	GetAchievedDescription(m_title);
			
			m_maximumPoints			= 	_descriptionData.GetIfAvailable<int>(kMaximumPoints);

			string _state			= 	_descriptionData.GetIfAvailable<string>(kState);
			if (_state.Equals(kStateHidden))
			{
				m_isHidden = true;
			}

			m_imagePath				= _descriptionData.GetIfAvailable<string>(kImagePath);
		}
		
		#endregion

		#region Static Methods

		internal static AchievementDescription[] ConvertAchievementDescriptionList (IList _achievementDescriptionList)
		{
			if (_achievementDescriptionList == null)
				return null;
			
			int 								_count								= _achievementDescriptionList.Count;
			AchievementDescription[]			_androidAchievementDescriptionList	= new AndroidAchievementDescription[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
			{
				_androidAchievementDescriptionList[_iter]							= new AndroidAchievementDescription(_achievementDescriptionList[_iter] as IDictionary);
			}
			
			return _androidAchievementDescriptionList;
		}

		#endregion

		#region Overriden Methods

		protected override void RequestForImage ()
		{
			string 	_error 	= null;
				
			if (string.IsNullOrEmpty(m_imagePath))
			{
				_error = "Image not available!";
			}

			RequestForImageFinished(URL.URLWithString(m_imagePath), _error);
		}

		#endregion

		#region Helper Methods
	
		private string GetAchievedDescription(string _achievementTitle)
		{
			string[] _formats = NPSettings.GameServicesSettings.Android.AchievedDescriptionFormats;
			string	_achievedDescription;
			
			if(_formats != null && _formats.Length > 0)
			{
				int _index							=	UnityEngine.Random.Range(0, _formats.Length);
				string _randomDescriptionformat 	=	_formats[_index];

				_achievedDescription = _randomDescriptionformat.Replace("#", _achievementTitle);
			}
			else
			{
				_achievedDescription = _achievementTitle;
			}
			
			return _achievedDescription;
		}		

		#endregion


	}
}
#endif