using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Color color = Color.rebeccaPurple;
    public float lifespan = 0.1f;
    public float width = 0.5f;
    private LineRenderer render;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        render = GetComponent<LineRenderer>();

        render.startColor = color;
        render.endColor = color;

        render.startWidth = width;
        render.endWidth = width;

        Vector3[] points = {startPoint, endPoint};
        render.SetPositions(points);

        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
