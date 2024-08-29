using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxManager : MonoBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] GameObject[] BOEntities;
    [SerializeField] GameObject[] YOEntities;
    [SerializeField] GameObject[] ragdolls;

    float resetState = 0;
    float startState = 0;
    bool inPlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AddToEntityList();
        StartInput();
        ResetBattleInput();

        if(inPlay == true)
        {
            StartBattle();
        }

    }

    void AddToEntityList()
    {
        BOEntities = GameObject.FindGameObjectsWithTag("BueOrder");
        YOEntities = GameObject.FindGameObjectsWithTag("YellowOrder");
        ragdolls = GameObject.FindGameObjectsWithTag("Ragdoll");
    }

    //sets the unit state to attack
    void StartBattle()
    {
        for(int i = 0; i < BOEntities.Length; i++)
        {
            var entity = BOEntities[i];
            entity.GetComponentInParent<BaseUnit>().SetInBattle(true);
        }

        for (int i = 0; i < YOEntities.Length; i++)
        {
            var entity = YOEntities[i];
            entity.GetComponentInParent<BaseUnit>().SetInBattle(true);
        }
    }

    //Deletes all soldier entities on the field
    void ResetBattle()
    {
        for (int i = 0; i < BOEntities.Length; i++)
        {
            var entity = BOEntities[i];
            entity.GetComponent<DestroyParentObject>().SetDestroyObject(true);
        }

        for (int i = 0; i < YOEntities.Length; i++)
        {
            var entity = YOEntities[i];
            entity.GetComponent<DestroyParentObject>().SetDestroyObject(true);
        }

        for (int i = 0; i < ragdolls.Length; i++)
        {
            var ragdollEntity = ragdolls[i];
            Destroy(ragdollEntity);
        }
    }

    //checks if the player wants to start the battle
    void StartInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(startState == 0)
            {
                inPlay = true;
                startState++;
            }
            else
            {
                inPlay = false;
                startState = 0;
            }
            
        }
    }

    void ResetBattleInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(resetState == 0)
            {
                ResetBattle();
                resetState++;
            }
            else
            {
                resetState = 0;
            }

        }
    }
}
