using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameController : MonoBehaviour {
	[SerializeField]
	private Spawner[] spawners;
	[SerializeField]
	private int[] wavesInfo; // array of numbers that say how many balls to spawn
	public int waveNumber = 0;
	[SerializeField]
	private Text countDownText;
	[SerializeField]
	private Text waveNumberText;

	public bool DebugMode;
	// Use this for initialization
	void Start ()
	{
		DOTween.Init(true, true); // init DOTween
		wavesInfo = ReadWavesFile("Assets/Levels.txt");
		// init countDownText
		countDownText.DOFade(0, 0);
		// start game loop
		StartCoroutine(GameLoop());
	}

	void Update()
	{
		// debug mode stuff
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D)) {
			DebugMode = !DebugMode;
			GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>().isTrigger = DebugMode;
			GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().isKinematic = DebugMode;
		}
		// if (DebugMode && Input.GetKeyDown(KeyCode.K)) 
		// {
		// 	GameObject[] temp = GameObject.FindGameObjectsWithTag("Ball");
		// 	foreach (var item in temp) 
		// 	{
		// 		item.GetComponent<Ball>().Split();
		// 	}
		// }
	}
	// main game loop
	IEnumerator GameLoop()
	{
		// count down
		yield return StartCoroutine(CountDown(3));
		SpawnWave();

		while (waveNumber < wavesInfo.Length) 
		{
			if (GameObject.FindGameObjectsWithTag("Ball").Length == 0) 
			{
				//count down
				yield return StartCoroutine(CountDown(3));
				SpawnWave();
			}
			yield return null;
		}
		// game over
		Debug.Log("Game Over");
		yield return null;

	}
	// spawn a wave of balls from the assigned spawner
	void SpawnWave()
	{
		// for now spawn from the only spawner
		spawners[0].SpawnBalls(wavesInfo[waveNumber]);
		waveNumber++; // increase wave number
		waveNumberText.text = waveNumber.ToString();
		waveNumberText.fontSize = 70;
		DOTween.To(()=> waveNumberText.fontSize, x=> waveNumberText.fontSize = x, 50, .2f);
	}
	// Method: Read the waves file
	int[] ReadWavesFile(string fileName)
	{
		string line;
		try 
		{
			StreamReader theReader = new StreamReader(fileName, true);
			using (theReader)
	        {
	         // While there's lines left in the text file, do this:
	         do
	         {
	             line = theReader.ReadLine();
	                 
	             if (line != null)
	             {
	                 // Do whatever you need to do with the text line, it's a string now
	                 // In this example, I split it into arguments based on comma
	                 string[] entries = line.Split(',');
	                 if (entries.Length > 0)
	                 {
	                 	int[] result = new int[entries.Length];
						for (int i = 0; i < entries.Length; i++) 
						{
							result[i] = int.Parse(entries[i]);
						}
						return result;
	                 }

	             }
	         }
	         while (line != null);
	         // Done reading, close the reader and return true to broadcast success    
	         theReader.Close();
	     }

	    }
		catch (System.Exception ex) 
		{
			Debug.Log(ex.Message);
			return null;
		}
		return null;
	}

	//Method: count down
	IEnumerator CountDown(int amount)
	{
		countDownText.text = "";
		// fades text in
		countDownText.DOFade(1, .5f);
		// tween fontSize from 250 to 200
		for (int i = 0; i < amount; i++)
		{
			//count down
			int a = amount - i;
			countDownText.text = a.ToString();
			countDownText.fontSize = 300;
			DOTween.To(()=> countDownText.fontSize, x=> countDownText.fontSize = x, 200, .9f);
			yield return new WaitForSeconds(1);
		}
		// fade countDownText out
		countDownText.DOFade(0, .5f);
		yield return null;
	}
}
