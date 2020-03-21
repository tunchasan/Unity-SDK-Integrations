using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

using URLAddress = VoxelBusters.Utility.URL;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides a cross-platform interface to display web contents inside your application.
	/// </summary>
	/// <description>
	/// To do so, drag and drop the WebView prefab into scene heirarchy, which is placed under folder <i>Assets/VoxelBusters/NativePlugins/Prefab</i>. 
	/// And then send it a request to display web content. 
	/// You can also use this class to move back and forward in the history, just like web browser by setting control type to <c>eWebviewControlType.TOOLBAR</c>.
	/// </description>	
	/// <example>
	/// The following code illustrates how to load webpage using web view.
	/// <code>
	/// using UnityEngine;
	/// using System.Collections;
	/// using VoxelBusters.NativePlugins;
	/// 
	/// public class ExampleClass : MonoBehaviour 
	/// {
	/// 	public WebView webView;
	/// 
	/// 	private void Start ()
	/// 	{
	/// 		// Set web view properties
	/// 		webView.AutoShowOnLoadFinish	= true; // Webview will show itself when request completes
	/// 		webView.ControlType				= eWebviewControlType.CLOSE_BUTTON; // Creates webview with close button for dismissing it
	/// 
	/// 		// Set to full screen
	/// 		webView.SetFullScreenFrame(); // Sets frame to full screen size
	/// 
	/// 		// Start request
	/// 		webView.LoadRequest("http://www.google.com");
	/// 	}
	/// 
	/// 	private void OnEnable ()
	/// 	{
	/// 		// Registering for event
	///			WebView.DidStartLoadEvent			+= OnDidStartLoadEvent;
	///			WebView.DidFinishLoadEvent			+= OnDidFinishLoadEvent;
	///			WebView.DidFailLoadWithErrorEvent	+= OnDidFailLoadWithErrorEvent;
	///     }
	/// 
	/// 	private void OnDisable ()
	/// 	{
	/// 		// Unregistering event
	///			WebView.DidStartLoadEvent			-= OnDidStartLoadEvent;
	///			WebView.DidFinishLoadEvent			-= OnDidFinishLoadEvent;
	///			WebView.DidFailLoadWithErrorEvent	-= OnDidFailLoadWithErrorEvent;
	/// 	}
	/// 
	/// 	private void DidStartLoadEvent (WebView _webview)
	/// 	{
	/// 		if (this.webView == _webview)
	/// 		{
	/// 			Debug.Log("Webview did start loading request.");
	/// 		}
	/// 	}
	/// 	
	/// 	private void DidFinishLoadEvent (WebView _webview)
	/// 	{
	/// 		if (this.webView == _webview)
	/// 		{
	/// 			Debug.Log("Webview did finish loading request.");
	/// 		}
	/// 	}
	/// 	
	/// 	private void DidFailLoadWithErrorEvent (WebView _webview, string _error)
	/// 	{
	/// 		if (this.webView == _webview)
	/// 		{
	/// 			Debug.Log("Webview did fail with error: " + _error);
	/// 		}
	/// 	}
	/// }
	/// </code>
	/// </example>
	public class WebView : MonoBehaviour 
	#if USES_WEBVIEW
	{
		#region Fields

#pragma warning disable
		[SerializeField]
		private 	bool				m_canHide				= true;
		[SerializeField]
		private 	bool				m_canBounce				= true;
		[SerializeField]
		private 	eWebviewControlType	m_controlType;
		[SerializeField]
		private 	bool				m_showSpinnerOnLoad;
		[SerializeField]
		private 	bool				m_autoShowOnLoadFinish	= true;
		[SerializeField]
		private 	bool				m_scalesPageToFit		= true;
		[SerializeField]		
		private 	Rect				m_frame					= new Rect(0f, 0f, -1f, -1f);
		[SerializeField]
		private 	Color				m_backgroundColor		= Color.white;
#pragma warning restore

		#endregion

		#region Properties
		
		internal string UniqueID 
		{ 
			get; 
			private set; 
		}

		/// <summary>
		/// The web view content URL. (read-only)
		/// </summary>
		public string URL
		{
			get;
			private set;
		}

		/// <summary>
		/// A Boolean value indicating whether the web view can be dismissed on close button click.
		/// </summary>
		/// <value><c>true</c> if web view can be dismissed; otherwise, <c>false</c>.</value>
		public bool CanHide
		{
			get 
			{ 
				return m_canHide; 
			}

			set 
			{ 
				m_canHide	= value; 

				// Native webview call
				WebViewNative _webViewNative	= NPBinding.WebView;
				if (_webViewNative != null)
				{
					_webViewNative.Platform.SetCanHide(
						_tag: this.UniqueID,
						_canHide: value
					);
				}
			}
		}

		/// <summary>
		/// A Boolean value that controls whether the web view bounces past the edge of content and back again.
		/// </summary>
		/// <value><c>true</c> if web view can bounce; otherwise, <c>false</c>.</value>
		public bool CanBounce
		{
			get 
			{ 
				return m_canBounce; 
			}

			set 
			{ 
				m_canBounce		= value; 
				
				// Native webview call
				WebViewNative _webViewNative	= NPBinding.WebView;
				if (_webViewNative != null)
				{
					_webViewNative.Platform.SetCanBounce(
						_tag: this.UniqueID,
						_canBounce: value
					);
				}
			}
		}

		/// <summary>
		/// An enum value that determines the appearence of web view.
		/// </summary>
		public eWebviewControlType ControlType
		{
			get 
			{
				return m_controlType; 
			}

			set 
			{ 
				m_controlType	= value; 
				
				// Native webview call
				WebViewNative _webViewNative	= NPBinding.WebView;
				if (_webViewNative != null)
				{
					_webViewNative.Platform.SetControlType(
						_tag: this.UniqueID,
						_type: m_controlType
					);
				}
			}
		}
		
		/// <summary>
		/// A boolean value indicating whether an activity spinner should be displayed while loading webpage.
		/// </summary>
		/// <value><c>true</c> if web view can show spinner while loading webpage; otherwise, <c>false</c>.</value>
		public bool ShowSpinnerOnLoad
		{
			get 
			{ 
				return m_showSpinnerOnLoad; 
			}
			
			set 
			{ 
				m_showSpinnerOnLoad	= value; 
				
				// Native webview call
				WebViewNative _webViewNative	= NPBinding.WebView;
				if (_webViewNative != null)
				{
					_webViewNative.Platform.SetShowSpinnerOnLoad(
						_tag: this.UniqueID,
						_showSpinner: value
					);
				}
			}
		}

		/// <summary>
		/// A boolean value indicating whether web view can auto show itself when load request is finished.
		/// </summary>
		/// <value><c>true</c> if web view can auto show itself; otherwise, <c>false</c>.</value>
		public bool AutoShowOnLoadFinish
		{
			get
			{ 
				return m_autoShowOnLoadFinish; 
			}

			set 
			{ 
				m_autoShowOnLoadFinish	= value; 
				
				// Native webview call
				WebViewNative _webViewNative	= NPBinding.WebView;
				if (_webViewNative != null)
				{
					_webViewNative.Platform.SetAutoShowOnLoadFinish(
						_tag: this.UniqueID,
						_autoShow: value
					);
				}
			}
		}

		/// <summary>
		/// A boolean value indicating whether web view scales webpages to fit the view and the user can change the scale.
		/// </summary>
		/// <value><c>true</c> if web view scales page to fit; otherwise, <c>false</c>.</value>
		public bool ScalesPageToFit
		{
			get 
			{
				return m_scalesPageToFit; 
			}

			set 
			{ 
				m_scalesPageToFit		= value; 
				
				// Native webview call
				WebViewNative _webViewNative	= NPBinding.WebView;
				if (_webViewNative != null)
				{
					_webViewNative.Platform.SetScalesPageToFit(
						_tag: this.UniqueID,
						_scaleToFit: value
					);
				}
			}
		}

		/// <summary>
		/// The frame rectangle, which describes the web view’s position and size.
		/// </summary>
		public Rect Frame
		{
			get 
			{ 
				return m_frame; 
			}

			set 
			{ 
				m_frame	= value; 

				// Incase if user forgets to set width
				if (m_frame.width == -1f)
					m_frame.width	= Screen.width;

				// Incase if user forgets to set height
				if (m_frame.height	== -1f)
					m_frame.height	= Screen.height;
				
				// Native webview call
				WebViewNative _webViewNative	= NPBinding.WebView;
				if (_webViewNative != null)
				{
					_webViewNative.Platform.SetFrame(
						_tag: this.UniqueID,
						_frame: value
					);
				}
			}
		}

		/// <summary>
		/// The web view’s background color.
		/// </summary>
		public Color BackgroundColor
		{
			get 
			{ 
				return m_backgroundColor; 
			}

			set 
			{ 
				m_backgroundColor	= value; 
				
				// Native webview call
				WebViewNative _webViewNative	= NPBinding.WebView;
				if (_webViewNative != null)
				{
					_webViewNative.Platform.SetBackgroundColor(
						_tag: this.UniqueID,
						_color: value
					);
				}
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Delegate for web view's generic callbacks.
		/// </summary>
		/// <param name="_webview">The web view that caused this event.</param>
		public delegate void WebViewEvent (WebView _webview);

		/// <summary>
		/// Delegate that will be called when web view fails to load requested content.
		/// </summary>
		/// <param name="_webview">The web view that failed to load content.</param>
		/// <param name="_error">The error that occurred during loading.</param>
		public delegate void WebViewFailedLoad (WebView _webview, string _error);

		/// <summary>
		/// Delegate that will be called when web view finishes running a JavaScript script.
		/// </summary>
		/// <param name="_webview">The web view that has finished executing script.</param>
		/// <param name="_result">The result received upon running the JavaScript script. You can even get <c>null</c> value, if web view fails to run the script.</param>
		public delegate void WebViewFinishedEvaluatingJS (WebView _webview, string _result);

		/// <summary>
		/// Delegate that will be called when web view sends a message to Unity.
		/// </summary>
		/// <description>
		/// <para>
		/// When a webpage with registered scheme starts loading, webview raises <c>WebView.DidReceiveMessageEvent</c> along with <c>WebViewMessage</c> object. 
		/// Using <c>WebViewMessage</c> object, you can get additional information about URL such as host, scheme and arguments. 
		/// </para>
		/// You can register your own scheme, by calling <see cref="WebView.AddNewURLSchemeName"/> before starting load request.
		/// </description>
		/// <param name="_webview">The web view that sent this message.</param>
		/// <param name="_message">The object holds information about URL.</param>
		public delegate void WebViewReceivedMessage (WebView _webview, WebViewMessage _message);
	
		#endregion

		#region Events

		/// <summary>
		/// Event that will be called when web view is first displayed.
		/// </summary>
		public static event WebViewEvent DidShowEvent;

		/// <summary>
		/// Event that will be called when web view is dismissed.
		/// </summary>
		public static event WebViewEvent DidHideEvent;

		/// <summary>
		/// Event that will be called when web view is destroyed.
		/// </summary>
		public static event WebViewEvent DidDestroyEvent;

		/// <summary>
		/// Event that will be called when web view begins load request.
		/// </summary>
		public static event WebViewEvent DidStartLoadEvent;

		/// <summary>
		/// Event that will be called when web view has finished loading.
		/// </summary>
		public static event WebViewEvent DidFinishLoadEvent;

		/// <summary>
		/// Event that will be called when web view has failed to load requested content.
		/// </summary>
		public static event WebViewFailedLoad DidFailLoadWithErrorEvent;

		/// <summary>
		/// Event that will be called when web view has finished executing JavaScript script.
		/// </summary>
		public static event WebViewFinishedEvaluatingJS DidFinishEvaluatingJavaScriptEvent;

		/// <summary>
		/// Event that will be called when web view passes a message to Unity.
		/// </summary>
		/// <description>
		/// This approach is used for communicating web view with Unity. 
		/// To do so, register your scheme by calling <see cref="WebView.AddNewURLSchemeName"/>.
		/// The web view will then listen to the URL's being loaded and raises <c>DidReceiveMessageEvent</c> event whenever a matching scheme is found.
		/// </description>
		public static event WebViewReceivedMessage DidReceiveMessageEvent;

		#endregion

		#region Unity Methods

		private void Awake ()
		{			
			// Assign unique id
			UniqueID			= GetInstanceID().ToString();
			
			//Consider updating with predefined constants.
			if (m_frame.width 	==	-1f)
				m_frame.width	= Screen.width;
			
			if (m_frame.height	==	-1f)
				m_frame.height	= Screen.height;
			
			// Create webview
			NPBinding.WebView.Create(this, m_frame);
			
			// Set properties
			CanHide					= m_canHide;
			CanBounce				= m_canBounce;
			ControlType				= m_controlType; 
			ShowSpinnerOnLoad		= m_showSpinnerOnLoad;
			AutoShowOnLoadFinish	= m_autoShowOnLoadFinish;
			ScalesPageToFit			= m_scalesPageToFit;
			BackgroundColor			= m_backgroundColor;
		}

		#endregion

		#region Webview Lifecycle

		/// <summary>
		/// Destroys the web view instance.
		/// </summary>
		/// <remarks>
		/// <c>DidDestroyEvent</c> event will be called when object is destroyed.
		/// </remarks>
		public void Destroy ()
		{
			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.Destroy(_tag: this.UniqueID);
			}
		}

		/// <summary>
		/// Displays the web view on the top of Unity view.
		/// </summary>
		public void Show ()
		{
			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.Show(_tag: this.UniqueID);
			}
		}

		/// <summary>
		/// Dismisses the web view.
		/// </summary>
		public void Hide ()
		{
			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.Hide(_tag: this.UniqueID);
			}
		}

		#endregion

		#region Load API's

		/// <summary>
		/// Connects to a given URL and asynchronous loads the content.
		/// </summary>
		/// <param name="_URL">A URL identifying the location of the content to load.</param>
		/// <remarks>
		/// \note Don’t use this method to load local HTML files, instead use <see cref="LoadHTMLStringContentsOfFile"/>.
		/// </remarks>
		public void LoadRequest (string _URL)
		{
			if (string.IsNullOrEmpty(_URL) || !_URL.Contains("://"))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[WebView] Load request failed, please use a valid URL");
				return;
			}

			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.LoadRequest(
					_tag: this.UniqueID,
					_URL: _URL
				);
			}
		}

		/// <summary>
		/// Loads the webpage contents from specified file.
		/// </summary>
		/// <param name="_HTMLFilePath">Path of the target file, to use as contents of the webpage.</param>
		/// <param name="_baseURL">The base URL for the content.</param>
		public void LoadHTMLStringContentsOfFile (string _HTMLFilePath, string _baseURL) 
		{
			DownloadAsset _request	= new DownloadAsset(URLAddress.FileURLWithPath(_HTMLFilePath), true);
			_request.OnCompletion	= (WWW _www, string _error) => {

				if (string.IsNullOrEmpty(_error))
				{
					LoadHTMLString(_www.text, _baseURL);
				}
				else
				{
					DebugUtility.Logger.LogError(Constants.kDebugTag, "[WebView] The operation could not be completed. Error=" + _error);
					return;
				}
			};
			_request.StartRequest();
		}

		/// <summary>
		/// Loads the webpage contents and runs JavaScript string.
		/// </summary>
		/// <param name="_HTMLString">The contents of the webpage.</param>
		/// <param name="_javaScript">The JavaScript string to run.</param>
		/// <param name="_baseURL">The base URL for the content.</param>
		public void LoadHTMLStringWithJavaScript (string _HTMLString, string _javaScript, string _baseURL = null)
		{
			// Invalid HTML string
			if (string.IsNullOrEmpty(_HTMLString))
			{
				LoadHTMLString(_HTMLString, _baseURL);
				return;
			}

			// Injecting javascript to html string
			string _HTMLStringWithJS	= _HTMLString;

			if (_javaScript != null)
				_HTMLStringWithJS	+= _javaScript;

			// Load
			LoadHTMLString(_HTMLStringWithJS, _baseURL);
		}

		/// <summary>
		/// Loads the webpage contents.
		/// </summary>
		/// <param name="_HTMLString">The contents of the webpage.</param>
		/// <param name="_baseURL">The base URL for the content.</param>
		public void LoadHTMLString (string _HTMLString, string _baseURL = null)
		{
			// Invalid HTML string
			if (string.IsNullOrEmpty(_HTMLString))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[WebView] Failed to load HTML contents, HTMLString=" + _HTMLString);
				return;
			}

			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.LoadHTMLString(
					_tag: this.UniqueID,
					_HTMLString: _HTMLString,
					_baseURL: _baseURL
				);
			}
		}

		/// <summary>
		/// Loads the webpage contents from specified file.
		/// </summary>
		/// <param name="_filepath">Path of the target file to use as contents of the webpage.</param>
		/// <param name="_MIMEType">The MIME type of the content.</param>
		/// <param name="_textEncodingName">The content's character encoding name.</param>
		/// <param name="_baseURL">The base URL for the content.</param>
		public void LoadFile (string _filepath, string _MIMEType, string _textEncodingName, string _baseURL)
		{
			DownloadAsset _request	= new DownloadAsset(new URLAddress(_filepath), true);
			_request.OnCompletion	= (WWW _www, string _error) => {
				
				if (string.IsNullOrEmpty(_error))
				{
					LoadData(_www.bytes, _MIMEType, _textEncodingName, _baseURL);
				}
				else
				{
					OnDidFailLoadWithError(null, "[WebView] The operation could not be completed. Error = " + _error);
					return;
				}
			};
			_request.StartRequest();
		}

		/// <summary>
		/// Loads the webpage contents from the given data.
		/// </summary>
		/// <param name="_byteArray">The data to use as the contents of the webpage.</param>
		/// <param name="_MIMEType">The MIME type of the content.</param>
		/// <param name="_textEncodingName">The content's character encoding name.</param>
		/// <param name="_baseURL">The base URL for the content.</param>
		public void LoadData (byte[] _byteArray, string _MIMEType, string _textEncodingName, string _baseURL)
		{
			if (_byteArray == null)
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[WebView] Load data failed");
				return;
			}

			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.LoadData(
					_tag: this.UniqueID,
					_byteArray: _byteArray, 
					_MIMEType: _MIMEType, 
					_textEncodingName: _textEncodingName, 
					_baseURL: _baseURL
				);
			}
		}

		/// <summary>
		/// Executes a JavaScript string.
		/// </summary>
		/// <param name="_javaScript">The JavaScript string to evaluate.</param>
		public void EvaluateJavaScriptFromString (string _javaScript)
		{
			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.EvaluateJavaScriptFromString(
					_tag: this.UniqueID,
					_javaScript: _javaScript
				);
			}
		}

		/// <summary>
		/// Reloads the current page.
		/// </summary>
		public void Reload ()
		{
			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.Reload(_tag: this.UniqueID);
			}
		}

		/// <summary>
		/// Stops loading the current page contents.
		/// </summary>
		public void StopLoading ()
		{
			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.StopLoading(_tag: this.UniqueID);
			}
		}

		#endregion

		#region URL Scheme

		/// <summary>
		/// Registers the specified scheme, after which web view will start to listen for custom URL.
		/// </summary>
		/// <description>
		/// This approach is used for communicating web view with Unity. 
		/// When web view starts loading contents, it will check against registered schemes. 
		/// And incase if a match is found, web view will raise <c>DidReceiveMessageEvent</c> along with URL information.
		/// </description>
		/// <param name="_URLSchemeName">The scheme name of the URL.</param>
		public void AddNewURLSchemeName (string _URLSchemeName)
		{
			if (string.IsNullOrEmpty(_URLSchemeName))
			{
				DebugUtility.Logger.LogError(Constants.kDebugTag, "[WebView] Failed to add URL scheme");
				return;
			}

			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.AddNewURLSchemeName(
					_tag: this.UniqueID,
					_newURLSchemeName: _URLSchemeName
				);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the web view frame to full screen size.
		/// </summary>
		public void SetFullScreenFrame ()
		{
			Frame	= new Rect(0f, 0f, Screen.width, Screen.height);
		}

		#endregion

		#region Cache Clearence

		/// <summary>
		/// Clears all stored cached URL responses.
		/// </summary>
		public void ClearCache ()
		{
			WebViewNative _webViewNative	= NPBinding.WebView;
			if (_webViewNative != null)
			{
				_webViewNative.Platform.ClearCache();
			}
		}

		#endregion

		#region Event Callback Methods

		private void OnDidShow ()
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[WebView] Received showing web view event.");

			// Send event
			if (DidShowEvent != null)
				DidShowEvent(this);
		}
		
		private void OnDidHide ()
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[WebView] Received dismissed web view event.");

			// Send event
			if (DidHideEvent != null)
				DidHideEvent(this);
		}

		private void OnDidDestroy ()
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[WebView] Received destroyed web view event.");

			// Send event
			if (DidDestroyEvent != null)
				DidDestroyEvent(this);
		}
		
		private void OnDidStartLoad ()
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[WebView] Received webpage loading started event.");

			// Send event
			if (DidStartLoadEvent != null)
				DidStartLoadEvent(this);
		}
		
		private void OnDidFinishLoad (string _URL)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[WebView] Received webpage loading finished event.");
			
			// Update properties
			this.URL	= _URL;
			
			// Send event
			if (DidFinishLoadEvent != null)
				DidFinishLoadEvent(this);
		}

		private void OnDidFailLoadWithError (string _URL, string _error)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[WebView] Received webpage loading failed event.");
			
			// Update properties
			this.URL	= _URL;
			
			// Send event
			if (DidFailLoadWithErrorEvent != null)
				DidFailLoadWithErrorEvent(this, _error);
		}

		private void OnDidFinishEvaluatingJavaScript (string _result)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[WebView] Received JavaScript execution finished event.");

			// Send event
			if (DidFinishEvaluatingJavaScriptEvent != null)
				DidFinishEvaluatingJavaScriptEvent(this, _result);
		}

		private void OnDidReceiveMessage (WebViewMessage _message)
		{
			DebugUtility.Logger.Log(Constants.kDebugTag, "[WebView] Received web view message event.");

			// Send event
			if (DidReceiveMessageEvent != null)
				DidReceiveMessageEvent(this, _message);
		}

		#endregion
	}
	#else
	{}
	#endif
}