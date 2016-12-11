using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
	[SerializeField]
	private Spawner[] spawners;
	[SerializeField]
	private int[] wavesInfo; // array of numbers that say how many balls to spawn
	public int waveNumber = 0;
	[SerializeField]
	private Text countDownText;

	public bool DebugMode;
	// Use this for initialization
	void Start ()
	{
		wavesInfo = ReadWavesFile("Assets/Levels.txt");
		StartCoroutine(GameLoop());
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D)) {
			DebugMode = !DebugMode;
			GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>().isTrigger = DebugMode;
			GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().isKinematic = DebugMode;
		}
	}

	IEnumerator GameLoop()
	{
		// count down
		countDownText.text = "3";
		yield return new WaitForSeconds(1);
		countDownText.text = "2";
		yield return new WaitForSeconds(1);
		countDownText.text = "1";
		yield return new WaitForSeconds(1);
		countDownText.text = "";
		SpawnWave();

		while (waveNumber < wavesInfo.Length) 
		{
			if (GameObject.FindGameObjectsWithTag("Ball").Length == 0) 
			{
				//count down
				countDownText.text = "3";
				yield return new WaitForSeconds(1);
				countDownText.text = "2";
				yield return new WaitForSeconds(1);
				countDownText.text = "1";
				yield return new WaitForSeconds(1);
				countDownText.text = "";
				SpawnWave();
			}
			if (DebugMode && Input.GetKeyDown(KeyCode.K)) 
			{
				GameObject[] temp = GameObject.FindGameObjectsWithTag("Ball");
				foreach (var item in temp) 
				{
					item.GetComponent<Ball>().Split();
				}
			}
			yield return null;
		}

	}

	void SpawnWave()
	{
		// for now spawn from the only spawner
		spawners[0].SpawnBalls(wavesInfo[waveNumber]);
		waveNumber++; // increase wave number
	}

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
	                 // deliniators, then send that array to DoStuff()
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
}
