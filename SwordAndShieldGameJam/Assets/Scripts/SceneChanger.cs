using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public string levelOne;

    public void LevelOne()
    {
        SceneManager.LoadScene(levelOne);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
