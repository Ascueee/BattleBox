using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParentObject : MonoBehaviour
{
    bool destroyObject = false;
    [SerializeField] GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyObject == true)
        {
            Destroy(parent);
        }
    }

    public void SetDestroyObject(bool setDestoyObject)
    {
        destroyObject = setDestoyObject;
    }
}
