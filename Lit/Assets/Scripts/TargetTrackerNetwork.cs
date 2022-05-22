using UnityEngine;
using UnityEngine.UI;

public class TargetTrackerNetwork : MonoBehaviour
{
    public StickmanNet stickman;
    public Racer player;
    [SerializeField] private RectTransform startPos;
    [SerializeField] private RectTransform endPos;
    [SerializeField] private Image image;

    private float mainPos = 0;
    private float startingPos;
    private Vector3 posOffset;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position.x;
        posOffset = endPos.position - startPos.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null || stickman == null) { return; }
        mainPos = ((player.currentPositionPercentage / 100) * posOffset.x) + startingPos;
        transform.position = new Vector3(mainPos, transform.position.y);
        image.material = Resources.Load<Material>($"{stickman.currentColor.colorID}");
    }
}
