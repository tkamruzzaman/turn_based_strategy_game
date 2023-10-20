using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        Hide();
    }

    public void Show() => meshRenderer.enabled = true;

    public void Hide() => meshRenderer.enabled = false;
}
