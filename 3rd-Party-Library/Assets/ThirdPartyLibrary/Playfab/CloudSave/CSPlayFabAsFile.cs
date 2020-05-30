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
/// </summary>
/// 

namespace Library.CloudSave
{
    public class CSPlayFabAsFile : MonoBehaviour
    {
        public string entityId; // Id representing the logged in player

        public string entityType; // entityType representing the logged in player

        public string ActiveUploadFileName;

        public string NewFileName;

        private readonly Dictionary<string, string> _entityFileJson = new Dictionary<string, string>();

        private readonly Dictionary<string, string> _tempUpdates = new Dictionary<string, string>();

        // GlobalFileLock provides is a simplistic way to avoid file collisions, specifically designed for this example.
        public int GlobalFileLock = 0;

        public CSPlayFabAsFile(string Id, string type)
        {
            entityId = Id;

            entityType = type;

            Debug.Log(Id + " -**- " + type);

            LoadAllFiles();
        }

        public void LoadAllFiles()
        {
            if (GlobalFileLock != 0)
                throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");

            GlobalFileLock += 1; // Start GetFiles

            var request = new PlayFab.DataModels.GetFilesRequest { Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType } };

            PlayFabDataAPI.GetFiles(request, OnGetFileMeta, OnSharedFailure);
        }

        public void OnGetFileMeta(PlayFab.DataModels.GetFilesResponse result)
        {
            Debug.Log("Loading " + result.Metadata.Count + " files");

            _entityFileJson.Clear();

            foreach (var eachFilePair in result.Metadata)
            {
                _entityFileJson.Add(eachFilePair.Key, null);

                GetActualFile(eachFilePair.Value);
            }

            GlobalFileLock -= 1; // Finish GetFiles
        }

        public void OnSharedFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());

            GlobalFileLock -= 1;
        }

        public void GetActualFile(PlayFab.DataModels.GetFileMetadata fileData)
        {
            GlobalFileLock += 1; // Start Each SimpleGetCall

            PlayFabHttp.SimpleGetCall(fileData.DownloadUrl,

                result => {

                    _entityFileJson[fileData.FileName] = Encoding.UTF8.GetString(result); GlobalFileLock -= 1;

                    Debug.LogWarning(fileData.FileName);

                    Debug.LogWarning(fileData.Size);

                }, // Finish Each SimpleGetCall

                error => { Debug.Log(error); }
            );
        }
    }
}


