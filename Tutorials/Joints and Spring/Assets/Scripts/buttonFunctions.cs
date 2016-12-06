namespace Assets.Scripts
{   //required usings
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ButtonFunctions : MonoBehaviour
    {   //button function. when the user "clicks" the button labeled "refresh", the
        //cloth simulation scene will be reloaded. this button should be used to
        //rebuild the cloth if 1)cloth is broken or 2)show different outcomes of the
        //project. 

        //When the user clicks the button labeled "Controls", the app will
        //load the controls scene. controls scene informs the user what to use
        //during run time.
        public void Restart()
        {
            SceneManager.LoadScene("Main");
        }

        public void Controls()
        {
            SceneManager.LoadScene("Controls");
        }
    }
}