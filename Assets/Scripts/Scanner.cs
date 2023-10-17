using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearsetTarget;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        nearsetTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;

        float diff = 100;

        foreach (var target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDif = Vector3.Distance(myPos, targetPos);

            if (curDif < diff)
            {
                diff = curDif;
                result = target.transform;
            }
        }

        return result;
    }

}
