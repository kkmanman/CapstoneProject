using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedSpikeBall : MonoBehaviour
{
    public float force = 10f;
    public float maxAngle = 45f;

    private HingeJoint2D hinge;
    private Rigidbody2D rb;

    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float angle = Mathf.Clamp(hinge.jointAngle, -maxAngle, maxAngle);
        float forceAngle = Mathf.Lerp(-maxAngle, maxAngle, (angle + maxAngle) / (2 * maxAngle));
        float torque = force * Mathf.Sin(forceAngle * Mathf.Deg2Rad);
        rb.AddTorque(torque);
    }
}
