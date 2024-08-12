using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshWound : MonoBehaviour
{
    [Header("FleshWound Components")]
    [SerializeField] Transform fleshWoundLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fleshWoundLocation != null)
        {
            transform.position = fleshWoundLocation.position;
        }
    }

    public void SetFleshWoundLocation(Transform fleshWoundPos)
    {
        fleshWoundLocation = fleshWoundPos;
    }
}
