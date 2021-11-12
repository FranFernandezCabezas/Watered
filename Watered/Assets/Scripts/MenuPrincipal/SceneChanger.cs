﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string SceneName;

    public void ChangeToScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}