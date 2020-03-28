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
public class CSPlayFabAsDataTable : CSPlayFabMaster
{
    //Constructor
    public CSPlayFabAsDataTable(string entityId, string entityType, string cloudMethod) : base(entityId, entityType, cloudMethod) { }

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

}