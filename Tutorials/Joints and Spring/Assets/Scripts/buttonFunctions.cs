using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void restart()
    {
        SceneManager.LoadScene("Main");
    }
}