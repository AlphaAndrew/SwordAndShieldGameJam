using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> playersInRange;
    void Start()
    {
        playersInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
