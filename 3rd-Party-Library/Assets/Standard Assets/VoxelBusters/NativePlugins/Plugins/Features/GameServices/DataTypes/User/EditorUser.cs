using UnityEngine;
using System.Collections;

#if USES_GAME_SERVICES && UNITY_EDITOR
using System;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorUser : User
	{
		#region Properties

		public override string Identifier
		{
			get;
			protected set;
		}
		
		public override string Name
		{
			get;
			protected set;
		}
		
		public override string Alias 
		{
			get;
			protected set;
		}
		
		#endregion
		
		#region Constructors

		internal EditorUser ()
		{}

		public EditorUser (EGCUser _user)
		{
			// Initialize properties
			Identifier		= _user.Identifier;
			Name			= _user.Name;
			Alias			= _user.Alias;
		}
		
		#endregion
		
		#region Methods
		
		protected override void RequestForImage ()
		{
			EditorGameCenter.Instance.GetUserImage(this);
		}
		
		#endregion

		#region Static Methods
		
		public static EditorUser[] ConvertUsersList (IList _gcUsers)
		{
			if (_gcUsers == null)
				return null;
			
			int 			_count			= _gcUsers.Count;
			EditorUser[]	_usersList		= new EditorUser[_count];
			
			for (int _iter = 0; _iter < _count; _iter++)
				_usersList[_iter]			= new EditorUser((EGCUser)_gcUsers[_iter]);
			
			return _usersList;
		}
		
		#endregion
		
		#region Event Callback Methods
		
		protected override void RequestForImageFinished (IDictionary _dataDict)
		{
			string		_error	= _dataDict.GetIfAvailable<string>(EditorGameCenter.kErrorKey);
			Texture2D	_image	= _dataDict.GetIfAvailable<Texture2D>(EditorGameCenter.kImageKey);

			DownloadImageFinished(_image, _error);
		}
		
		#endregion
	}
}
#endif