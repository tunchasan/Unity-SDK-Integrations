using UnityEngine;
using System.Collections;

#if UNITY_EDITOR && USES_NOTIFICATION_SERVICE
using UnityEditor;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	[CustomEditor(typeof(EditorNotificationCenter))]
	public class EditorNotificationCenterInspector : CustomScriptableObjectEditor 
	{
		#region Properties

		private EditorNotificationCenter	NotificationCenter
		{
			get 
			{
				return target as EditorNotificationCenter;
			}
		}

		private bool						m_showLocalNotifications			= true;
		private GUIContent					m_localNotificationsGUIContent		= new GUIContent("Received Local Notifications", "Received local notifications");
		private bool						m_showRemoteNotifications			= true;
		private GUIContent					m_remoteNotificationsGUIContent		= new GUIContent("Received Remote Notifications", "Received remote notifications");

		#endregion

		#region Unity Methods

		protected override void OnGUIWindow ()
		{
			GUIStyle _subTitleStyle	= new GUIStyle("BoldLabel");
			_subTitleStyle.wordWrap	= true;

			// Draw properties
			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{
				UnityEditorUtility.DrawLabel("Registration Status", _subTitleStyle, UnityEditorUtility.Alignment.Center);
				GUILayout.Space(2f);
				DrawProperties(GUIStyle.none);
				GUILayout.Space(2f);
			}
			GUILayout.EndVertical();
			GUILayout.Space(5f);

			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{
				UnityEditorUtility.DrawLabel("Validate Notification Payload", _subTitleStyle, UnityEditorUtility.Alignment.Center);
				GUILayout.Space(2f);
				if (GUILayout.Button("Push Notification"))
					EditorApplication.ExecuteMenuItem(Menu.kMenuItemPushNotification);
				GUILayout.Space(2f);
			}
			GUILayout.EndVertical();
			GUILayout.Space(5f);

			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{
				UnityEditorUtility.DrawLabel("Notification Bar", _subTitleStyle, UnityEditorUtility.Alignment.Center);
				GUILayout.Space(2f);
				LayoutLocalNotifications();
				LayoutRemoteNotifications();
				GUILayout.Space(2f);
			}
			GUILayout.EndVertical();
		}

		#endregion

		#region Misc. Methods

		private void LayoutLocalNotifications ()
		{
			m_showLocalNotifications = UnityEditorUtility.DrawPropertyHeader(m_showLocalNotifications, m_localNotificationsGUIContent);
			
			if (m_showLocalNotifications)
			{
				DrawReceivedNotifications(NotificationCenter.LocalNotifications, (_selectedNotification)=>{
					if (_selectedNotification != null)
					{
						NotificationCenter.OnTappingLocalNotification(_selectedNotification);
					}
				});
			}
		}

		private void LayoutRemoteNotifications ()
		{
			m_showRemoteNotifications = UnityEditorUtility.DrawPropertyHeader(m_showRemoteNotifications, m_remoteNotificationsGUIContent);
			
			if (m_showRemoteNotifications)
			{
				DrawReceivedNotifications(NotificationCenter.RemoteNotifications, (_selectedNotification)=>{
					if (_selectedNotification != null)
					{
						NotificationCenter.OnTappingRemoteNotification(_selectedNotification);
					}
				});
			}
		}

		private void DrawReceivedNotifications (IList _notificationList, System.Action<CrossPlatformNotification> _callbackOnTap)
		{
			if (_notificationList == null || _notificationList.Count == 0)
				return;

			foreach (CrossPlatformNotification _notification in _notificationList)
			{
				if (GUILayout.Button(_notification.AlertBody, "LockedHeaderButton"))
				{
					if (_callbackOnTap != null)
					{
						// Set editor as dirty
						EditorUtility.SetDirty(target);
						
						// Send callback
						_callbackOnTap(_notification);
					}
					
					break;
				}
			}
		}
		
		#endregion
	}
}
#endif