using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.UASUtils
{
	public class DebugUtility 
	{
		#region Static Fields

		private		static		ILogger		nullLogger;

		#endregion

		#region Static Properties

		public static ILogger Logger
		{
			get
			{
				return GetLoggerInstance();
			}
		}

		#endregion

		#region Static Methods

		private static ILogger GetLoggerInstance()
		{
		#if NP_DEBUG
			return Debug.unityLogger;
		#else
			if (nullLogger == null)
				nullLogger = new NullLogger();
			
			return nullLogger;
		#endif
		}

		#endregion
	}
}