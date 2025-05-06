using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GetValuesFromDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public UnityEvent<int> OnDropdownValueChanged;

    private void Awake()
    {
        dropdown.onValueChanged.AddListener(HandleDropdownChange);
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(HandleDropdownChange);
    }

    private void HandleDropdownChange(int index)
    {
        Debug.Log($"Selected dropdown index: {index}");
        OnDropdownValueChanged?.Invoke(index);
    }

    public int GetDropdownValue()
    {
        return dropdown.value;
    }
}

