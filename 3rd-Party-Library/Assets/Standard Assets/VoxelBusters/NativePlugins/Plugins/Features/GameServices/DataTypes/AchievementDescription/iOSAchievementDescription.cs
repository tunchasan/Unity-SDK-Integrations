using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class iOSAchievementDescription : AchievementDescription 
	{
		#region Constants

		private 	const 	string 		kIdentifierKey				= "id";
		private 	const 	string 		kTitleKey					= "title";
		private 	const 	string 		kUnachievedDescriptionKey	= "unachieved-desc";
		private 	const 	string 		kAchievedDescriptionKey		= "achieved-desc";
		private 	const 	string 		kMaximumPointsKey			= "max-points";
		private 	const 	string 		kHiddenKey					= "hidden";
		private 	const 	string 		kReplayableKey				= "replayable";
		private		const 	string		kImagePathKey				= "image-path";

		#endregion

		#region Properties

		public override string Identifier
		{
			get;
			protected set;
		}
		
		public override	string Title
		{
			get;
			protected set;
		}
		
		public override string AchievedDescription
		{
			get;
			protected set;
		}
		
		public override	string UnachievedDescription
		{
			get;
			protected set;
		}
		
		[System.Obsolete("This property is deprecated. Instead use NPBinding.GameServices.GetNoOfStepsForCompletingAchievement method.")]
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

		private iOSAchievementDescription ()
		{}
		
		public iOSAchievementDescription (IDictionary _dataDict)
		{
			// Parse data dictionary values
			Identifier				= _dataDict.GetIfAvailable<string>(kIdentifierKey);
			Title					= _dataDict.GetIfAvailable<string>(kTitleKey);
			UnachievedDescription	= _dataDict.GetIfAvailable<string>(kUnachievedDescriptionKey);
			AchievedDescription		= _dataDict.GetIfAvailable<string>(kAchievedDescriptionKey);
#pragma warning disable
			MaximumPoints			= _dataDict.GetIfAvailable<int>(kMaximumPointsKey);
#pragma warning restore
			IsHidden				= _dataDict.GetIfAvailable<bool>(kHiddenKey);

			// Set global identifier
			GlobalIdentifier		= GameServicesUtils.GetAchievementGID(Identifier);
		}
		
		#endregion
		
		#region External Methods
		
		[DllImport("__Internal")]
		private static extern void loadAchievementImage (string _descriptionInfoJSON);
		
		#endregion

		#region Static Methods
		
		public static iOSAchievementDescription[] ConvertAchievementDescriptionsList (IList _descriptionsJSONList)
		{
			if (_descriptionsJSONList == null)
				return null;
			
			int 				_count				= _descriptionsJSONList.Count;
			iOSAchievementDescription[]	_descriptionsList	= new iOSAchievementDescription[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_descriptionsList[_iter]			= new iOSAchievementDescription((IDictionary)_descriptionsJSONList[_iter]);
			
			return _descriptionsList;
		}
		
		#endregion

		#region Methods
		
		protected override void RequestForImage ()
		{
			// Native method call
			loadAchievementImage(GetDescriptionInfoObject().ToJSON());
		}
		
		public IDictionary GetDescriptionInfoObject ()
		{
			IDictionary		_JSONDict	= new Dictionary<string, object>();
			_JSONDict[kIdentifierKey]	= Identifier;
			_JSONDict[GameServicesIOS.kObjectInstanceIDKey]	= GetInstanceID();

			return _JSONDict;
		}
		
		#endregion

		#region Event Callback Methods

		protected override void RequestForImageFinished (IDictionary _dataDict)
		{
			string			_error		= _dataDict.GetIfAvailable<string>(GameServicesIOS.kNativeMessageErrorKey);

			if (_error == null)
			{
				string _imagePath = _dataDict.GetIfAvailable<string>(kImagePathKey);
				RequestForImageFinished(URL.FileURLWithPath(_imagePath), null);
			}
			else
				RequestForImageFinished(URL.FileURLWithPath(null), _error);
		}

		#endregion
	}
}
#endif