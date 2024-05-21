using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvascontroller : MonoBehaviour
{

    public GameObject panelAjustes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activarAjustes(bool active)
    {
        panelAjustes.SetActive(active);
    }
}
