using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;

    [SerializeField] private Transform mouseVisual;
    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Vector3 pos = GetPostion();

        if (pos != Vector3.zero)
        {
            mouseVisual.gameObject.SetActive(true);
            mouseVisual.position = pos;
        }
        else
        {
            mouseVisual.gameObject.SetActive(false);    
        }
    }

    public static Vector3 GetPostion (){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, instance.mousePlaneLayerMask);
        return hitInfo.point;
    }
}

