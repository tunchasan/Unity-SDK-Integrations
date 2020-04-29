using Library.CloudSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSaveExample : MonoBehaviour
{
    /*******************************************************************************************************************/

    public void GetSingleUserData() // Get Single User Data 
    {
        CloudSaveOnDataTable.GetSingleUserData("coin", singleUserDataSuccessCallback, singleUserDataErrorCallback);
    }

    private void singleUserDataErrorCallback(string error) // Get Single User Data error Callback
    {
        Debug.Log(error);
    }

    private void singleUserDataSuccessCallback(string success) // Get Single User Data Success Callback
    {
        Debug.Log(success);
    }

    /*******************************************************************************************************************/

    public void GetManyUserData() // Get Many User Data 
    {
        List<string> keyList = new List<string>() { "health", "coin", "gem" };

        CloudSaveOnDataTable.GetManyUserData(keyList, manyUserDataSuccessCallback, manyUserDataErrorCallback);
    }

    private void manyUserDataErrorCallback(string error)
    {
        Debug.Log(error);
    }

    private void manyUserDataSuccessCallback(ArrayList result)
    {
        foreach (var data in result)
        {
            Debug.Log(data);
        }
    }

    /*******************************************************************************************************************/

    public void GetAllUserData() // Get All User Data 
    {
        CloudSaveOnDataTable.GetAllUserData(allUserDataSuccessCallback, allUserDataErrorCallback);
    }

    private void allUserDataErrorCallback(string error)
    {
        Debug.Log(error);
    }

    private void allUserDataSuccessCallback(ArrayList result)
    {
        foreach (var data in result)
        {
            Debug.Log(data);
        }
    }

    /*******************************************************************************************************************/

    public void RemoveUserData() // Remove User Data
    {
        CloudSaveOnDataTable.RemoveUserData(new List<string>() { "coin" });
    }

    /*******************************************************************************************************************/

    public void AddUserNewData() // Add New User Data
    {
        Dictionary<string, string> datas = new Dictionary<string, string>();

        datas.Add("floor","5");

        CloudSaveOnDataTable.SetUserData(datas);
    }

    /*******************************************************************************************************************/

    void Update()
    {
        if (Input.GetKey("w")) // Get Single User Data
        {
            GetSingleUserData();
        }

        else if (Input.GetKey("s")) // Get Many User Data
        {
            GetManyUserData();
        }

        else if (Input.GetKey("d")) // Get All Data
        {
            GetAllUserData();
        }

        else if (Input.GetKey("a")) // Remove User Data
        {
            RemoveUserData();
        }

        else if (Input.GetKey("e")) // Add User New Data
        {
            AddUserNewData();
        }
    }
}
