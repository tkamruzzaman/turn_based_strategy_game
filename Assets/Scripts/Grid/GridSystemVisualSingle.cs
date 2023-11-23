using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer selectedMeshRenderer;
    [Space]
    [SerializeField] private Material squareMaterial;
    [SerializeField] private Material hexagonalMaterial;


    private void Start()
    {
        Hide();
        HideSelected();
        selectedMeshRenderer.material = LevelGrid.Instance.GridType == GridType.Square ? squareMaterial : hexagonalMaterial;
    }

    public void Show(Material material)
    {
        meshRenderer.material = material;
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }

    public void ShowSelected() => selectedMeshRenderer.gameObject.SetActive(true);

    public void HideSelected() => selectedMeshRenderer.gameObject.SetActive(false);

}