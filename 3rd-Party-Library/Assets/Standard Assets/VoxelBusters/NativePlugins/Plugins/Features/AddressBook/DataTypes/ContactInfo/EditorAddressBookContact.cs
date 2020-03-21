using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed class EditorAddressBookContact : AddressBookContact 
	{
		#region Constructors
		
		public EditorAddressBookContact (AddressBookContact _source) : base(_source)
		{}
		
		#endregion
	}
}
