using UnityEngine;
using Unity.Netcode;
public class PlayerColor : NetworkBehaviour
{
    [SerializeField] private MeshRenderer tankRenderer;
    [SerializeField] Material[] tankMaterials;
    [SerializeField] GetValuesFromDropdown dropdownValue;

    public NetworkVariable<int> selectedColorIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //set initial material based on synced index
        ApplyMaterial(selectedColorIndex.Value);
        //subscribe to changes
        selectedColorIndex.OnValueChanged += OnMaterialChanged;

        if (IsOwner)
        {
            dropdownValue = FindObjectOfType<GetValuesFromDropdown>();

            if (dropdownValue == null)
            {
                Debug.LogError("GetValuesFromDropdown not found in the scene!");
                return;
            }
            dropdownValue.OnDropdownValueChanged.AddListener(ChangeMaterial);
        }
    }

    private void OnDestroy()
    {
        selectedColorIndex.OnValueChanged -= OnMaterialChanged;
        
        if (IsOwner && dropdownValue != null)
        {
            dropdownValue.OnDropdownValueChanged.RemoveListener(ChangeMaterial);
        }
    }

    private void OnMaterialChanged(int oldIndex, int newIndex)
    {
        ApplyMaterial(newIndex);
    }

    private void ApplyMaterial(int index)
    {
        if (index >= 0 && index < tankMaterials.Length)
        {
            tankRenderer.material = tankMaterials[index];
        }
    }

    //call from ui
    public void ChangeMaterial(int newIndex)
    {
        if (!IsOwner) return;

        SubmitMaterialChangeRequestRPC(newIndex);
    }

    [Rpc(SendTo.Server)]
    private void SubmitMaterialChangeRequestRPC(int newIndex)
    {
        if (!IsServer) return;

        if (newIndex >= 0 && newIndex < tankMaterials.Length)
        {
            Debug.Log($"Server received material change request: {newIndex}");

            selectedColorIndex.Value = newIndex;
        }
    }
}