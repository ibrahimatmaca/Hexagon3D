using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Material cubeMat;

	public float cubeSize = 0.2f;
	public int cubesInRow = 5;

	float cubesPivotDistance;
	Vector3 cubesPivot;

	public float explosionForce = 50f;
	public float explosionRadius = 4f;
	public float explosionUpward = 0.4f;
    public bool isBox;
	// Use this for initialization
	void Start()
	{
		//calculate pivot distance
		cubesPivotDistance = cubeSize * cubesInRow / 2;
		//use this value to create pivot vector)
		cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            explode();
        }
    }

	public void explode()
	{
        //make object disappear
        //GetComponent<MeshRenderer>().enabled = (false);
        Destroy(this.gameObject, 2f);
        gameObject.SetActive(false);
		//loop 3 times to create 5x5x5 pieces in x,y,z coordinates
		for (int x = 0; x < cubesInRow; x++)
		{
			for (int y = 0; y < cubesInRow; y++)
			{
				for (int z = 0; z < cubesInRow; z++)
				{
					createPiece(x, y, z);
				}
			}
		}

		//get explosion position
		Vector3 explosionPos = transform.position;
		//get colliders in that position and radius
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
		//add explosion force to all colliders in that overlap sphere
		foreach (Collider hit in colliders)
		{
			//get rigidbody from collider object
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if (rb != null)
			{
				//add explosion force to this body with given parameters
				rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
			}
		}
	}

	private void createPiece(int x, int y, int z)
	{
		//create piece
		GameObject piece;
		piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        piece.layer = 15;
		//set piece position and scale
		piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
		piece.transform.localScale = new Vector3(cubeSize + Random.Range(-0.09f, 0.09f), cubeSize + Random.Range(-0.09f, 0.09f), cubeSize + Random.Range(-0.09f, 0.09f));
        //set piece material
        piece.GetComponent<MeshRenderer>().material = cubeMat;
		//add rigidbody and set mass
		piece.AddComponent<Rigidbody>();
		piece.GetComponent<Rigidbody>().mass = cubeSize * 3;
        piece.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 200f);
        Destroy(piece, 2f);
	}


}