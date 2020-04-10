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

namespace Library.CloudSave
{
    public class CSPlayFabMaster : MonoBehaviour
    {
        protected string entityId; // Id representing the logged in player

        protected string entityType; // entityType representing the logged in player

        protected string cloudMethod;

        //Cloud Service Types
        public enum cloudType { FILE, DATATABLE, OBJECT };

        //Constructor
        public CSPlayFabMaster(string entityId, string entityType, string cloudMethod)
        {
            this.entityId = entityId;

            this.entityType = entityType;

            this.cloudMethod = cloudMethod;

            //Select Cloud Save Method
            selectCloudMethod(cloudMethod);
        }

        //Empty Constructor.
        public CSPlayFabMaster() { }

        //The function detects which CS method will be use.
        private void selectCloudMethod(string type)
        {
            switch (type)
            {
                case "FILE":
                    {
                        new CSPlayFabAsFile(entityId, entityType);   //Cloud Save with CloudFile Services

                        return;
                    }

                case "DATATABLE":
                    {
                        new CSPlayFabAsDataTable();   //Cloud Save with CloudDataTable Services

                        Debug.Log("{ Cloud Player Statistic } Service is Initializing...{ " + entityId + " <---> " + entityType + " }");

                        return;
                    }

                case "OBJECT":
                    {
                        new CSPlayFabAsObject();   //Cloud Save with CloudObject Services

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

}



