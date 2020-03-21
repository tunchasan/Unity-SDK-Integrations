using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class Menu 
	{
		#region Constants
	
		// Menu item names
		private 	const 	string 	kMenuNodeMainNode					= "Window/Voxel Busters/NativePlugins";
		private 	const 	string 	kMenuNodeSimulation					= kMenuNodeMainNode + "/Editor Simulation";
		public 		const 	string 	kMenuItemPushNotification			= kMenuNodeSimulation + "/Test Notification Payload";

		// Priority
		private 	const 	int		kMenuItemPrioritySimulate			= 100;
		private 	const 	int		kMenuItemPriorityNPSettings			= 120;
		private 	const 	int		kMenuItemPriorityMisc				= 140;
		
		#endregion

		#region Simulation Methods

#if USES_ADDRESS_BOOK
		[MenuItem(kMenuNodeSimulation + "/Open Address Book", false, kMenuItemPrioritySimulate)]
		private static void ShowAddressBook ()
		{
			EditorAddressBook _addressBook	= EditorAddressBook.Instance;

			if (_addressBook != null)
			{
				Selection.activeObject	= _addressBook;
			}
		}
#endif

#if USES_GAME_SERVICES
		[MenuItem(kMenuNodeSimulation + "/Open Game Center", false, kMenuItemPrioritySimulate)]
		private static void SelectGameCenter ()
		{
			EditorGameCenter _gameCenter	= EditorGameCenter.Instance;
			
			if (_gameCenter != null)
				Selection.activeObject		= _gameCenter;
		}
#endif

#if USES_NOTIFICATION_SERVICE
		[MenuItem(kMenuNodeSimulation + "/Open Notification Center", false, kMenuItemPrioritySimulate)]
		private static void ShowNotificationCenter ()
		{
			EditorNotificationCenter _notificationCenter	= EditorNotificationCenter.Instance;

			if (_notificationCenter != null)
			{
				Selection.activeObject	= _notificationCenter;
			}
		}

		[MenuItem(kMenuItemPushNotification, false, kMenuItemPrioritySimulate)]
		private static void ShowPushNotificationService ()
		{
			// Notification center is selected
			ShowNotificationCenter();

			// Show post notification window
			EditorPushNotificationService.ShowWindow();
		}
#endif


#if USES_BILLING
		[MenuItem(kMenuNodeSimulation + "/Helpers/Clear Purchase History", false, kMenuItemPrioritySimulate)]
		private static void ClearPurchases ()
		{
			EditorStore.ClearPurchaseHistory();
		}
#endif

#if USES_GAME_SERVICES
		[MenuItem(kMenuNodeSimulation + "/Helpers/Refresh Game Center", false, kMenuItemPrioritySimulate)]
		private static void RefreshGameCenter ()
		{
			EditorGameCenter.Instance.Refresh();
		}

		[MenuItem(kMenuNodeSimulation + "/Helpers/Reset Game Center Achievements", false, kMenuItemPrioritySimulate)]
		private static void ResetGameAchievements ()
		{
			EditorGameCenter.Instance.ResetAchievements();
		}
#endif

#if USES_NOTIFICATION_SERVICE
		[MenuItem(kMenuNodeSimulation + "/Helpers/Clear Notifications", false, kMenuItemPrioritySimulate)]
		private static void ClearAllNotifications ()
		{
			EditorNotificationCenter.Instance.ClearNotifications();
		}

		[MenuItem(kMenuNodeSimulation + "/Helpers/Cancel Notifications", false, kMenuItemPrioritySimulate)]
		private static void CancelAllNotifications ()
		{
			EditorNotificationCenter.Instance.CancelAllLocalNotifications();
		}
#endif
		
		#endregion

		#region Settings

		[MenuItem(kMenuNodeMainNode + "/Open NPSettings", false, kMenuItemPriorityNPSettings)]
		private static void SelectSettings ()
		{
			NPSettings _npSettings	= NPSettings.Instance;
			
			if (_npSettings != null)
			{
				Selection.activeObject	= _npSettings;
			}
		}

		#endregion

		#region Product

		[MenuItem(kMenuNodeMainNode + "/Welcome Tour", false, kMenuItemPriorityMisc)]
		private static void ShowWelcomeTourWindow ()
		{
			WelcomeTourWindow.ShowWindow();
		}

		[MenuItem(kMenuNodeMainNode + "/Check for Updates", false, kMenuItemPriorityMisc)]
		private static void CheckForUpdates ()
		{
			NPSettings _npSettings	= NPSettings.Instance;
			
			if (_npSettings != null)
			{
				_npSettings.AssetStoreProduct.CheckForUpdates();
			}
		}

#if UNITY_EDITOR && !(UNITY_WEBPLAYER || UNITY_WEBGL || NETFX_CORE)
		[MenuItem(kMenuNodeMainNode + "/Uninstall", false, kMenuItemPriorityMisc)]
		private static void UninstallNativePlugins ()
		{				
			UninstallPlugin.Uninstall();
		}
#endif

		#endregion
	}
}
#endif