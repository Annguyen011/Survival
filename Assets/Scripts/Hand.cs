using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer sprite;

    SpriteRenderer player;
    private Vector3 rightPos = new Vector3 (0.35f, -.15f, 0);
    private Vector3 leftPos = new Vector3 (-.15f, -.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35f);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135f);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];    
    }

    private void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft)
        {
            transform.localRotation = !isReverse ? leftRot : leftRotReverse;
            sprite.flipY = isReverse;
            sprite.sortingOrder = !isReverse? 1 : -1;
        }
        else
        {
            transform.localPosition = !isReverse?rightPos : leftPos;
            sprite.flipX = isReverse;
            sprite.sortingOrder = isReverse ? 1 : -1;
        }
    }
}
