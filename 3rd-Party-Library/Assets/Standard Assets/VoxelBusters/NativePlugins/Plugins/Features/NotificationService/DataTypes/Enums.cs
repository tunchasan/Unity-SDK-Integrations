using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Defines possible options to alert user when a local or push notification arrives.
	/// </summary>
	[System.Flags]
	public enum NotificationType
	{
		/// <summary> Badges the icon. </summary>
		Badge	= 1, 

		/// <summary> Plays a sound. </summary>
		Sound,

		/// <summary> Displays an alert message. </summary>
		Alert 	= 4
	}

	/// <summary>
	/// Defines possible intevals at which the notification can be rescheduled.
	/// </summary>
	public enum eNotificationRepeatInterval
	{
		/// <summary> The system fires the notification once and then discards it. </summary>
		NONE	= 0, 

		/// <summary> The system reschedules the notification delivery for every minute. </summary>
		MINUTE,

		/// <summary> The system reschedules the notification delivery for every hour. </summary>
		HOUR,

		/// <summary> The system reschedules the notification delivery for every day. </summary>
		DAY,

		/// <summary> The system reschedules the notification delivery for every week. </summary>
		WEEK,

		/// <summary> The system reschedules the notification delivery for every month. </summary>
		MONTH,

		/// <summary> The system reschedules the notification delivery for every year. </summary>
		YEAR
	}
}