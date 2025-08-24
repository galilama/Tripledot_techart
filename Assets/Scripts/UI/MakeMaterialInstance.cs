using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class AnimateDissolveUIButton : MonoBehaviour
{
    [Header("Shader property reference names (from Shader Graph)")]
    public string dissolveAmountProp   = "_DissolveAmount";
    public string outlineThicknessProp = "_OutlineThickness";
    public string dissolveScaleProp    = "_DissolveScale";
    public string outlineColorProp     = "_OutlineColor";
    public string spiralProp           = "_Spiral";

    [Header("Animatable values")]
    [Range(0f, 1f)] public float dissolveAmount = 0f;
    [Range(0f, 2f)] public float outlineThickness = 0.44f;
    public float dissolveScale = 17f;
    public Color outlineColor = Color.cyan;    // HDR ok
    public float spiral = 5f;

    Image img;

    // cached property IDs
    int idDissolveAmount, idOutlineThickness, idDissolveScale, idOutlineColor, idSpiral;

    void OnEnable()
    {
        img = GetComponent<Image>();
        CacheIDs();

        // ensure this object has its own material instance (so animating doesn't change others)
        if (img != null && img.material != null && !img.material.name.EndsWith(" (Instance)"))
            img.material = new Material(img.material) { name = img.material.name + " (Instance)" };

        Apply();
    }

    void OnValidate()
    {
        CacheIDs();
        Apply();
    }

    void Update() => Apply();

    void CacheIDs()
    {
        idDissolveAmount   = string.IsNullOrEmpty(dissolveAmountProp)   ? 0 : Shader.PropertyToID(dissolveAmountProp);
        idOutlineThickness = string.IsNullOrEmpty(outlineThicknessProp) ? 0 : Shader.PropertyToID(outlineThicknessProp);
        idDissolveScale    = string.IsNullOrEmpty(dissolveScaleProp)    ? 0 : Shader.PropertyToID(dissolveScaleProp);
        idOutlineColor     = string.IsNullOrEmpty(outlineColorProp)     ? 0 : Shader.PropertyToID(outlineColorProp);
        idSpiral           = string.IsNullOrEmpty(spiralProp)           ? 0 : Shader.PropertyToID(spiralProp);
    }

    void Apply()
    {
        if (img == null || img.material == null) return;

        if (idDissolveAmount   != 0) img.material.SetFloat(idDissolveAmount,   dissolveAmount);
        if (idOutlineThickness != 0) img.material.SetFloat(idOutlineThickness, outlineThickness);
        if (idDissolveScale    != 0) img.material.SetFloat(idDissolveScale,    dissolveScale);
        if (idOutlineColor     != 0) img.material.SetColor(idOutlineColor,     outlineColor);
        if (idSpiral           != 0) img.material.SetFloat(idSpiral,           spiral);
    }
}
