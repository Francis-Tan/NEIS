using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpriteHolder : MonoBehaviour
{
    public GameObject parent;
    public void DestroyParent()
    {
        Destroy(parent);
    }
}
