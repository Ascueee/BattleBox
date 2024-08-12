using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [Header("Cursor Var")]
    [SerializeField] LayerMask cursorHitLayer;
    [SerializeField] GameObject cursorObj;
    [SerializeField] GameObject spawnEntityObj;
    [SerializeField] UIManager uiManager;

    GameObject cursor;
    Vector3 worldPosition;

    bool spawnCursor;
    bool inCursor;
    bool inDelete;
    float cursorState;
    float state;

    // Start is called before the first frame update
    void Start()
    {
        spawnCursor = true;
        state = 0;
        cursorState = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        PlayerInput();

        if(inCursor == true)
        {
            CursorObject();
            DeleteInput();
            MoveCursorObject();

        }
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
 
            if (state == 0)
            {
                inCursor = true;
                state++;
            }
            else
            {
                state = 0;
                inCursor = false;
                spawnCursor = true;
                Destroy(cursor);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {

            if(spawnEntityObj != null && cursorObj != null)
            {
                var entity = Instantiate(spawnEntityObj, cursor.transform.position, Quaternion.identity);
            }

        }
    }
    
    //spawns cursor object where the mouse is
    void CursorObject()
    {
        var cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(spawnCursor == true)
        {
            cursor = Instantiate(cursorObj, cursorPos, Quaternion.identity);
            uiManager.SetCursorObj(cursor.GetComponent<Cursor>());
            spawnCursor = false;
        }
    }

    void MoveCursorObject()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray cursorPosRay = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(cursorPosRay, out RaycastHit hitData, cursorHitLayer))
        {
            if (hitData.collider.gameObject.tag == "BueOrder" || hitData.collider.gameObject.tag == "YellowOrder")
            {
                if(inDelete == true)
                {
                    print("Hit entity");
                    Destroy(hitData.collider.gameObject.transform.parent.gameObject);
                }
                
            }

            worldPosition = hitData.point;
            
        }
        cursor.transform.position = worldPosition;
        uiManager.SetCursorObj(gameObject.GetComponent<Cursor>());

    }

    void DeleteInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(cursorState == 0)
            {
                
                inDelete = true;
                cursorState++;
            }
            else
            {
                inDelete = false;
                cursorState = 0;
            }

        }
    }

    public void SetEntitySpawner(GameObject entity)
    {
        spawnEntityObj = entity;
    }
}
