// Assets/Editor/TextureClampPostprocessor.cs
using UnityEngine;
using UnityEditor;

class TextureClampPostprocessor : AssetPostprocessor
{
    const string kInitFlagUserData = "ClampInitialized";
    const string kInitFlagLabel    = "ClampInitialized"; // hidden-ish; just don't use it yourself

    void OnPreprocessTexture()
    {
        var texImporter = (TextureImporter)assetImporter;
        var settings = TextureClampSettings.GetOrDefaults();

        // Opt-out with a label like "NoClamp"
        foreach (var label in AssetDatabase.GetLabels(assetImporter))
        {
            if (label == settings.optOutLabel)
                return;
        }

        // If we already initialized this texture once, do nothing.
        if (IsInitialized_PreImport(texImporter))
            return;

        // First-time clamp (or meta copied from elsewhere but not marked initialized)
        int target = Mathf.Max(32, settings.maxSize);   // defaults to 128 unless you changed it
        texImporter.maxTextureSize = target;

        // Mark via userData now (postprocess will also add a label)
        AppendUserDataFlag(texImporter, kInitFlagUserData);
    }

    // After Unity imports the texture, we can reliably add an asset label too.
    void OnPostprocessTexture(Texture2D texture)
    {
        // If already labeled, nothing to do
        if (HasInitLabel(assetPath))
            return;

        var obj = AssetDatabase.LoadMainAssetAtPath(assetPath);
        if (obj == null) return;

        var existing = AssetDatabase.GetLabels(obj);
        // add kInitFlagLabel if not present
        bool has = false;
        for (int i = 0; i < existing.Length; i++)
        {
            if (existing[i] == kInitFlagLabel) { has = true; break; }
        }
        if (!has)
        {
            var newLabels = new string[existing.Length + 1];
            existing.CopyTo(newLabels, 0);
            newLabels[newLabels.Length - 1] = kInitFlagLabel;
            AssetDatabase.SetLabels(obj, newLabels);
        }
    }

    // --- helpers ---

    static bool IsInitialized_PreImport(TextureImporter importer)
    {
        // 1) Check userData flag
        if (!string.IsNullOrEmpty(importer.userData) && importer.userData.Contains(kInitFlagUserData))
            return true;

        // 2) Check label if possible (label exists if asset has been imported before)
        return HasInitLabel(importer.assetPath);
    }

    static bool HasInitLabel(string path)
    {
        var obj = AssetDatabase.LoadMainAssetAtPath(path);
        if (obj == null) return false;
        var labels = AssetDatabase.GetLabels(obj);
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i] == kInitFlagLabel)
                return true;
        }
        return false;
    }

    static void AppendUserDataFlag(AssetImporter importer, string flag)
    {
        string ud = importer.userData ?? string.Empty;
        if (!ud.Contains(flag))
            importer.userData = string.IsNullOrEmpty(ud) ? flag : (ud + ";" + flag);
    }
}
