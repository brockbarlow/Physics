using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{   //button function. when the user "clicks" the button labeled "refresh", the
    //cloth simulation scene will be reloaded. this button should be used to
    //rebuild the cloth if 1)cloth is broken or 2)show different outcomes of the
    //project.
    public void restart()
    {
        SceneManager.LoadScene("Main");
    }
}