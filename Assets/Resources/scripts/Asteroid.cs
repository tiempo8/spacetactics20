using UnityEngine;

public class Asteroid : MonoBehaviour {
	
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag==TagsStorage.ENEMY_TAG)
			col.gameObject.GetComponent<EnemySpaceship>().Destroyed();
		if(col.gameObject.tag==TagsStorage.FRIENDLY_TAG)
			col.gameObject.GetComponent<FriendlySpaceship>().Destroyed();
		if(col.gameObject.tag==TagsStorage.ROCKET_TAG || col.gameObject.tag==TagsStorage.THORPEDE_TAG)
			col.gameObject.GetComponent<MissleBehaviour>().Boom();
	}
}
