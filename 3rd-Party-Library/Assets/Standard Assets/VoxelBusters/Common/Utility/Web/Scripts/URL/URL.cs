using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility
{
	public struct URL 
	{
		#region Properties

		public string URLString
		{
			get;
			private set;
		}

		#endregion

		#region Constants

		private const string  kFileProtocol			= "file://";
		private const string  kHttpProtocol			= "http://";
		private const string  kProtocolSeperator	= "://";
		#endregion

		#region Constructors

		public URL (string _URLString) : this ()
		{
			if (_URLString.IndexOf(kProtocolSeperator) == -1)
				URLString	= kFileProtocol + _URLString;
			else
				URLString	= _URLString;
		}

		#endregion

		#region Static Methods

		public static URL FileURLWithPath (string _rootPath, string _relativePath)
		{
			return FileURLWithPath(_rootPath + "/" + _relativePath);
		}

		public static URL FileURLWithPath (string _filePath)
		{
			string	_URLWithScheme	= _filePath;
			
			if (_filePath != null)
			{
				if (_filePath.IndexOf(kProtocolSeperator) == -1)
					_URLWithScheme	= kFileProtocol + _filePath;
			}

			return new URL()
			{
				URLString	= _URLWithScheme
			};
		}

		public static URL URLWithString (string _rootURLString, string _relativePath)
		{
			return URLWithString(_rootURLString + "/" + _relativePath);
		}
	
		public static URL URLWithString (string _URLString)
		{
			string	_URLWithScheme	= _URLString;
			
			if (_URLString != null)
			{
				if (_URLString.IndexOf(kProtocolSeperator) == -1)
					_URLWithScheme	= kFileProtocol + _URLString;
			}
			
			return new URL()
			{
				URLString	= _URLWithScheme
			};
		}

		#endregion

		#region Methods

		public bool isFileReferenceURL ()
		{
			return URLString.Contains(kFileProtocol);
		}

		public override string ToString ()
		{
			return string.Format("[URL: {0}]", URLString);
		}

		#endregion
	}
}
