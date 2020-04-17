using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library.PlayerData.Inventory
{
	public class Inventory
	{
		public static List<ItemInstance> GetUserInventory()
		{
			int gold;

			List<ItemInstance> UserInventory = new List<ItemInstance>();
			
			PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
			{
				foreach (ItemInstance item in result.Inventory)
				{
					UserInventory.Add(item);

					Debug.Log(item.DisplayName);
				}

			},(error) =>

			{
				Debug.LogError("PlayFab Inventory Request: " + error.GenerateErrorReport());
			});
			
			return UserInventory;

		}


		public static void ConsumeItem(string itemID, int consumeCount)
		{
			PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
			{
				ConsumeCount = consumeCount,
				// This is a hex-string value from the GetUserInventory result

				ItemInstanceId = itemID

			}, (result) =>

			{
				Debug.Log("Item Consumed");

			}, (error) =>

			{
				Debug.Log(error.GenerateErrorReport()) ;
			});

		}


		public static void ConsumeItems(List <string> consumedItemList)
		{
			foreach (string item in consumedItemList)
			{
				ConsumeItem(item,1);
			}
			
		}

	}

}

