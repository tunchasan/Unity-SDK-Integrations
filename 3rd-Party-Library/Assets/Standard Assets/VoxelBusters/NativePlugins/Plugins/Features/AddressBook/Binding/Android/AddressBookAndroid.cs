#if UNITY_ANDROID && USES_ADDRESS_BOOK
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class AddressBookAndroid : AddressBook 
	{
		
		#region Constructors
		
		AddressBookAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion
		
		#region Overriden API's

		public override eABAuthorizationStatus GetAuthorizationStatus ()
		{
			bool _accessGranted = Plugin.Call<bool>(Native.Methods.IS_AUTHORIZED);

			if(_accessGranted)
			{
				return eABAuthorizationStatus.AUTHORIZED;
			}
			else
			{
				return eABAuthorizationStatus.DENIED;
			}
		}
		
		protected override void ReadContacts (eABAuthorizationStatus _status, ReadContactsCompletion _onCompletion)
		{
			ReadContactsFinishedEvent = _onCompletion;

			// Native method is called
			Plugin.Call(Native.Methods.READ_CONTACTS);
		}
		
		#endregion
	}
}
#endif