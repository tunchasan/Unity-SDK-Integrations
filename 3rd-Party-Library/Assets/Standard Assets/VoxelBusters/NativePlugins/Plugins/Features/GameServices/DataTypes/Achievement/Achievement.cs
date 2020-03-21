using UnityEngine;
using System.Collections;
using System;
using VoxelBusters.Utility;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Represents an object used to communicate with game server about local user's progress towards completing achievement.
	/// </summary>
	/// <description> 
	/// By default, when an achievement is completed, a notification banner is displayed to the user. 
	/// If your game wants to display its own iterface, you can prevent it by setting <b>Show Default Achievement Completion Banner</b> property in <b>Game Services Settings</b> to <b>NO</b>.
	/// </description>
	/// <remarks>
	/// \note Your game must authenticate the local user before using any features.
	/// </remarks>
	public abstract class Achievement : NPObject
	{
		#region Fields

		protected		double		m_percentageCompleted;

		#endregion

		#region Properties

		/// <summary>
		/// An unique identifier internally used to identify the achievement across all the supported platforms. (read-only)
		/// </summary>
		public string GlobalIdentifier
		{
			get;
			protected set;
		}

		/// <summary>
		/// A string used to identify the achievement in the current platform. (read-only)
		/// </summary>
		public abstract string Identifier
		{
			get;
			protected set;
		}

		/// <summary>
		/// The percentage describes how far the user has progressed on this achievement. (read-only) 
		/// </summary>
		public virtual double PercentageCompleted
		{
			get
			{
				return m_percentageCompleted;
			}

			set
			{
				m_percentageCompleted	= System.Math.Min(100d, value);
			}
		}

		/// <summary>
		/// The bool value indicates whether the current user has completed this achievement. (read-only)
		/// </summary>
		public abstract bool Completed
		{
			get;
			protected set;
		}

		/// <summary>
		/// The last time that progress on the achievement was successfully reported to game server. (read-only)
		/// </summary>
		public abstract DateTime LastReportedDate
		{
			get;
			protected set;
		}

		#endregion

		#region Delegates

		/// <summary>
		/// Delegate that will be called when all the previously reported achievements for the current user are retrieved.
		/// </summary>
		/// <param name="_achievements">An array of <see cref="Achievement"/> objects, that holds all the progress reported to game server.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void LoadAchievementsCompletion (Achievement[] _achievements, string _error);

		/// <summary>
		/// Delegate that will be called when request to report achievement progress is completed.
		/// </summary>
		/// <param name="_success">A bool value used to indicate operation status.</param>
		/// <param name="_error">If the operation was successful, this value is nil; otherwise, this parameter holds the description of the problem that occurred.</param>
		public delegate void ReportProgressCompletion (bool _success, string _error);

		#endregion

		#region Events

		private event ReportProgressCompletion ReportProgressFinishedEvent;

		#endregion

		#region Constructor

		protected Achievement () : base (NPObjectManager.eCollectionType.GAME_SERVICES)
		{}

		protected Achievement (string _globalIdentifier, string _identifier, double _percentageCompleted, bool _completed, DateTime _reportedDate) 
			: base (NPObjectManager.eCollectionType.GAME_SERVICES)
		{	
			// Initialize properties
			GlobalIdentifier	= _globalIdentifier;
			Identifier			= _identifier;
			PercentageCompleted	= _percentageCompleted;
			Completed			= _completed;
			LastReportedDate	= _reportedDate;
		}

		protected Achievement (string _globalIdentifier, string _identifier, double _percentageCompleted = 0) 
			: this (_globalIdentifier, _identifier, _percentageCompleted, false, DateTime.Now)
		{}

		#endregion

		#region Methods

		/// <summary>
		/// Reports the progress of this achievement.
		/// </summary>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public virtual void ReportProgress (ReportProgressCompletion _onCompletion)
		{
			// Cache callback
			ReportProgressFinishedEvent	= _onCompletion;
		}
		
		public override string ToString ()
		{
			return string.Format("[Achievement: Identifier={0}, PercentageCompleted={1}, Completed={2}, LastReportedDate={3}]",
			                     Identifier, PercentageCompleted, Completed, LastReportedDate);
		}
		
		#endregion

		#region Event Callback Methods

		protected virtual void ReportProgressFinished (IDictionary _dataDict)
		{}

		protected void ReportProgressFinished (bool _success, string _error)
		{
			if (ReportProgressFinishedEvent != null)
				ReportProgressFinishedEvent(_success, _error);
		}

		#endregion

		#region Deprecated Properties

		[System.Obsolete("This property is deprecated. Instead use PercentageCompleted property for reporting progress.")]
		public int PointsScored
		{
			get
			{
				return 0;
			}

			set
			{
				throw new System.NotSupportedException("Use PercentageCompleted property for reporting progress.");
			}
		}

		#endregion
	}
}