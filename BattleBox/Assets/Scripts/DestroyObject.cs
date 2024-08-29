using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    // Update is called once per frame
    void Update()
    {
        DestroyGameObject();
    }

    void DestroyGameObject()
    {
        Destroy(gameObject, destroyTime);
    }
}
