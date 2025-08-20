using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class BottomBarView : MonoBehaviour
{
    [System.Serializable]
    public struct NavItem
    {
        public Toggle toggle;     // assign the Toggle
        public string contentId;  // e.g. "Home", "Shop", "Map", "Locked1", "Locked2"
    }

    public ToggleGroup toggleGroup;
    public NavItem[] items;

    [System.Serializable] public class ContentActivatedEvent : UnityEvent<string> {}
    public ContentActivatedEvent ContentActivated; // fires with the contentId string
    public UnityEvent Closed;                      // fires when all toggles are off

    void Awake()
    {
        // keep whatever you set in the inspector (allow switch off or not)
        foreach (var it in items)
        {
            var captured = it;
            if (!captured.toggle) continue;

            captured.toggle.group = toggleGroup;
            captured.toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn) ContentActivated.Invoke(captured.contentId);
                else if (AllOff()) Closed.Invoke();
            });
        }

        // initial fire
        if (AllOff()) Closed.Invoke();
        else
        {
            var on = items.FirstOrDefault(i => i.toggle && i.toggle.isOn);
            if (on.toggle) ContentActivated.Invoke(on.contentId);
        }
    }

    bool AllOff() => items.All(i => !i.toggle || !i.toggle.isOn);
}
