using UnityEngine;
using System.Collections;
using VoxelBusters.Utility.UnityGUI.MENU;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
#if !USES_WEBVIEW
	public class WebViewDemo : NPDisabledFeatureDemo
#else
	public class WebViewDemo : NPDemoBase
#endif
	{
		#region Properties

#pragma warning disable
		[SerializeField]
		private 	string 		m_url;
		[SerializeField, Multiline(6)]
		private 	string		m_HTMLString;
		[SerializeField]
		private 	string		m_javaScript;
		[SerializeField]
		private 	string		m_evalString;
		[SerializeField]
		private 	string[]	m_schemeList	= new string[] {
			"unity",
			"mailto",
			"tel"
		};
#pragma warning restore

#if USES_WEBVIEW
		private 	WebView		m_webview;
#endif

		#endregion

#if !USES_WEBVIEW
	}
#else
		#region Unity Methods

		protected override void Start ()
		{
			base.Start ();

			// Cache instances
			m_webview	= FindObjectOfType<WebView>();

			// Unset enable feature text
			SetFeatureInterfaceInfoText("WebView works differently when compared to other features supported by our plugin. Here Native WebView is represented as a GameObject. " +
				"So this means you can have multiple instances of WebView in a single view and customise its functionalities as per your preference.");

			// Set info texts
			AddExtraInfoTexts(
				"Either you can create a GameObject with WebView component attached to it. Or else you can make use of WebView prefab provided along with this plugin (Path: Assets/VoxelBusters/NativePlugins/Prefab). "
				);
		}

		protected override void OnEnable ()
		{
			base.OnEnable();

			// Set frame
			if (m_webview != null)
				SetFrame();

			// Registering callbacks
			WebView.DidShowEvent						+= DidShowEvent;
			WebView.DidHideEvent						+= DidHideEvent;
			WebView.DidDestroyEvent						+= DidDestroyEvent;
			WebView.DidStartLoadEvent					+= DidStartLoadEvent;
			WebView.DidFinishLoadEvent					+= DidFinishLoadEvent;
			WebView.DidFailLoadWithErrorEvent			+= DidFailLoadWithErrorEvent;
			WebView.DidFinishEvaluatingJavaScriptEvent	+= DidFinishEvaluatingJavaScriptEvent;
			WebView.DidReceiveMessageEvent				+= DidReceiveMessageEvent;
		}

		protected override void OnDisable ()
		{
			base.OnDisable();

			// Deregistering callbacks
			WebView.DidShowEvent						-= DidShowEvent;
			WebView.DidHideEvent						-= DidHideEvent;
			WebView.DidDestroyEvent						-= DidDestroyEvent;
			WebView.DidStartLoadEvent					-= DidStartLoadEvent;
			WebView.DidFinishLoadEvent					-= DidFinishLoadEvent;
			WebView.DidFailLoadWithErrorEvent			-= DidFailLoadWithErrorEvent;
			WebView.DidFinishEvaluatingJavaScriptEvent	-= DidFinishEvaluatingJavaScriptEvent;
			WebView.DidReceiveMessageEvent				-= DidReceiveMessageEvent;
		}

		#endregion

		#region GUI Methods

		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities();

			if (m_webview == null)
			{
				GUILayout.Label("Create WebView", kSubTitleStyle);

				if (GUILayout.Button("Create"))
				{
					GameObject _newWebviewGO	= new GameObject("WebView");
					m_webview					= _newWebviewGO.AddComponent<WebView>();

					AddNewResult("Successfully created new WebView.");
				}

				return;
			}

			DrawLoadAPI();
			DrawLifeCycleAPI();
			DrawControlTypes();
			DrawPropertiesAPI();

			// Misc
			GUILayout.Label("Misc.", kSubTitleStyle);

			if (GUILayout.Button("Add New URL Scheme Name"))
			{
				AddNewURLSchemeName();
			}

			GUILayout.Box ("[NOTE] You will receive DidReceiveMessageEvent, when webview tries to load URL which starts with currently watched URL Scheme's.");

			if (GUILayout.Button("Clear Cache"))
			{
				ClearCache();
			}
		}

		private void DrawLoadAPI ()
		{
			GUILayout.Label("Load URL", kSubTitleStyle);

			m_url = GUILayout.TextField(m_url);

			if (GUILayout.Button("Load"))
			{
				LoadRequest();
			}

			GUILayout.Label("Other Load Operations", kSubTitleStyle);

			if (GUILayout.Button("Load HTML String"))
			{
				LoadHTMLString();
			}

			if (GUILayout.Button("Load HTML String With JavaScript"))
			{
				LoadHTMLStringWithJavaScript();
			}

			if (GUILayout.Button("Load File"))
			{
				LoadFile();
			}

			GUILayout.Box ("[NOTE] You will receive DidStartLoadEvent, when webview starts loading page.");
			GUILayout.Box ("[NOTE] You will receive DidFinishLoadEvent, when webview finishes loading page.");
			GUILayout.Box ("[NOTE] You will receive DidFailLoadWithErrorEvent, when webview fails to load page.");

			if (GUILayout.Button("Evaluate JavaScript"))
			{
				EvaluateJavaScriptFromString();
			}

			GUILayout.Box ("[NOTE] You will receive DidFinishEvaluatingJavaScriptEvent, when webview finishes evaluating javascript expression.");
		}

		private void DrawLifeCycleAPI ()
		{
			GUILayout.Label("Lifecycle", kSubTitleStyle);

			if (GUILayout.Button("Show"))
			{
				ShowWebView();
			}

			GUILayout.Box ("[NOTE] DidShowEvent is fired, when webview appears on the screen.");

			if (GUILayout.Button("Hide"))
			{
				HideWebView();
			}

			GUILayout.Box ("[NOTE] DidHideEvent is fired, when webview is removed from the screen.");

			if (GUILayout.Button("Destroy"))
			{
				DestroyWebView();
			}

			GUILayout.Box ("[NOTE] DidDestroyEvent is fired, when webview is destroyed.");
		}

		private void DrawControlTypes ()
		{
			GUILayout.Label("Control Types", kSubTitleStyle);

			GUILayout.BeginHorizontal();
			{
				bool _usingNoControls	= m_webview.ControlType == eWebviewControlType.NO_CONTROLS;
				bool _usingCloseButton	= m_webview.ControlType == eWebviewControlType.CLOSE_BUTTON;
				bool _usingToolbar		= m_webview.ControlType == eWebviewControlType.TOOLBAR;

				if (_usingNoControls != GUILayout.Toggle(_usingNoControls, "No Controls"))
					m_webview.ControlType	= eWebviewControlType.NO_CONTROLS;

				if (_usingCloseButton != GUILayout.Toggle(_usingCloseButton, "Close Button"))
					m_webview.ControlType	= eWebviewControlType.CLOSE_BUTTON;

				if (_usingToolbar != GUILayout.Toggle(_usingToolbar, "Tool Bar"))
					m_webview.ControlType	= eWebviewControlType.TOOLBAR;
			}
			GUILayout.EndHorizontal();
		}

		private void DrawPropertiesAPI ()
		{
			GUILayout.Label("Properties", kSubTitleStyle);

			GUILayout.BeginVertical(UISkin.scrollView);
			GUILayout.BeginHorizontal();

			bool _canHideNewValue				= GUILayout.Toggle(m_webview.CanHide, "CanHide");
			bool _canBounceNewValue				= GUILayout.Toggle(m_webview.CanBounce, "CanBounce");
			bool _showSpinnerOnLoadNewValue		= GUILayout.Toggle(m_webview.ShowSpinnerOnLoad, "ShowSpinnerOnLoad");

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			bool _autoShowOnLoadFinishNewValue	= GUILayout.Toggle(m_webview.AutoShowOnLoadFinish, "AutoShowOnLoadFinish");
			bool _scalesPageToFitNewValue		= GUILayout.Toggle(m_webview.ScalesPageToFit, "ScalesPageToFit");

			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

			// Update the value only on value change
			if (_canHideNewValue != m_webview.CanHide)
				m_webview.CanHide				= _canHideNewValue;

			if (_canBounceNewValue != m_webview.CanBounce)
				m_webview.CanBounce				= _canBounceNewValue;

			if (_showSpinnerOnLoadNewValue != m_webview.ShowSpinnerOnLoad)
				m_webview.ShowSpinnerOnLoad		= _showSpinnerOnLoadNewValue;

			if (_autoShowOnLoadFinishNewValue != m_webview.AutoShowOnLoadFinish)
				m_webview.AutoShowOnLoadFinish	= _autoShowOnLoadFinishNewValue;

			if (_scalesPageToFitNewValue != m_webview.ScalesPageToFit)
				m_webview.ScalesPageToFit		= _scalesPageToFitNewValue;

			if (GUILayout.Button("Set Frame"))
			{
				SetFrame();
			}

			if (GUILayout.Button("Set Full Screen Frame"))
			{
				SetFullScreenFrame();
			}
		}

		#endregion

		#region API Methods

		private void LoadRequest ()
		{
			m_webview.LoadRequest(m_url);
		}

		private void LoadHTMLString ()
		{
			m_webview.LoadHTMLString(m_HTMLString);
		}

		private void LoadHTMLStringWithJavaScript ()
		{
			m_webview.LoadHTMLStringWithJavaScript(m_HTMLString, m_javaScript);
		}

		private void LoadFile ()
		{
			m_webview.LoadFile(Demo.Utility.GetScreenshotPath(), "image/png", null, null);
		}

		private void EvaluateJavaScriptFromString ()
		{
			m_webview.EvaluateJavaScriptFromString(m_evalString);
		}

		private void ShowWebView ()
		{
			m_webview.Show();
		}

		private void HideWebView ()
		{
			m_webview.Hide();
		}

		private void DestroyWebView ()
		{
			m_webview.Destroy();
		}

		private void AddNewURLSchemeName ()
		{
			AddNewResult("Registered schemes for receiving web view messages.");

			for (int _iter = 0; _iter < m_schemeList.Length; _iter++)
				m_webview.AddNewURLSchemeName(m_schemeList[_iter]);
		}

		private void SetFrame ()
		{
			m_webview.Frame		= new Rect(0f, Screen.height * 0.75f, Screen.width, Screen.height * 0.2f);

			AddNewResult(string.Format("Setting new frame: {0} for web view.", m_webview.Frame));
		}

		private void SetFullScreenFrame ()
		{
			m_webview.SetFullScreenFrame();

			AddNewResult("Setting web view frame to full screen.");
		}

		private void ClearCache ()
		{
			m_webview.ClearCache();

			AddNewResult("Cleared web view cache.");
		}

		#endregion

		#region API Callback Methods

		private void DidShowEvent (WebView _webview)
		{
			AddNewResult("Displaying web view on screen.");
		}

		private void DidHideEvent (WebView _webview)
		{
			AddNewResult("Dismissed web view from the screen.");
		}

		private void DidDestroyEvent (WebView _webview)
		{
			AddNewResult("Released web view instance.");
		}

		private void DidStartLoadEvent (WebView _webview)
		{
			AddNewResult("Started loading webpage contents.");
			AppendResult(string.Format("URL: {0}.", _webview.URL));
		}

		private void DidFinishLoadEvent (WebView _webview)
		{
			AddNewResult("Finished loading webpage contents.");
		}

		private void DidFailLoadWithErrorEvent (WebView _webview, string _error)
		{
			AddNewResult("Failed to load requested contents.");
			AppendResult(string.Format("Error: {0}.", _error));
		}

		private void DidFinishEvaluatingJavaScriptEvent (WebView _webview, string _result)
		{
			AddNewResult("Finished evaluating JavaScript script.");
			AppendResult(string.Format("Result: {0}.", _result));
		}

		private void DidReceiveMessageEvent (WebView _webview,  WebViewMessage _message)
		{
			AddNewResult("Received a new message from web view.");
			AppendResult(string.Format("Host: {0}.", 		_message.Host));
			AppendResult(string.Format("Scheme: {0}.", 		_message.Scheme));
			AppendResult(string.Format("URL: {0}.", 		_message.URL));
			AppendResult(string.Format("Arguments: {0}.", 	_message.Arguments.ToJSON()));
		}

		#endregion
	}
#endif
}
