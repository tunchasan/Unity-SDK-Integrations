using PlayFab;
using PlayFab.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// The class provide many type of "Cloud Save" via PlayFab.
/// 1- Cloud Save with Files
/// It allows us to Save & Load Player's saved information that contains Maps info, Project.ini files and others...
/// 2- Cloud Save with Objects
/// It allows us to Save & Load Specific Player Info. as an object in PlayFab Cloud.
/// 3- Cloud Save with Player's Statistic
/// It allows us to Save & Load Player's Ingame Stats in PlayFab Cloud.
/// </summary>

public class CSPlayFabMaster : MonoBehaviour
{
    private string entityId; // Id representing the logged in player

    private string entityType; // entityType representing the logged in player

    //Cloud Service Types
    public enum cloudType { FILE, STATISTIC, OBJECT };

    public CSPlayFabMaster(string entityId, string entityType, string cloudMethod)
    {
        this.entityId = entityId;

        this.entityType = entityType;

        //Select Cloud Save Method
        selectCloudMethod(cloudMethod);
    }

    //The function detects which CS method will be use.
    private void selectCloudMethod(string type)
    {
        switch (type)
        {
            case "FILE":
                {
                    CSPlayFabFile CSFile = new CSPlayFabFile(entityId, entityType);   //Cloud Save with CloudFile Services

                    return;
                }

            case "STATISTIC":
                {
                    CSPlayFabStatistic CSFile = new CSPlayFabStatistic(entityId, entityType);   //Cloud Save with CloudStatistic Services

                    return;
                }

            case "OBJECT":
                {
                    CSPlayFabObject CSFile = new CSPlayFabObject();   //Cloud Save with CloudObject Services

                    return;
                }

            default:
                {
                    Debug.LogError("Something wrong with Cloud Save Type");

                    return;
                }


        }

    }

}

