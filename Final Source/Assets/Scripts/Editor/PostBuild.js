#pragma strict
import UnityEditor.Callbacks;
@PostProcessBuild
static function OnPostprocessBuild( target : BuildTarget, pathToBuiltProject : String ) {
FileUtil.CopyFileOrDirectory(Application.dataPath + "\\LevelsXML",  pathToBuiltProject.Replace(".exe", "_Data" ) + "\\LevelsXML");
//FileUtil.CopyFileOrDirectory(Application.dataPath + "\\SoundEffects",  pathToBuiltProject.Replace(".exe", "_Data" ) + "\\SoundEffects");
FileUtil.CopyFileOrDirectory(Application.dataPath + "\\Textures",  pathToBuiltProject.Replace(".exe", "_Data" ) + "\\Textures");
}