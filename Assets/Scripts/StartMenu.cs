    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StartMenu : MonoBehaviour {

        public GameObject howToPlay;
        public GameObject credits;
        
        public void StartGame() => SceneManager.LoadScene(1);
        public void ToggleHowToPlay() => howToPlay.SetActive(!howToPlay.activeSelf);
        public void ToggleCredits() => credits.SetActive(!credits.activeSelf);

    }
