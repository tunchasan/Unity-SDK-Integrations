using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

namespace Library.PlayerData.Inventory
{
	public class Inventory
	{
		public List<ItemInstance> GetUserInventory()
		{
			List<ItemInstance> UserInventory = new List<ItemInstance>();

			PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
			{

				foreach (ItemInstance item in result.Inventory)
				{
					UserInventory.Add(item);

					Debug.Log(item.DisplayName);
				}

			},(OnInventoryRequestFailed) // Error Callback
			
			);

			return UserInventory;

		}

		private void OnInventoryRequestFailed(PlayFabError error)
		{
			Debug.LogError("PlayFab Inventory Request: " + error.GenerateErrorReport());
		}

	}

}

