// Assets/Editor/TextureClampSettings.cs
using UnityEngine;
using UnityEditor;

public class TextureClampSettings : ScriptableObject
{
    [Min(32)] public int maxSize = 128;
    public string optOutLabel = "NoClamp";

    public const string AssetPath = "Assets/Editor/TextureClampSettings.asset";
    const string FolderPath = "Assets/Editor";

    /// Try to load the saved settings asset. Returns null if not present.
    public static TextureClampSettings TryLoad()
    {
        return AssetDatabase.LoadAssetAtPath<TextureClampSettings>(AssetPath);
    }

    /// Returns existing settings if present; otherwise a TEMP instance with defaults (not saved).
    public static TextureClampSettings GetOrDefaults()
    {
        var s = TryLoad();
        if (s != null) return s;

        var temp = CreateInstance<TextureClampSettings>();
        temp.hideFlags = HideFlags.HideAndDontSave; // clearly not an asset
        return temp;
    }

    [MenuItem("Tools/Texture Clamp Settings")]
    public static void CreateOrSelect()
    {
        // Ensure folder exists
        if (!AssetDatabase.IsValidFolder(FolderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Editor");
        }

        var s = TryLoad();
        if (s == null)
        {
            s = CreateInstance<TextureClampSettings>();
            AssetDatabase.CreateAsset(s, AssetPath);
            AssetDatabase.SaveAssets();
            Debug.Log("Created " + AssetPath + " (default Max Size = 128).");
        }
        Selection.activeObject = s;
        EditorGUIUtility.PingObject(s);
    }

    [MenuItem("Tools/Reimport All Textures With Clamp")]
    public static void ReimportAll()
    {
        var guids = AssetDatabase.FindAssets("t:Texture");
        for (int i = 0; i < guids.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            EditorUtility.DisplayProgressBar("Reimporting Textures", path, (float)i / guids.Length);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("Reimport complete.");
    }
}
