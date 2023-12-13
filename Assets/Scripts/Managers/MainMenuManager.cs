using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Play(String scene)
        {
            if (scene != "Gameplay")
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            SceneManager.LoadScene(scene);
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}
