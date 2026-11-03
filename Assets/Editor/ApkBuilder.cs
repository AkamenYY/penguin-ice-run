using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// ИНСТРУМЕНТ: собирает APK для Android со всеми сценами из Build Settings.
// Меню: Tools → Build APK. Файл появится в папке проекта: Builds/PenguinIceRun.apk
public static class ApkBuilder
{
    [MenuItem("Tools/Build APK")]
    public static void Build()
    {
        PlayerSettings.SetApplicationIdentifier(UnityEditor.Build.NamedBuildTarget.Android, "com.madok.penguinice");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

        Directory.CreateDirectory("Builds");
        string path = Path.GetFullPath("Builds/PenguinIceRun.apk");
        BuildPlayerOptions opt = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = path,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
        BuildReport r = BuildPipeline.BuildPlayer(opt);
        Debug.Log("APK BUILD: " + r.summary.result + " -> " + path + " errors=" + r.summary.totalErrors);
    }
}
