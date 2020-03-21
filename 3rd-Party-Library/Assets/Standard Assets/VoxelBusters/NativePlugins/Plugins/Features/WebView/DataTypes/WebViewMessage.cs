using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Represents an immutable object, that holds information about a message sent from native webview.
	/// </summary>
	/// <description>
	/// <para>
	/// When a webpage with registered scheme starts loading, webview raises <c>WebView.DidReceiveMessageEvent</c> along with <c>WebViewMessage</c> object. 
	/// Using <c>WebViewMessage</c> object, you can get additional information about URL such as host, scheme and arguments. 
	/// </para>
	/// You can register your own scheme, by calling <see cref="WebView.AddNewURLSchemeName"/> before starting load request.
	/// </description>
	/// <example>
	/// Please check following code sample, for easy understanding about scheme registeration and handling messages.
	/// <code>
	/// using UnityEngine;
	/// using System.Collections;
	/// using VoxelBusters.NativePlugins;
	/// 
	/// public class ExampleClass : MonoBehaviour 
	/// {	
	/// 	[SerializeField]
	/// 	private 		WebView 	m_webview;
	/// 	private			string		m_url		= "unity://action?val1=1&val2=2";
	/// 
	/// 	private void Start ()
	/// 	{
	/// 		// Registering scheme of loading url
	/// 		m_webview.AddNewURLSchemeName("unity");
	/// 	}
	/// 
	/// 	private void OnEnable ()
	/// 	{
	/// 		// Registering for event
	/// 	    WebView.DidReceiveMessageEvent	+= OnDidReceiveMessage;
	///     }
	/// 
	/// 	private void OnDisable ()
	/// 	{
	/// 		// Unregistering event
	/// 	    WebView.DidReceiveMessageEvent	-= OnDidReceiveMessage;
	/// 	}
	/// 
	/// 	public void LoadRequest ()
	/// 	{
	/// 		m_webview.LoadRequest(m_url);
	/// 	}
	/// 
	/// 	private void OnDidReceiveMessage (WebView _webview, WebViewMessage _message)
	/// 	{
	/// 		if (_webview == this.m_webview)
	/// 		{
	/// 			Debug.Log("Scheme: " + _webview.SchemeName);
	/// 			Debug.Log("Host: " + _webview.Host);
	/// 
	/// 			foreach (string _argKey in _message.Arguments.Keys)
	/// 				Debug.Log("Arg key: " + _argKey + "value: " + _message.Arguments[_argKey]);
	/// 		}
	/// 	}
	/// }
	/// </code>
	/// Output
	/// <code>
	/// Scheme: unity
	/// Host: action
	/// Arg key: val1 value: 1
	/// Arg key: val2 value: 2 
	/// </code>
	/// </example>
	public class WebViewMessage  
	{
		#region Properties

		/// <summary>
		/// The custom URL that holds information about the web view message. (read-only)
		/// </summary>
		public string URL
		{
			get;
			protected set;
		}

		/// <summary>
		/// The scheme name of the web view message. (read-only)
		/// </summary>
		public string Scheme
		{ 
			get;
			protected set; 
		}

		/// <summary>
		/// The host name of the web view message. (read-only)
		/// </summary>
		public string Host	 							
		{ 
			get; 
			protected set; 
		}

		/// <summary>
		/// The arguments of the web view message. (read-only)
		/// </summary>
		/// <description>
		/// Each key-value pair in dictionary represents argument name and its corresponding value.
		/// </description>
		public Dictionary<string, string> Arguments  	
		{ 
			get; 
			protected set; 
		}

		#endregion

		#region Constructor

		protected WebViewMessage ()
		{
			Scheme		= null;
			Host		= null;
			Arguments	= null;
		}

		#endregion
		
		#region Overriden Methods
		
		public override string ToString ()
		{
			return string.Format("[WebViewMessage Scheme={0}, Host={1}, Arguments={2}]", 
			                     Scheme, Host, Arguments.ToJSON());
		}
		
		#endregion
	}
}