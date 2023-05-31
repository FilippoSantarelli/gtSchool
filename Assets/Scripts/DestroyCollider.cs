using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static SelectionController;

public class DestroyCollider : MonoBehaviour
{
    public delegate void BlockCollided(GameObject collidingBlock);
    public static event BlockCollided OnBlockCollision;

    public void OnTriggerEnter(Collider other)
    {
        JengaBlock fallingBlock = other.GetComponent<JengaBlock>();
        if (fallingBlock != null)
        {
            if (OnBlockCollision != null)
            {
                OnBlockCollision(other.gameObject);
            }
        }
    }
}

