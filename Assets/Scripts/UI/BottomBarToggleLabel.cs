using UnityEngine;
using UnityEngine.UI;
using TMPro; // only if you use TextMeshPro

[RequireComponent(typeof(Toggle))]
public class BottomBarToggleLabel : MonoBehaviour
{
    public GameObject label; // drag the Label child here

    Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        if (label) label.SetActive(_toggle.isOn);
        _toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        if (label) label.SetActive(isOn);
    }
}
