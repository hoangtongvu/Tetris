using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

static class USSUtils
{
    public static StyleSheet LoadStyleSheet(string relativeLocation)
    {
        var dir = GetScriptDirectory();
        var ussPath = Path.Combine(dir, relativeLocation);
        var relativePath = "Assets" + ussPath.Substring(Application.dataPath.Length);

        return AssetDatabase.LoadAssetAtPath<StyleSheet>(relativePath);
    }

    private static string GetScriptDirectory([CallerFilePath] string path = "")
        => Path.GetDirectoryName(path);
}