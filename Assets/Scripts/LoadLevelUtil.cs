using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelUtil : MonoBehaviour {
	public void LoadALevel(string levelName)
	{
		SceneManager.LoadSceneAsync(levelName);
	}
	public void Quit(string levelName)
	{
		Application.Quit();
	}
}