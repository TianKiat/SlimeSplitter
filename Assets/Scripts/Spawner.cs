using UnityEngine;

public class Spawner : MonoBehaviour {
	private Vector2 origin;

	[SerializeField]
	private float width; // width of spawning box
	[SerializeField]
	private float height; // height of spawning box
	[SerializeField]
	private float minHeight; // minimum height from bottom of spawn box to spawn the balls
	[SerializeField]
	private float minSpacing; // minimum spacing between the balls that spawn
	[SerializeField]
	private GameObject[] spawnables; // balls that this spawner can spawn

	private SpawningBox spawnBox; // spawn box instance

	private Vector2[] savedSpawnPos; // saved spawn positions
	// Use this for initialization
	void Start () 
	{
		origin = transform.position;
		spawnBox = CreateSpawnBox(width, height); // initialize the spawn box
		// init seed
		// if (PlayerPrefs.HasKey("seed"))
		// {
		// 	Random.InitState(PlayerPrefs.GetInt("seed"));
		// }
		// else
		// {
		// 	int seed = System.DateTime.Now.Minute;
		// 	PlayerPrefs.SetInt("seed", seed);
		// }
	}
	// Method: spawn a given number of balls
	public void SpawnBalls(int _numOfBalls)
	{
		Random.InitState(PlayerPrefs.GetInt("seed"));
		Debug.Log(PlayerPrefs.GetInt("seed"));
		Vector2 lastSpawnPosition = origin; // position of ball that was last spawned
		Vector2[] savedPos = new Vector2[_numOfBalls];
		for (int i = 0; i < _numOfBalls; ++i) 
		{
			// generate random position within the spawnBox
			Vector2 spawnPos = new Vector2(Random.Range(spawnBox.botLeft.x, spawnBox.BotRight.x),Random.Range(spawnBox.botLeft.y, spawnBox.topLeft.y));
			// if the spawn position is far enough from the last spawn position and if it is above the minimum height then spawn the ball
			if (Vector2.Distance(spawnPos, lastSpawnPosition) > minSpacing) 
			{
				// spawn a random ball
				int randomIndex = Mathf.RoundToInt(Random.Range(0, spawnables.Length));
				Mathf.Clamp(randomIndex, 0, spawnables.Length - 1);

				GameObject temp = Instantiate(spawnables[randomIndex], spawnPos, Quaternion.identity) as GameObject;
				temp.GetComponent<Ball>().startForce = i % 2 == 0 ? (Vector2.right * 2) :(Vector2.left * 2);

				// update last spawn position
				lastSpawnPosition = spawnPos;
				savedPos[i] = spawnPos;
			} else 
			{
				// decrease the iterator to try again
				--i;
			}
		}
	}

	// Method: create spawning box
	private SpawningBox CreateSpawnBox(float _width, float _height)
	{
		// calculate corners
		Vector2 v1 = new Vector2(origin.x - _width / 2, origin.y + _height / 2);
		Vector2 v2 = new Vector2(origin.x + _width / 2, origin.y + _height / 2);
		Vector2 v3 = new Vector2(origin.x - _width / 2, origin.y - _height / 2);
		Vector2 v4 = new Vector2(origin.x + _width / 2, origin.y - _height / 2);

		return new SpawningBox(v1, v2, v3, v4);
	}
	// Method: for drawing a visual representation of the spawning box when in the editor
	void OnDrawGizmos()
	{
		// if width or height is more than 0 create a visual representation of the spawning box
		if (width > 0 || height > 0 && !Application.isPlaying) 
		{
			origin = transform.position;
			Gizmos.color = Color.blue;
			SpawningBox sb = CreateSpawnBox(width, height);
			Gizmos.DrawLine(sb.topLeft, sb.topRight);
			Gizmos.DrawLine(sb.topLeft, sb.botLeft);
			Gizmos.DrawLine(sb.topRight, sb.BotRight);
			Gizmos.DrawLine(sb.botLeft, sb.BotRight);
		}
	}

	// Struct: spawning box
	struct SpawningBox
	{
		public Vector2 topLeft, topRight, botLeft, BotRight;
		public SpawningBox (Vector2 _topleft, Vector2 _topRight, Vector2 _botLeft, Vector2 _botRight)
		{
			topLeft = _topleft;
			topRight = _topRight;
			botLeft = _botLeft;
			BotRight = _botRight;
		}
	}
}
