using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelUtil : MonoBehaviour {
	public void LoadALevel(string levelName)
	{
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			// init seed at the start of the game
			int seed = GameController.GetSeed();
			Random.InitState(seed);
			PlayerPrefs.SetInt("seed", seed);
			PlayerPrefs.Save();
		}
		SceneManager.LoadSceneAsync(levelName);
	}
	public void Quit(string levelName)
	{
		PlayerPrefs.Save();
		Application.Quit();
	}
}