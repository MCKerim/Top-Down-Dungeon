using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MenüCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] cameras;

    [SerializeField] private int startCamera;
    private int activeCam;

    private bool canChangeCamera = true;
    private float delay = 2;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        activeCam = startCamera;
        cameras[activeCam].m_Priority = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= 1 * Time.deltaTime;
        }

        if(canChangeCamera && timer <= 0)
        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                NextCam();
            }

            if (Input.GetKeyDown(KeyCode.A))
            { 
                PreviousCam();
            }
        }
    }

    public void LockCamera(bool yes)
    {
        canChangeCamera = !yes;
    }

    public void NextCam()
    {
        if (activeCam + 1 <= cameras.Length - 1)
        {
            cameras[activeCam].m_Priority = 0;
            activeCam++;
            cameras[activeCam].m_Priority = 100;

            timer = delay;
        }
        //else
        //{
        //    cameras[activeCam].m_Priority = 0;
        //    activeCam = 0;
        //    cameras[activeCam].m_Priority = 100;
        //
        //    timer = delay;
        //}
    }

    public void PreviousCam()
    {
        if (activeCam - 1 >= 0)
        {
            cameras[activeCam].m_Priority = 0;
            activeCam--;
            cameras[activeCam].m_Priority = 100;

            timer = delay;
        }
        //else
        //{
        //    cameras[activeCam].m_Priority = 0;
        //    activeCam = cameras.Length - 1;
        //    cameras[activeCam].m_Priority = 100;
        //
        //    timer = delay;
        //}
    }
}
