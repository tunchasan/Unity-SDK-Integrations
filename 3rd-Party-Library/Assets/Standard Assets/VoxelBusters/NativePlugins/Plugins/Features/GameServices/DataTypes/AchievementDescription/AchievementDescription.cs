using UnityEngine;
using System.Collections;
using System;
using VoxelBusters.Utility;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Represents an object used to describe an achievement's properties such as achievement's title, max points, image etc.
	/// </summary>
	/// <remarks>
	/// \note Your game must authenticate the local user before using any features.
	/// </remarks>
	public abstract class AchievementDescription : NPObject
	{
		#region Fields

		private		Texture2D		m_image;

		#endregion

		#region Properties
		
		/// <summary>
		/// An unified string internally used to identify the achievement across all the supported platforms. (read-only)
		/// </summary>
		public string GlobalIdentifier
		{
			get;
			protected set;
		}

		/// <summary>
		/// A string used to identify the achievement in the current platform. (read-only)
		/// </summary>
		public abstract string Identifier
		{
			get;
			protected set;
		}

		/// <summary>
		/// A localized title for the achievement. (read-only)
		/// </summary>
		public abstract string Title
		{
			get;
			protected set;
		}

		/// <summary>
		/// A localized description to be used after the local user has completed the achievement. (read-only)
		/// </summary>
		public abstract string AchievedDescription
		{
			get;
			protected set;
		}

		/// <summary>
		/// A localized description of the achievement to be used when the local user has not completed the achievement. (read-only)
		/// </summary>
		public abstract string UnachievedDescription
		{
			get;
			protected set;
		}

		/// <summary>
		/// Boolean that states whether this achievement is initially visible to users. (read-only)
		/// </summary>
		public abstract bool IsHidden
		{
			get;
			protected set;
		}

		public string InstanceID
		{
			get;
			private set;
		}

		#endregion

		#region Delegates

		/// <summary>
		/// Delegate that will be called when all the achievement descriptions are retrieved from game server.
		/// </summary>
		/// <param name="_descriptions">An array of <see cref="AchievementDescription"/> objects, that holds description of achievements stored in game server.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void LoadAchievementDescriptionsCompletion (AchievementDescription[] _descriptions, string _error);
	
		#endregion
		
		#region Events
	
		private event DownloadTexture.Completion DownloadImageFinishedEvent;
		
		#endregion

		#region Constructor

		protected AchievementDescription () : base (NPObjectManager.eCollectionType.GAME_SERVICES)
		{}

		#endregion
	
		#region Methods

		/// <summary>
		/// Asynchronously downloads the achievement's image.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void GetImageAsync (DownloadTexture.Completion _onCompletion)
		{
			// Cache callback
			DownloadImageFinishedEvent	= _onCompletion;

			// Using cached information
			if (m_image != null)
			{
				DownloadImageFinishedEvent(m_image, null);
				return;
			}

			// Request for image
			RequestForImage();
		}
		
		protected virtual void RequestForImage ()
		{}
		
		public override string ToString ()
		{
			return string.Format("[AchievementDescription: Identifier={0}, Title={1}, IsHidden={2}]", Identifier, Title, IsHidden);
		}

		#endregion

		#region Event Callback Methods

		protected virtual void RequestForImageFinished (IDictionary _dataDict)
		{}

		protected void RequestForImageFinished (URL _imagePathURL, string _error)
		{
			if (_error != null)
			{
				DownloadImageFinished(null, _error);
				return;
			}
			else
			{
				DownloadTexture _newRequest		= new DownloadTexture(_imagePathURL, true, true);
				_newRequest.OnCompletion		= (Texture2D _image, string _downloadError)=>{

					// Invoke result handler
					DownloadImageFinished(_image, _downloadError);
				};
				_newRequest.StartRequest();
			}
		}

		protected void DownloadImageFinished (Texture2D _image, string _error)
		{
			// Set properties
			m_image	= _image;

			// Send callback
			if (DownloadImageFinishedEvent != null)
				DownloadImageFinishedEvent(_image, _error);
		}

		#endregion

		#region Deprecated Properties

		[System.Obsolete("This property is deprecated. Instead use NPBinding.GameServices.GetNoOfStepsForCompletingAchievement method.")]
		public abstract int MaximumPoints
		{
			get;
			protected set;
		}

		#endregion
	}
}