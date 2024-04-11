using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public bool draw = true;

    private void OnDrawGizmos()
    {
        if(draw)
        {
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
