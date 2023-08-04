
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Mirror;
public class MainMenu : NetworkBehaviour
{

    public static string playerName;
    public GameObject defoultSection;
    public GameObject optionsSection;
    public Animator[] textAnimations;
   //public GameObject clientPopup;
  //  public GameObject serverPopup;

    public void Start()
    {
      

        LoadLevel(); 
        Time.timeScale = 1.0f;

    }

    public void PlayGame()
    {
        int nextScene =  SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void PlayAgain()
    {
        int thisScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(thisScene);
       // LoadLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        int previousScene = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(previousScene);
    }

    public void BackToGame()
    {
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
    }

    public void LoadLevel()
    {
        LevelData data = SaveSystem.LoadLevel();
        if (data != null)
        {
            playerName = data.playerName;
            
        }
    }

    public void ReturnFromOptionsSection()
    {
        optionsSection.SetActive(false);
        defoultSection.SetActive(true);

        foreach (Animator anim in textAnimations)
        {
            anim.Play(0,-1, 0);
            //anim.Play("GameNameAnim",0);
        }

    }
    public void EnterOptionsSection()
    {

        defoultSection.SetActive(false);
        optionsSection.SetActive(true);
        foreach (Animator anim in textAnimations)
        {
            anim.Play(0, -1, 0);
            //anim.Play("GameNameAnim",0);
        }
    }

    public void OnMouseOvder()
    {
      //  FindObjectOfType<AudioManager>().Play("ButtonHover");
    }

    public void OnMousePress()
    {
       // FindObjectOfType<AudioManager>().Play("ButtonPress_2");
    }

}
