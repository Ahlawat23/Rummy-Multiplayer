using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField]
    public float delayBeforeLoading = 3f;

    [SerializeField]
    public string sceneNameToload;

    private float timeElapsed;

    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //timeElapsed += Time.deltaTime;
        
        //if(timeElapsed > delayBeforeLoading)
        //{
        //    SceneManager.LoadScene(sceneNameToload);
        //}
    }

    public void LoadScene1()
    {
        SceneManager.LoadScene("Tournament");
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("enginePhotonStarter");
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTour()
    {
        SceneManager.LoadScene("TournamentScreen");
    }
}
