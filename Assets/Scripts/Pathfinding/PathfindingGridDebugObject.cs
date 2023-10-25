using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TMP_Text gCostText;
    [SerializeField] private TMP_Text hCostText;
    [SerializeField] private TMP_Text fCostText;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;
    [Space]
    [SerializeField] private bool isToShowWalkableArea;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = gridObject as PathNode;
        isWalkableSpriteRenderer.gameObject.SetActive(isToShowWalkableArea);
    }

    protected override void Update()
    {
        base.Update();

        gCostText.text = pathNode.GetGCost() == int.MaxValue?  "G": pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost() == int.MaxValue? "H" : pathNode.GetHCost().ToString();    
        fCostText.text = pathNode.GetFCost() == int.MaxValue ? "F" : pathNode.GetFCost().ToString();

        isWalkableSpriteRenderer.color = pathNode.GetIsWalkable()? Color.green: Color.red;
    }
}
