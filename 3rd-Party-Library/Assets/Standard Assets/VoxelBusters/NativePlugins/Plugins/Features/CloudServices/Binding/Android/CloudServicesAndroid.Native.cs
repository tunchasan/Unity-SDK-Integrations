using UnityEngine;

#if USES_CLOUD_SERVICES && UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public sealed partial class CloudServicesAndroid : CloudServices 
	{
		#region Platform Native Info
		
		internal class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME			= "com.voxelbusters.nativeplugins.features.cloudservices.CloudServicesHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				internal const string 	INITIALISE				=	"initialise";
				internal const string 	IS_SIGNED_IN			=	"isSignedIn";
				
				internal const string 	LOAD_CLOUD_DATA			=	"loadCloudData";
				internal const string 	LOAD_CLOUD_DATA_SYNC	=	"loadCloudDataSynchronously";
				internal const string 	INITIATE_SAVE_TO_CLOUD	=	"initiateSaveToCloud";
				internal const string 	SAVE_CLOUD_DATA			=	"saveCloudData";
			}
		}
		
		#endregion
		
		
		#region  Native Access Variables
		
		internal static AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}

#endif
