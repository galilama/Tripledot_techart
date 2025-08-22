// ToggleAnimatorRelay.cs
using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimatorRelay : MonoBehaviour
{
    public Animator animator;
    public string parameter = "On";
    Toggle toggle;

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        toggle = GetComponent<Toggle>();
    }

    void OnEnable()
    {
        // Sync visuals to current toggle state when the window (re)opens
        if (animator && toggle) animator.SetBool(parameter, toggle.isOn);
    }

    // wired to Toggle.onValueChanged (Dynamic bool)
    public void SetBool(bool value) => animator.SetBool(parameter, value);
}
