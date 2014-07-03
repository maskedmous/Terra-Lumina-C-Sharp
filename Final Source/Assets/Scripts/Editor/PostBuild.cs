using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;

public static class PostBuild
{
	[PostProcessBuild]
	static void  OnPostprocessBuild (  BuildTarget target ,   string pathToBuiltProject   )
	{
		FileUtil.CopyFileOrDirectory(Application.dataPath + "\\LevelsXML",  pathToBuiltProject.Replace(".exe", "_Data" ) + "\\LevelsXML");
		FileUtil.CopyFileOrDirectory(Application.dataPath + "\\Textures",  pathToBuiltProject.Replace(".exe", "_Data" ) + "\\Textures");
	}
}