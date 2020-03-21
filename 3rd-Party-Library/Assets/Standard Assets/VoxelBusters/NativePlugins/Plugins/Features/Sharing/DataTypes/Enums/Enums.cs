using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	///	The services available for sharing contents from your application.
	/// </summary>
	public enum eShareOptions
	{
		UNDEFINED	= 0,

		/// <summary> Share via messaging service. Can be SMS/MMS or Messager apps on Tablets. </summary>
		MESSAGE,

		/// <summary> Share via email service. </summary>
		MAIL,

		/// <summary> Share via Facebook service. </summary>
		FB,

		/// <summary> Share via Twitter service. </summary>
		TWITTER,

		/// <summary> Share via WhatsApp service. </summary>
		WHATSAPP
	}

	/// <summary>
	///	Possible values for the result, when composer view is dismissed.
	/// </summary>
	public enum eShareResult
	{
		/// <summary> The composer view is dismissed. </summary>
		CLOSED
	}
}

namespace VoxelBusters.NativePlugins.Internal
{
	public enum eSocialServiceType
	{
		TWITTER,
		FB,
		ALL
	}
}