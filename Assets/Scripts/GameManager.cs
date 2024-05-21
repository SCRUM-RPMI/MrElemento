using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject[] gm;

    private void Awake()
    {
        gm = GameObject.FindGameObjectsWithTag("GameController");
        if(gm.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cambiarEscena(int numEscena)
    {
        SceneManager.LoadScene(numEscena);
    }
}
