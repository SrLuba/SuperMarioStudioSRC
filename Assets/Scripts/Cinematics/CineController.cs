using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineController : MonoBehaviour
{
    public AudioSource src;
    public void ChangeScene(string scene) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
    public void StopMusic() {
        src.Stop();
    }
}
