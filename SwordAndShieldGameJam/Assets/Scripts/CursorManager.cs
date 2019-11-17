using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public bool isVisable;
    // Start is called before the first frame update
    void Start()
    {
        isVisable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVisable)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }
}
