using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private object gridObject;

    public virtual void SetGridObject(object gridObject) => this.gridObject = gridObject;

    public object GetGridObject() => gridObject;

    protected virtual void Update() => text.text = gridObject.ToString();

}
