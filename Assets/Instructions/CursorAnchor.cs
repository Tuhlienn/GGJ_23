using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    public class CursorAnchor : MonoBehaviour
    {

        void Update()
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            this.transform.position = pos;   
        }
    }
}