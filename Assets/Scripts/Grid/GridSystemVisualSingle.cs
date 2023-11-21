//#define TESTING
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject selectedGameObject;

    private void Start()
    {
        Hide();
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

#if TESTING
    public void ShowSelected()=> selectedGameObject.SetActive(true);

    public void HideSelected()=> selectedGameObject.SetActive(false);
#endif

}