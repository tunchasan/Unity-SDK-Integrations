using UnityEngine;

#if UNITY_ANDROID && USES_ADDRESS_BOOK
namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class AddressBookAndroid : AddressBook 
	{
		#region Platform Native Info
		
		private class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME			= "com.voxelbusters.nativeplugins.features.addressbook.AddressBookHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				internal const string READ_CONTACTS	= "readContacts";
				internal const string ADD_CONTACT	= "addContact";
				internal const string IS_AUTHORIZED	= "isAuthorized";
			}
		}
		
		#endregion
		
		#region  Native Access Variables
		
		private AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}
#endif