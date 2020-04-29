using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// </summary>
/// 3- Cloud Save with Data Table
/// It allows us to Save & Load Player's Ingame Stats in PlayFab Cloud.
/// </summary>
/// 

namespace Library.CloudSave
{
    public class CloudSaveOnDataTable
    {
        //CREATE and UPDATE CLOUD DATATABLE in PLAYFAB
        #region CREATE - UPDATE

        // If the specified field is not in the dataTable, the function will add this field to dataTable.
        // If it is, the specified fields will be updated by datas.
        public static void SetUserData(Dictionary<string, string> dataDictionary) // CAN BE CONFIGURED BY AUTOMATICALLY
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                //THE DATA THAT'S SEND TO CLOUD
                Data = dataDictionary,

                //DATA TABLE FIELDS Access Modifiers.
                //If you want your values are unreachable from client, make it "UserDataPermission.Private"

                Permission = UserDataPermission.Public,

            },

            result => Debug.Log("Successfully updated user data(s)"),

            error =>
            {
                Debug.Log("Got error while setting user data(s)...");

                Debug.Log(error.GenerateErrorReport());

            });
        }

        #endregion

        //REMOVE DATAs FROM PLAYFAB CLOUD DATATABLE.
        #region REMOVE

        //Remove the specified field data from dataTable.
        public static void RemoveUserData(List<string> dataList)   // CAN BE CONFIGURED BY AUTOMATICALLY
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                // Specified Field Name to remove from dataTable.
                KeysToRemove = dataList
            },

            result => Debug.Log("Successfully removed user data(s)"),

            error =>
            {
                Debug.Log("Got error while removing user data(s)...");

                Debug.Log(error.GenerateErrorReport());

            });
        }

        #endregion

        //GET DATA FROM PLAYFAB CLOUD DATATABLE
        #region GET

        //Get single user data from PLAYFAB CLOUD DATATABLE
        public static void GetSingleUserData(string dataKey, Action<string> resultCallback, Action<string> errorCallback) // CAN BE CONFIGURED BY MANUALLY
        {
            //Request PlayFab Cloud to get datas.
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                Keys = null

            }, result => {

                if (result.Data == null || !result.Data.ContainsKey(dataKey))
                {
                    //if the DataTable does not have the Key(field)

                    //Debug.Log(dataKey + " was not found ");

                    errorCallback(dataKey + " was not found.");
                }

                else  // FOUND
                {
                    //Debug.Log("Found user data:");

                    //Debug.Log(dataKey + ": " + result.Data[dataKey].Value);

                    resultCallback(result.Data[dataKey].Value.ToString()); 
                }

            }, (error) => { // ERROR CALLBACK

                //Debug.Log("Got error retrieving user data:");

                //Debug.Log(error.GenerateErrorReport());

                errorCallback("Got error retrieving user data:" + error.GenerateErrorReport().ToString());
            });

        }

        //Get all user datas from PLAYFAB CLOUD DATATABLE
        public static void GetAllUserData(Action<ArrayList> resultCallback, Action<string> errorCallback) // CAN BE CONFIGURED BY MANUALLY
        {
            //The data will store the "dataList" field's value.
            ArrayList arrayList = new ArrayList();

            //Request PlayFab Cloud to get datas.
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                Keys = null

            }, result => {

                if (result.Data != null)
                {
                    UserDataRecord[] dataList = new UserDataRecord[result.Data.Count];

                    result.Data.Values.CopyTo(dataList, 0);

                    foreach(var data in dataList)
                    {
                        arrayList.Add(data.Value);
                    }

                    resultCallback(arrayList);
                }

                else  // NOT FOUND ANY DATA
                {
                    //Debug.LogError("Player does not have any data.");

                    errorCallback("Player does not have any data.");
                }

            }, (error) => { // ERROR CALLBACK

                //Debug.Log("Got error retrieving user data:");

                //Debug.Log(error.GenerateErrorReport());

                errorCallback(error.GenerateErrorReport().ToString());

            });

        }

        //Get many user data from PLAYFAB CLOUD DATATABLE.
        public static void GetManyUserData(List<string> keyList, Action<ArrayList> resultCallback, Action<string> errorCallback)
        {
            //The data will store the "dataList" field's value.
            ArrayList arrayList = new ArrayList();

            //Request PlayFab Cloud to get datas.
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                Keys = keyList

            }, result => {

                if (result.Data == null)
                {
                    //Debug.LogError("Player does not have any data.");

                    errorCallback("Player does not have any data.");
                }

                else  // FOUND
                {
                    Debug.Log("Found datas:");

                    UserDataRecord[] returnDatas = new UserDataRecord[result.Data.Count];

                    result.Data.Values.CopyTo(returnDatas, 0);

                    foreach (var data in returnDatas)
                    {
                        arrayList.Add(data.Value);

                        //Debug.Log(data.Value);
                    }

                    resultCallback(arrayList);
                }

            }, (error) => { // ERROR CALLBACK

                //Debug.Log("Got error retrieving user data:");

                //Debug.Log(error.GenerateErrorReport());

                errorCallback(error.GenerateErrorReport().ToString());

            });

        }

        #endregion
    }
}
