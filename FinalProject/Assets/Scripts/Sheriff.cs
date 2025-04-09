//using System.Numerics;
using UnityEngine;

public class Sheriffs : Enemy
{

    [Header("Sheriff Private Fields")]
    [SerializeField] private Vector3 p0;
    [SerializeField] private float frequency = 1f; // speed of oscillation
    [SerializeField] private float amplitude = 2f; // range of up/down movement
    [SerializeField] private float verticalSpeed = 2f;
private int direction = 1;
    private float timeStart;

    void Start()
    {
        p0 = Vector3.zero;
        p0.x = bndCheck.camWidth + bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.TopScreen);
    }
    public override void Move()
    {

        // Vector3 pos = p0;
        // float t = Time.time - timeStart;
        // pos.y += Mathf.Sin(t * frequency) * amplitude; // up/down oscillation
        // transform.position = pos;

        Vector3 pos = transform.position;
    pos.y += verticalSpeed * direction * Time.deltaTime;
    transform.position = pos;

    if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp))
    {
        direction = -1;
    }
    else if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown))
    {
        direction = 1;
    }
    }
}
