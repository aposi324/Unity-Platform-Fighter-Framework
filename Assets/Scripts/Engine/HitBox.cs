using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox2 : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }
}
