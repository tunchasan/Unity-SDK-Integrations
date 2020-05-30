using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library.PlayerData.Currency
{
    public class VirtualCurrency
    {
        public static void AddUserVirtualCurrency(Action successAction, Action<string> errorAction,string currencyType, int amount)
        {
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
            {
                VirtualCurrency = currencyType,

                Amount = amount

            }, (result) =>
            {
                Debug.Log("Added " + amount + " " + currencyType + " to player.");

                successAction();

            }, (error) =>
            {
                //Debug.LogError(error.GenerateErrorReport());

                errorAction(error.GenerateErrorReport());

            });

        }

        public static void SubtractUserVirtualCurrency(Action successAction, Action<string> errorAction, string currencyType, int amount)
        {
            PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest
            {
                VirtualCurrency = currencyType,

                Amount = amount

            }, (result) =>

            {
                Debug.Log("Substracted " + amount + " " + currencyType + " to player.");

                successAction();

            }, (error) =>

            {
                //Debug.LogError(error.GenerateErrorReport());

                errorAction(error.GenerateErrorReport());
            });

        }

        public static void GetUserVirtualCurrencies(Action<Dictionary<string,int>> successAction, Action<string> errorAction)
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
            {
                //Debug.Log(result.VirtualCurrency.Values);

                successAction(result.VirtualCurrency);

            }, (error) =>

            {
                //Debug.LogError("PlayFab Inventory Request: " + error.GenerateErrorReport());

                errorAction("PlayFab Inventory Request: " + error.GenerateErrorReport());
            });

        }

    }

}


