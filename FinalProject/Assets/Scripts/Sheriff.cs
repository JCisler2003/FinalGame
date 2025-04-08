//using System.Numerics;
using UnityEngine;

public class Sheriffs : Enemy
{

    [Header("Sheriff Private Fields")]
    [SerializeField] private Vector3 p0;
    [SerializeField] private float frequency = 1f; // speed of oscillation
    [SerializeField] private float amplitude = 2f; // range of up/down movement
    private float timeStart;

    void Start()
    {
        p0 = Vector3.zero;
        p0.x = bndCheck.camWidth + bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
    }
    public override void Move()
    {
        Vector3 pos = p0;
        float t = Time.time - timeStart;
        pos.y += Mathf.Sin(t * frequency) * amplitude; // up/down oscillation
        transform.position = pos;
    }
}
