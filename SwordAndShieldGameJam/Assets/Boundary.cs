using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public Transform resetPoint;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerObject")
        {
            other.gameObject.transform.position = resetPoint.position;
        }
    }
}
