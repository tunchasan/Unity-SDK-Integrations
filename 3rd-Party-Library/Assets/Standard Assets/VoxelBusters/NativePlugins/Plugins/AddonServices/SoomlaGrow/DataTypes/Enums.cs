using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public enum eHighwayConnectionState
	{
		INITIALISED,
		CONNECTED,
		DISCONNECTED
	}

	public enum eSocialProvider 
	{
		FACEBOOK 		= 0,
		FOURSQUARE 		= 1,
		GOOGLE 			= 2,
		LINKEDIN 		= 3,
		MYSPACE 		= 4,
		TWITTER 		= 5,
		YAHOO 			= 6,
		SALESFORCE 		= 7,
		YAMMER 			= 8,
		RUNKEEPER 		= 9,
		INSTAGRAM 		= 10,
		FLICKR 			= 11
	}

	public enum eSocialActionType 
	{
		UPDATE_STATUS 	= 0,
		UPDATE_STORY 	= 1,
		UPLOAD_IMAGE 	= 2,
		GET_CONTACTS 	= 3,
		GET_FEED 		= 4
	}
}