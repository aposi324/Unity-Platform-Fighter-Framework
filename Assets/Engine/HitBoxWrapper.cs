using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxWrapper : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public HitBox hitData;

    private void Start()
    {
        hitData = new HitBox(45, 1f, 1f, 1f, 1f);
        //hitBox = new HitBox(HelperFunctions.CorrectAngle, 1f, 1f, 1f, 1f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }
}
