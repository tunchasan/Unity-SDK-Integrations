using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.UASUtils.Demo
{
	public class DemoMainMenu : DemoGUIWindow 
	{
		#region Fields

		private 	Dictionary<string, List<DemoSubMenu>> 	m_subMenuCollection		= new Dictionary<string, List<DemoSubMenu>>();
		private 	DemoSubMenu					m_currentSubMenu		= null;

		#endregion
	
		#region Unity Methods

		protected override void Start () 
		{
			base.Start();

			// Initialise
			CollectSubMenuItems();
		
			// Disable all children initially
			DisableAllSubMenus();
		}
	
		private void Update ()
		{
			if (m_currentSubMenu != null && !m_currentSubMenu.gameObject.activeSelf)
			{
				m_currentSubMenu = null;
			}
		}

		#endregion

		#region Helpers

		private void CollectSubMenuItems ()
		{
			// Get list of all DemoSubMenu under this Main Menu
			DemoSubMenu[]	_subMenuList	= this.GetComponentsInChildren<DemoSubMenu>(true);
			
			foreach (DemoSubMenu _currentSubMenu in _subMenuList)
			{
				string				_groupName			= _currentSubMenu.transform.parent.name;
				List<DemoSubMenu>	_demoSubMenuList;

				// Organise sub menu based on grouping
				if (!m_subMenuCollection.TryGetValue(_groupName, out _demoSubMenuList))
				{
					_demoSubMenuList				= new List<DemoSubMenu>();
					m_subMenuCollection[_groupName]	= _demoSubMenuList;
				}

				_demoSubMenuList.Add(_currentSubMenu);

				// Initialise skin
				if (UISkin != null)
				{
					if (_currentSubMenu.UISkin == null)
						_currentSubMenu.UISkin 		= UISkin;
				}
			}
		}

		private void DisableAllSubMenus ()
		{
			foreach (List<DemoSubMenu> _subMenuList in m_subMenuCollection.Values)
			{
				foreach (DemoSubMenu _currentSubMenu in _subMenuList)
					_currentSubMenu.gameObject.SetActive(false);
			}
		}
	
		private void EnableSubMenu (DemoSubMenu _enabledSubMenu)
		{
			DisableAllSubMenus();
	
			// Enable new feature window
			_enabledSubMenu.gameObject.SetActive(true);
			
			// Save the window instance
			m_currentSubMenu = _enabledSubMenu;
		}

		#endregion
		
		#region Drawing

		protected override void OnGUIWindow()
		{		
			if (m_currentSubMenu == null)
			{
				RootScrollView.BeginScrollView();
				{
					foreach (string _groupName in m_subMenuCollection.Keys)
					{
						GUILayout.Box(_groupName);

						// Draw submenu's under this group
						foreach (DemoSubMenu _currentSubMenu in m_subMenuCollection[_groupName])
						{
							if (GUILayout.Button(_currentSubMenu.name))
							{
								EnableSubMenu(_currentSubMenu);
								break;
							}
						}
					}
				}
				RootScrollView.EndScrollView();
				
				GUILayout.FlexibleSpace();
			}
		}

		#endregion
	}
}