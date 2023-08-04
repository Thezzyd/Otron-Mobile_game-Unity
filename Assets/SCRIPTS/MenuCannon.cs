using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCannon : MonoBehaviour
{
    public bool right;
    public RaycastHit2D hit;

    public Transform rayCastStartPoint;
    public Transform laserTransform;
    public ParticleSystem laserHitEffect;

    void FixedUpdate()
    {

        if (!right)
        {
            transform.Rotate(Vector3.forward * 0.4f);
        }

        else
        {
            transform.Rotate(-Vector3.forward * 0.4f);
        }
        

        if (transform.eulerAngles.z > 180)
            right = true;
        if (transform.eulerAngles.z < 0)
            right = false;


        Vector3 forwardVel = transform.forward;
        Vector3 horizontalVel = transform.right;

        hit = Physics2D.Raycast(rayCastStartPoint.position, (forwardVel + horizontalVel) * 5f, 13.672f);
        float laserScaleY = hit.distance / 3.418f;
        if (hit.collider)
        {
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserScaleY, 1);
            laserHitEffect.Emit(3);
        }
        else
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, 4f, 1f);

    }
}
