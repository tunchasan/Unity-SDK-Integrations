using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class EditorAchievementDescription : AchievementDescription
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
		
		public override string AchievedDescription
		{
			get;
			protected set;
		}
		
		public override string UnachievedDescription
		{
			get;
			protected set;
		}
		
		[System.Obsolete("This property is deprecated.")]
		public override int MaximumPoints
		{
			get;
			protected set;
		}
		
		public override bool IsHidden
		{
			get;
			protected set;
		}

		#endregion

		#region Constructors

		private EditorAchievementDescription ()
		{}

		public EditorAchievementDescription (EGCAchievementDescription _gcDescriptionInfo)
		{
			// Set properties from info object
			Identifier				= _gcDescriptionInfo.Identifier;
			Title					= _gcDescriptionInfo.Title;
			UnachievedDescription	= _gcDescriptionInfo.UnachievedDescription;
			AchievedDescription		= _gcDescriptionInfo.AchievedDescription;
#pragma warning disable
			MaximumPoints			= _gcDescriptionInfo.MaximumPoints;
#pragma warning restore
			IsHidden				= _gcDescriptionInfo.IsHidden; 

			// Set global identifier			
			GlobalIdentifier		= GameServicesUtils.GetAchievementGID(Identifier);
		}

		#endregion

		#region Static Methods
		
		public static EditorAchievementDescription[] ConvertAchievementDescriptionsList (IList _gcDescriptions)
		{
			if (_gcDescriptions == null)
				return null;
			
			int 				_count				= _gcDescriptions.Count;
			EditorAchievementDescription[]	_descriptionsList	= new EditorAchievementDescription[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_descriptionsList[_iter]			= new EditorAchievementDescription((EGCAchievementDescription)_gcDescriptions[_iter]);
			
			return _descriptionsList;
		}
		
		#endregion
		
		#region Methods
		
		protected override void RequestForImage ()
		{
			EditorGameCenter.Instance.GetDescriptionImage(this);
		}
		
		#endregion

		#region Event Callback Methods
		
		protected override void RequestForImageFinished (IDictionary _dataDict)
		{
			string			_error		= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			Texture2D		_image		= _dataDict.GetIfAvailable<Texture2D>(EditorGameCenter.kImageKey);

			DownloadImageFinished(_image, _error);
		}
		
		#endregion
	}
}
#endif