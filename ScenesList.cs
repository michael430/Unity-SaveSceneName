
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class ScenesList : ScriptableObject
{
    public string[] scenesNames;
}

#if UNITY_EDITOR
public class ScenesListEditor : Editor
{

    // ref:  https://davikingcode.com/blog/retrieving-the-names-of-your-scenes-at-runtime-with-unity/
    // Note: the array index should be match with scene build index in built setting.
    [MenuItem("House of Secrets/Save Scenes Names")]
    private static void SaveScenesNames()
    {
        // Get the scenes list form the built setting
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        // First, try to load the list if already exists
        ScenesList list = (ScenesList)AssetDatabase.LoadAssetAtPath("Assets/Resources/ScenesList.asset", typeof(ScenesList));

        // Delete the old Asset file
        if (list)
            DestroyImmediate(list, true);

        //if (list == null)
        {
            list = CreateInstance<ScenesList>();
            // Make sure we have Resources folder already, Otherwise this may throw a error. Or we need to add the folder check codes.
            AssetDatabase.CreateAsset(list, "Assets/Resources/ScenesList.asset");
        }

        List<string> sceneList = new List<string>();
        // Store the list of scene names
        for (int i = 0; i < scenes.Length; ++i)
        {
            // only save the enabled scene
            if (!scenes[i].enabled) continue;

            sceneList.Add(System.IO.Path.GetFileNameWithoutExtension(scenes[i].path));
        }

        list.scenesNames = new string[sceneList.Count];

        // Apply to the Array of the ScriptableObject
        for (int i = 0; i < sceneList.Count; ++i)
        {
            list.scenesNames[i] = sceneList.ToArray()[i];
        }

        EditorUtility.SetDirty(list);

        // Writes to disk
        AssetDatabase.SaveAssets();

        // Selects the Asset file
        Selection.activeObject = list;
    }
}
#endif
