using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SPUMEditor : Editor
{
    [MenuItem("SPUM Editor/Resolution SPUM Prefab Material")]
    private static void ResolutionSPUMPrefabMaterial()
    {
        // Load Main Material
        Material spriteDiffuseMaterial = AssetDatabase.LoadAssetAtPath("Assets/SPUM/Basic_Resources/Materials/SpriteDiffuse.mat", typeof(Material)) as Material;

        // Load SPUM Character Prefabs
        string folderPath = "Assets/Resources/Prefabs/Monsters/";
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        for (int i = 0; i < prefabGUIDs.Length; i++)
        {
            string prefabGUID = prefabGUIDs[i];
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // Update SpriteRenderer Material
            SpriteRenderer[] spriteRenderers = go.GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < spriteRenderers.Length; j++)
            {
                SpriteRenderer spriteRenderer = spriteRenderers[j];
                spriteRenderer.material = spriteDiffuseMaterial;
            }
        }

        // Save
        AssetDatabase.SaveAssets();

        Debug.Log("Resolution SPUM Prefab Material!");
    }
}

public class MonsterEditor : Editor
{
    [MenuItem("SPUM Editor/Monster Get SpriteRenderers")]
    private static void ResolutionSPUMPrefabMaterial()
    {
        // Load SPUM Character Prefabs
        string folderPath = "Assets/Resources/Prefabs/Monsters/";
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        for (int i = 0; i < prefabGUIDs.Length; i++)
        {
            string prefabGUID = prefabGUIDs[i];
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (go.GetComponent<MonsterFSMController>() != null)
                go.GetComponent<MonsterFSMController>().SetSpriteRenderer();
        }

        // Save
        AssetDatabase.SaveAssets();

        Debug.Log("Resolution SPUM Prefab Material!");
    }
}