using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsHand : MonoBehaviour
{
    public GameObject head;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(head.transform.position);
        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
}
