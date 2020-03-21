#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

using Features 	= VoxelBusters.NativePlugins.ApplicationSettings.Features;
using Array		= System.Array;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	[CustomEditor(typeof(NPSettings))]
	public class NPSettingsEditor : AssetStoreProductInspector
	{
		#region Constants

		private		const 	string			kActiveView						= "np-active-view";

		// URL
		private		const 	string			kTutorialURL					= "http://bit.ly/2ssUZpl";		
		private		const	string			kDocumentationURL				= "http://bit.ly/1cBFHDd";
		private		const	string			kForumURL						= "http://bit.ly/1AjQRYp";
		private		const	string			kSubscribePageURL				= "http://bit.ly/2ESQfAg";
		
		// Keys
		private		const	string			kUndoGroupApplicationSettings	= "application-settings";

		// style
		private		const	int				kTitleFontSize					= 16;
		private		const	int				kSubTitleFontSize				= 12;
		private		const	int				kGridRowCount					= 2;

		#endregion

		#region Properties

		// Toolbar tabs
		private 	eSettingsType									m_activeType;
		private		Dictionary<eSettingsType, SerializedProperty>	m_settingsCollection	= new Dictionary<eSettingsType, SerializedProperty>();

		// GUI contents
#pragma warning disable
		private 	GUIContent				m_documentationContent		= new GUIContent("Documentation", 	"See online documentation");
        private 	GUIContent				m_supportContent			= new GUIContent("Support",	"Houston, we have a problem!");
		private 	GUIContent				m_tutotialsContent			= new GUIContent("Tutorials", 		"Read our blog posts about product features and usage");
		private		GUIContent				m_writeReviewContent		= new GUIContent("Review", 			"Share your experience with others");
		private 	GUIContent				m_upgradeContent			= new GUIContent("Upgrade", 		"Click to find out more about full version product");
		private 	GUIContent				m_subscribeContent			= new GUIContent("Subscribe", 		"Stay updated regarding bug fixes and releases");
		private 	GUIContent				m_saveChangesContent		= new GUIContent("Save", 			"Save all your changes");
#pragma warning restore

        private bool        m_recentUpdatesFoldout = true;
        private Vector2     m_recentUpdatesScrollPosition;
		#endregion

		#region Unity Callbacks

		private void OnInspectorUpdate() 
		{
			// Call Repaint on OnInspectorUpdate as it repaints the windows
			// less times as if it was OnGUI/Update
			Repaint();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			// Initialise 
			m_settingsCollection.Add(eSettingsType.APPLICATION_SETTINGS,			serializedObject.FindProperty("m_applicationSettings"));
			m_settingsCollection.Add(eSettingsType.BILLING_SETTINGS,				serializedObject.FindProperty("m_billingSettings"));
			m_settingsCollection.Add(eSettingsType.CLOUD_SERVICES_SETTINGS,			serializedObject.FindProperty("m_cloudServicesSettings"));
			m_settingsCollection.Add(eSettingsType.MEDIA_LIBRARY_SETTINGS,			serializedObject.FindProperty("m_mediaLibrarySettings"));
			m_settingsCollection.Add(eSettingsType.GAME_SERVICES_SETTINGS,			serializedObject.FindProperty("m_gameServicesSettings"));
			m_settingsCollection.Add(eSettingsType.NETWORK_CONNECTVITY_SETTINGS,	serializedObject.FindProperty("m_networkConnectivitySettings"));
			m_settingsCollection.Add(eSettingsType.NOTIFICATION_SERVICE_SETTINGS,	serializedObject.FindProperty("m_notificationSettings"));
			m_settingsCollection.Add(eSettingsType.SOCIAL_NETWORK_SETTINGS,			serializedObject.FindProperty("m_socialNetworkSettings"));
			m_settingsCollection.Add(eSettingsType.UTILITY_SETTINGS,				serializedObject.FindProperty("m_utilitySettings"));
			m_settingsCollection.Add(eSettingsType.WEBVIEW_SETTINGS,				serializedObject.FindProperty("m_webViewSettings"));
			m_settingsCollection.Add(eSettingsType.ADDON_SERVICES_SETTINGS,			serializedObject.FindProperty("m_addonServicesSettings"));

			// Restoring last selection
			m_activeType	= (eSettingsType)EditorPrefs.GetInt(kActiveView, 0);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			// Save changes to settings
			EditorPrefs.SetInt(kActiveView, (int)m_activeType);	
		}

		protected override void OnGUIWindow()
		{
			// draw inspector properties
			GUILayout.BeginVertical(UnityEditorUtility.kOuterContainerStyle);
			{	
				base.OnGUIWindow();

				// disable inspector when its compiling
				GUI.enabled	= !EditorApplication.isCompiling;

				UnityEditorUtility.DrawSplitter(new Color(0.35f, 0.35f, 0.35f), 1, 10);
				DrawTopBar();
				GUILayout.Space(10f);

				if (m_activeType == eSettingsType.NONE)
				{
					DrawMainMenu(_options: GetAvailableTabs());
				}
				else
				{
					DrawDetailedView();
                }

				GUILayout.Space(10f);
				DrawFooter();
				GUILayout.Space(10f);
			}
			GUILayout.EndVertical();

			// reset GUI state
			GUI.enabled	= true;
		}

		#endregion

		#region Misc. Methods

		private void DrawTopBar()
		{
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button(m_documentationContent, Constants.kButtonLeftStyle))
					Application.OpenURL(kDocumentationURL);
				
				if (GUILayout.Button(m_tutotialsContent, Constants.kButtonMidStyle))
					Application.OpenURL(kTutorialURL);

				if (GUILayout.Button(m_supportContent, Constants.kButtonMidStyle))
					Application.OpenURL(kForumURL);
				
				#if NATIVE_PLUGINS_LITE_VERSION
				if (GUILayout.Button(m_upgradeContent, Constants.kButtonMidStyle))
					Application.OpenURL(Constants.kFullVersionProductURL);
				#endif
				
				if (GUILayout.Button(m_writeReviewContent, Constants.kButtonMidStyle))
					Application.OpenURL(Constants.kProductURL);

				if (GUILayout.Button(m_subscribeContent, Constants.kButtonMidStyle))
					Application.OpenURL(kSubscribePageURL);

				if (GUILayout.Button(m_saveChangesContent, Constants.kButtonRightStyle))
					OnPressingSave();
			}
			GUILayout.EndHorizontal();
		}

		private void DrawMainMenu(eSettingsType[] _options)
		{
//			GUILayout.Label("Native Plugins Settings", 
//			                style: CreateBoldLabel(_size: kTitleFontSize, _alignement: TextAnchor.MiddleCenter));

			// get menu button content
			GUIContent[]	_contents	= Array.ConvertAll(_options, (_option) =>
			{
				SerializedProperty _property = m_settingsCollection[_option];
				return new GUIContent(text: _property.displayName, 
				                      tooltip: _property.tooltip,
				                      image : null);
			});

			// display grid buttons
			GUILayout.BeginVertical();
			GUILayout.Space(2f);
			int	_selectedIndex	= GUILayout.SelectionGrid(selected: -1, 
			                                              contents: _contents, 
			                                              xCount: kGridRowCount, 
			                                              style: "LargeButton", 
			                                              options: GUILayout.MinHeight(220f));
			if (_selectedIndex != -1)
			{
				m_activeType	= _options[_selectedIndex];
			}
			GUILayout.EndVertical();
		}

		private void DrawDetailedView()
		{
			SerializedProperty _settingsProperty	= m_settingsCollection[m_activeType];

			// title section
			GUILayout.BeginHorizontal();
			GUILayout.Space(4f);
			if (GUILayout.Button("NP Settings", "GUIEditor.BreadcrumbLeft"))
			{
				m_activeType	= eSettingsType.NONE;
			}
//			GUILayout.FlexibleSpace();
//			GUILayout.BeginVertical();
			GUILayout.Label(_settingsProperty.displayName, "GUIEditor.BreadcrumbMid");
//			GUILayout.EndVertical();
//			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);

			// property detail screen
			_settingsProperty.isExpanded	= true;
			UnityEditorUtility.ForEach(_settingsProperty, (_childProperty) =>
			{
				if (_childProperty.hasChildren && _childProperty.propertyType != SerializedPropertyType.String)
				{
					GUILayout.Label(_childProperty.displayName, "OL Minus");
					UnityEditorUtility.ForEach(_childProperty, (_innerChildProperty) =>
					{
						EditorGUI.indentLevel++;
						EditorGUILayout.PropertyField(_innerChildProperty, true);
						EditorGUI.indentLevel--;
					});
				}
				else
				{
					EditorGUILayout.PropertyField(_childProperty);
				}
			});
		}

		private void DrawFooter()
		{
			Color	_oldColor 		= GUI.color;

			bool	_isModified 	= EditorPrefs.GetBool(NPSettings.kPrefsKeyPropertyModified, false);
			if (_isModified)
			{
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				EditorGUILayout.HelpBox("You've made some changes. \nPlease click on Save Changes button to apply those modifications.", 
				                        MessageType.Warning, 
				                        wide: true);
				GUI.color = Color.red;
				if (GUILayout.Button(m_saveChangesContent, GUILayout.ExpandWidth(false), GUILayout.MinHeight(38f)))
					OnPressingSave();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			GUI.color = _oldColor;


            // Draw updates
            DrawUpdateNews();
		}

        private void DrawUpdateNews()
        {
            string newJson = EditorPrefs.GetString(Constants.kVBNewsKey, "[]");
            IList news = (IList)JSONUtility.FromJSON(newJson);

            if (news.Count > 0)
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUI.indentLevel++;
                m_recentUpdatesFoldout = EditorGUILayout.Foldout(m_recentUpdatesFoldout, "Recent Updates", true);
                if (m_recentUpdatesFoldout)
                {
                    m_recentUpdatesScrollPosition = GUILayout.BeginScrollView(m_recentUpdatesScrollPosition, GUILayout.MinHeight(150f));
                    GUILayout.BeginVertical(EditorStyles.helpBox);

                    foreach (IDictionary eachEntry in news)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        foreach (string key in eachEntry.Keys)
                        {
                            object value = eachEntry[key];

                            if ((value as IDictionary) != null)
                            {
                                IDictionary info = value as IDictionary;
                                string link = info.GetIfAvailable<string>("link");
                                string description = info.GetIfAvailable<string>("description");

                                GUILayout.BeginHorizontal();
                                if (GUILayout.Button(key))
                                    Application.OpenURL(link);
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                                EditorGUI.indentLevel++;
                                GUILayout.Label(description, EditorStyles.wordWrappedLabel);
                                EditorGUI.indentLevel--;
                            }
                            else
                            {
                                GUILayout.Label(key, EditorStyles.boldLabel);
                                EditorGUI.indentLevel++;
                                GUILayout.Label(value.ToString(), EditorStyles.wordWrappedLabel);
                                EditorGUI.indentLevel--;
                            }
                        }

                        GUILayout.Space(6);
                        GUILayout.EndVertical();
                    }

                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                }
                EditorGUI.indentLevel--;
            }
        }

		private eSettingsType[] GetAvailableTabs()
		{
			ArrayList _tabs = new ArrayList();

			Dictionary<eSettingsType, SerializedProperty>.Enumerator _enumerator	= m_settingsCollection.GetEnumerator();
			while (_enumerator.MoveNext())
			{
				KeyValuePair<eSettingsType, SerializedProperty> _current = _enumerator.Current;
				if (!CanDrawTab(_current.Key) || (_current.Value == null) || !_current.Value.hasVisibleChildren)
					continue;

				_tabs.Add(_current.Key);
			}

			return (eSettingsType[])_tabs.ToArray(typeof(eSettingsType));
		}

		private bool CanDrawTab(eSettingsType _tab)
		{
			Features _supportedFeatures = ((NPSettings)target).GetApplicationSettings().SupportedFeatures;
			switch (_tab)
			{
				case eSettingsType.BILLING_SETTINGS:
					return _supportedFeatures.UsesBilling;

				case eSettingsType.CLOUD_SERVICES_SETTINGS:
					return _supportedFeatures.UsesCloudServices;

				case eSettingsType.GAME_SERVICES_SETTINGS:
					return _supportedFeatures.UsesGameServices;

				case eSettingsType.MEDIA_LIBRARY_SETTINGS:
					return _supportedFeatures.UsesMediaLibrary;

				case eSettingsType.NETWORK_CONNECTVITY_SETTINGS:
					return _supportedFeatures.UsesNetworkConnectivity;

				case eSettingsType.NOTIFICATION_SERVICE_SETTINGS:
					return _supportedFeatures.UsesNotificationService;
				
				case eSettingsType.SOCIAL_NETWORK_SETTINGS:
					return _supportedFeatures.UsesTwitter;

				case eSettingsType.WEBVIEW_SETTINGS:
					return _supportedFeatures.UsesWebView;

				default:
					break;
			}

			return true;
		}

		private void OnPressingSave()
		{
			((NPSettings)target).Rebuild();
		}
			   
		#endregion


		#region Style Methods

		public GUIStyle GetBoldLabelStyle(int _size, TextAnchor _alignement = TextAnchor.MiddleLeft)
		{
			GUIStyle _titleStyle 	= new GUIStyle(EditorStyles.boldLabel);
			_titleStyle.alignment	= _alignement;
			_titleStyle.fontSize	= _size;

			return _titleStyle;
		}

		public GUIStyle GetMenuButtonStyle()
		{
			GUIStyle _titleStyle 	= new GUIStyle(GUI.skin.button);
			_titleStyle.fontSize	= 14;

			return _titleStyle;
		}

		#endregion

		#region Nested Types

		private enum eSettingsType
		{
			NONE,
			APPLICATION_SETTINGS,
			BILLING_SETTINGS,
			CLOUD_SERVICES_SETTINGS,
			GAME_SERVICES_SETTINGS,
			MEDIA_LIBRARY_SETTINGS,
			NETWORK_CONNECTVITY_SETTINGS,
			NOTIFICATION_SERVICE_SETTINGS,
			SOCIAL_NETWORK_SETTINGS,
			UTILITY_SETTINGS,
			WEBVIEW_SETTINGS,
			ADDON_SERVICES_SETTINGS
		}

		#endregion
	}
}
#endif