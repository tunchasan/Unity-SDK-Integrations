using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using VoxelBusters.Utility;

public static class FindDuplicates
{
    /*//In Progress
	[MenuItem ("Windows/FindDuplicates")]
	public static void Find()
	{
		string			dataPath			= Application.dataPath;
		DirectoryInfo 	assetsPathDir 		= new DirectoryInfo(dataPath);

		string[] filePaths = Directory.GetFiles(dataPath, "*.aar", SearchOption.AllDirectories);

		for (int i = 0; i < filePaths.Length; i++) 
		{
			Debug.Log (filePaths [i]);
			CheckForClassName(filePaths [i], "");
		}
	}

	private static void CheckForClassName(string path, string className)
	{
		string extension = Path.GetExtension (path);

		if (extension == ".jar") 
		{
			
		}
		else if(extension == ".aar")
		{
			Zip.DecompressToDirectory (path, Application.temporaryCachePath+"/FindDuplicates");
		}
	}*/
}
