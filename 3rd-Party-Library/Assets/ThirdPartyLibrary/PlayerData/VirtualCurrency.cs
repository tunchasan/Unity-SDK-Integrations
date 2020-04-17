using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library.PlayerData.Currency
{
    public class VirtualCurrency
    {
        public static void AddUserVirtualCurrency(string currencyType, int amount)
        {
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
            {
                VirtualCurrency = currencyType,

                Amount = amount

            }, (result) =>
            {
                Debug.Log("Added " + amount + " " + currencyType + " to player.");

            }, (error) =>
            {
                Debug.LogError(error.GenerateErrorReport());
            }) ;

        }

        public static void SubtractUserVirtualCurrency(string currencyType, int amount)
        {
            PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest
            {
                VirtualCurrency = currencyType,

                Amount = amount

            }, (result) =>

            {
                Debug.Log("Substracted " + amount + " " + currencyType + " to player.");

            }, (error) =>

            {
                Debug.LogError(error.GenerateErrorReport());
            });

        }

        public static void GetUserVirtualCurrencies(string currencyType, int amount)
        {
            Dictionary<string, int> VirtualCurrency = new Dictionary<string, int>();

            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
            {
                Debug.Log(result.VirtualCurrency.Values);

            }, (error) =>

            {
                Debug.LogError("PlayFab Inventory Request: " + error.GenerateErrorReport());
            });

        }

    }

}


