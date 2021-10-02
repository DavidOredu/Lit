using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int dotsNumber;
    [SerializeField] GameObject dotsParent;
    [SerializeField] GameObject dotsPrefab;
    [SerializeField] float dotsSpacing;
    [SerializeField][Range(0.01f, 0.3f)] float dotMinScale;
    [SerializeField] [Range(0.3f, 1f)] float dotMaxScale;

    Vector2 pos;
    float timeStamp;
    Transform[] dotsList;

    // Start is called before the first frame update
    void Start()
    {
        HideTrejectory();
        PrepareDots();
    }
    public void ShowTrejectory()
    {
        dotsParent.SetActive(true);
    }
    void PrepareDots()
    {
        dotsList = new Transform[dotsNumber];
        dotsPrefab.transform.localScale = Vector3.one * dotMaxScale;

        float scale = dotMaxScale;
        float scaleFactor = scale / dotsNumber;
        for (int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(dotsPrefab, null).transform;
            dotsList[i].parent = dotsParent.transform;

            dotsList[i].localScale = Vector3.one * scale;
            if (scale > dotMinScale)
                scale -= scaleFactor;
        }
    }
    public void UpdateDots(Vector3 pos, Vector2 force)
    {
        timeStamp = dotsSpacing;

        for (int i = 0; i < dotsNumber; i++)
        {
            this.pos.x = (pos.x + force.x * timeStamp);
            this.pos.y = (pos.y + force.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            dotsList[i].position = this.pos;
            timeStamp += dotsSpacing;
        }
    } 
    public void HideTrejectory()
    {
        dotsParent.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
