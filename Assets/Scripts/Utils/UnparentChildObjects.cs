using System.Collections.Generic;
using UnityEngine;

public class UnparentChildObjects : MonoBehaviour
{
    [SerializeField] private bool isToUnparent;
    [Space]
    [SerializeField] private List< Transform> childrenTransformList;

    [ContextMenu("AssignChildren")]
    private void Reset()
    {
        childrenTransformList.Clear();

        foreach (Transform child in transform)
        {
            childrenTransformList.Add(child);
        }
    }

    private void Awake()
    {
        if(!isToUnparent) { return; }

        foreach(Transform child in childrenTransformList)
        {
            child.SetParent(null);
        }

        Destroy(gameObject);
    }
}
