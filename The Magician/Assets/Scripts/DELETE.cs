using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheMagician;

public class DELETE : MonoBehaviour
{
    [SerializeField] MouseController mc;
    void LateUpdate()
    {
        transform.position = mc.Position;
    }
}
