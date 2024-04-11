using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTestState : IState
{
    private int zahl;

    public FirstTestState(int number)
    {
        zahl = number;
    }

    public void Enter()
    {
        Debug.Log("Enter");
    }

    public void Execute()
    {
        Debug.Log("Execute");
        if(true)
        {
            
        }
    }

    public void Exit()
    {
        Debug.Log("Exit");
    }
}

public class SearchResults
{
    public float number;
    public SearchResults(float result)
    {
        number = result;
    }

}
