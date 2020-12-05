using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CSV.GetInstance().loadFile(Application.dataPath + "/Res", "myTest.csv");
        for (int i = 0; i < CSV.GetInstance().m_ArrayData.Count; i++)
        {
            Debug.Log(CSV.GetInstance().m_ArrayData[i]);
        }
    }
}
