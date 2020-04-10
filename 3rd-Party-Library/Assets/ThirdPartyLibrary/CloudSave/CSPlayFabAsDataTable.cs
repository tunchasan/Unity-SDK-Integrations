using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// </summary>
/// 3- Cloud Save with Data Table
/// It allows us to Save & Load Player's Ingame Stats in PlayFab Cloud.
/// </summary>
/// 

namespace Library.CloudSave
{
    public class CSPlayFabAsDataTable : CSPlayFabMaster
    {
        //CREATE and UPDATE CLOUD DATATABLE in PLAYFAB
        #region CREATE - UPDATE

        // If the specified field is not in the dataTable, the function will add this field to dataTable.
        // If it is, the specified fields will be updated by datas.
        public void SetUserData() // CAN BE CONFIGURED BY MANUALLY
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                //THE DATA THAT'S SEND TO CLOUD
                Data = new Dictionary<string, string>() {

                //DATA TABLE FIELDS// -> FILL PLACE AS YOU WANT -> EXAMPLE: {"PlayerLevel", "1"}, {"PlayerHealth", "20"}
                /***************************************************************************************************/

                {"PlayerLevel", "1"},


                {"PlayerHealth", "20"}

                /***************************************************************************************************/

        },

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

        // If the specified field is not in the dataTable, the function will add this field to dataTable.
        // If it is, the specified fields will be updated by datas.
        public void SetUserData(Dictionary<string, string> dataDictionary) // CAN BE CONFIGURED BY AUTOMATICALLY
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
        public void RemoveUserData()    // CAN BE CONFIGURED BY MANUALLY
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                // Specified Field Name to remove from dataTable.
                KeysToRemove = new List<string>(){

                //SPECIFY THE FIELDS THAT's YOU WANNA REMOVE FROM DATATABLE... -> EXAMPLE: "PlayerLevel", "PlayerHealth"
                /***************************************************************************************************/

                "PlayerLevel",

                "PlayerHealth"

                /***************************************************************************************************/
            }

            },

            result => Debug.Log("Successfully removed user data(s)"),

            error =>
            {

                Debug.Log("Got error while removing user data(s)...");

                Debug.Log(error.GenerateErrorReport());

            });
        }

        //Remove the specified field data from dataTable.
        public void RemoveUserData(List<string> dataList)   // CAN BE CONFIGURED BY AUTOMATICALLY
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
        public string GetSingleUserData(string dataKey) // CAN BE CONFIGURED BY MANUALLY
        {
            //The data will store the "dataList" field's value.
            string getData = "";

            //Request PlayFab Cloud to get datas.
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = entityId, // Id representing the logged in player

                Keys = null

            }, result => {

                if (result.Data == null || !result.Data.ContainsKey(dataKey))
                {
                    //if the DataTable does not have the Key(field)

                    Debug.Log(dataKey + " was not found ");

                    getData = "<-ERROR->"; // Assign invalid data.        
                }

                else  // FOUND
                {
                    Debug.Log("Got user data:");

                    Debug.Log(dataKey + ": " + result.Data[dataKey].Value);

                    getData = result.Data[dataKey].Value.ToString(); // Assign valid data.
                }


            }, (error) => { // ERROR CALLBACK

                Debug.Log("Got error retrieving user data:");

                Debug.Log(error.GenerateErrorReport());

                getData = "<-ERROR->"; // Assign invalid data.

            });

            return getData;
        }

        //Get many user data from PLAYFAB CLOUD DATATABLE.
        public List<string> GetUserDatas(List<string> dataList)
        {
            //The list will store the "dataList" field's values.
            List<string> getDataList = new List<string>();

            foreach (string elem in dataList)
            {
                getDataList.Add(GetSingleUserData(elem)); //get single field
            }

            return getDataList;
        }

        #endregion
    }
}
