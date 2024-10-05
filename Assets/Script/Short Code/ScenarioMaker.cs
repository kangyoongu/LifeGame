using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioMaker : MonoBehaviour
{
    public static ScenarioMaker Instance;
    public Scenario[] scenarios;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}
