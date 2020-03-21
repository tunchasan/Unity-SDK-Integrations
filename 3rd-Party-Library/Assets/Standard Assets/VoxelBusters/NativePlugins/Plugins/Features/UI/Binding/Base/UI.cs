using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Provides a cross-platform interface for creating and presenting native user interfaces.
	/// </summary>
	public partial class UI : MonoBehaviour 
	{
		#region API

		/// <summary>
		/// Shows the toast. A toast is a view containing a quick little message for the user.
		/// </summary>
		/// <param name="_message">The text message in a Toast view.</param>
		/// <param name="_length">Toast view show duration length.</param>
		/// <example>
		/// The following code example demonstrates how to show a short toast message.
		/// <code>
		/// using UnityEngine;
		/// using System.Collections;
		/// using VoxelBusters.NativePlugins;
		/// 
		/// public class ExampleClass : MonoBehaviour 
		/// {
		/// 	public void ShowToastMessage ()
		/// 	{
		/// 		NPBinding.UI.ShowToast("This is a sample message.", eToastMessageLength.SHORT);
		///     }
		/// }
		/// </code>
		/// </example>
		/// <remarks>
		/// \note This is an Android only feature. On iOS, this call has no effect.
		/// </remarks>
		public virtual void ShowToast (string _message, eToastMessageLength _length)
		{}
	
		/// <summary>
		/// Sets position of popover controller to specified position.
		/// </summary>
		/// <description>
		/// Popover controllers are used to Pick Media and to present Share options in iPad. 
		/// And by default, popover controllers are set to (0.0, 0.0) position.
		/// </description>
		/// <param name="_position">Screen position where popover is displayed.</param>
		/// <remarks>
		/// \note This is an iOS only feature. On Android, this call has no effect.
		/// </remarks>
		public virtual void SetPopoverPoint (Vector2 _position)
		{}

		/// <summary>
		/// Sets position of popover controller to last touch position.
		/// </summary>
		/// <description>
		/// Popover controllers are used to Pick Media and to present Share options in iPad. 
		/// And by default, popover controllers are set to (0.0, 0.0) position.
		/// </description>
		/// <remarks>
		/// \note This is an iOS only feature. On Android, this call has no effect.
		/// </remarks>
		public void SetPopoverPointAtLastTouchPosition ()
		{
			Vector2 _touchPosition	= Vector2.zero;

#if UNITY_EDITOR
			_touchPosition			= Input.mousePosition;
#else
			if (Input.touchCount > 0)
			{
				_touchPosition		= Input.GetTouch(0).position;
				_touchPosition.y	= Screen.height - _touchPosition.y;
			}
#endif
			// Set popover position
			SetPopoverPoint(_touchPosition);
		}

		#endregion
	}
}