using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class StringEvent : UnityEvent<string> {}

public class BottomBarView : MonoBehaviour
{
    [Header("Assign your 5 nav toggles (scene instances)")]
    public List<Toggle> toggles = new List<Toggle>();

    [Header("Events required by assignment")]
    public StringEvent ContentActivated;   // passes the selected button's id/name
    public UnityEvent Closed;              // fired when all toggles are off

    ToggleGroup group;

    void Awake()
    {
        // Ensure a ToggleGroup so only one can be on (but allow none).
        group = GetComponent<ToggleGroup>();
        if (!group) group = gameObject.AddComponent<ToggleGroup>();
        group.allowSwitchOff = true;

        foreach (var t in toggles)
            if (t) t.group = group;
    }

    void OnEnable()
    {
        foreach (var t in toggles)
            if (t) t.onValueChanged.AddListener(OnAnyChanged);

        // Sync animators & fire initial state on enable
        SyncAnimators();
        FireEvents();
    }

    void OnDisable()
    {
        foreach (var t in toggles)
            if (t) t.onValueChanged.RemoveListener(OnAnyChanged);
    }

    void OnAnyChanged(bool _)
    {
        SyncAnimators();
        FireEvents();
    }

    void SyncAnimators()
    {
        // Drive each button's Animator bool "On" from its Toggle.isOn
        foreach (var t in toggles)
        {
            if (!t) continue;
            var anim = t.GetComponent<Animator>();
            if (anim) anim.SetBool("On", t.isOn);
        }
    }

    void FireEvents()
    {
        // Find first ON toggle; if none, fire Closed
        foreach (var t in toggles)
        {
            if (t && t.isOn)
            {
                ContentActivated?.Invoke(t.gameObject.name); // or your own ID
                return;
            }
        }
        Closed?.Invoke();
    }
}
