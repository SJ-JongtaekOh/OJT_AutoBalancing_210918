using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testojt1 : MonoBehaviour
{
    public List<float> aaa = new List<float>();


    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < 10000; i++)
            {
                aaa.Add(0);
            }

            aaa.RemoveRange(0, aaa.Count);
       
        }

    }
}
