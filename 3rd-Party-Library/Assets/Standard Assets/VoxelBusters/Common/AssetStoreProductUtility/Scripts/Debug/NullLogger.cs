using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.UASUtils
{
	public class NullLogger : ILogger 
	{
		#region ILogger Implementation

		public bool IsLogTypeAllowed(LogType logType)
		{
			return false;
		}

		public void Log(LogType logType, object message)
		{}

		public void Log(LogType logType, object message, Object context)
		{}

		public void Log(LogType logType, string tag, object message)
		{}

		public void Log(LogType logType, string tag, object message, Object context)
		{}

		public void Log(object message)
		{}

		public void Log(string tag, object message)
		{}

		public void Log(string tag, object message, Object context)
		{}

		public void LogWarning(string tag, object message)
		{}

		public void LogWarning(string tag, object message, Object context)
		{}

		public void LogError(string tag, object message)
		{}

		public void LogError(string tag, object message, Object context)
		{}

		public void LogFormat(LogType logType, string format, params object[] args)
		{}

		public void LogException(System.Exception exception)
		{}

		public ILogHandler logHandler
		{
			get
			{
				return null;
			}
			set
			{}
		}

		public bool logEnabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public LogType filterLogType
		{
			get
			{
				return LogType.Log;
			}
			set
			{}
		}

		#endregion

		#region ILogHandler Implementation

		public void LogFormat(LogType logType, Object context, string format, params object[] args)
		{}

		public void LogException(System.Exception exception, Object context)
		{}

		#endregion
	}
}