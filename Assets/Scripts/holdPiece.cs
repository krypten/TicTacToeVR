using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdPiece : MonoBehaviour {
    public GameObject GameLogic;
    public GameObject raycastHolder;
    public GameObject player;
    public GameObject pieceBeingHeld;
	public GameObject gravityAttractor;

    public bool holdingPiece = false;
    public float hoverHeight = 0.3f;

    RaycastHit hit;
	public float gravityFactor = 10.0f;
	private Vector3 forceDirection;

	private BoxCollider boxColliderComponent;
	private GameLogic gameLogicComponent;
	private Rigidbody rigidBodyComponent;
	private PlayerPiece playerPieceComponent;

    // Use this for initialization
    void Start () {
	}

	public void grabPiece(GameObject selectedPiece) {
        if (selectedPiece.GetComponent<PlayerPiece>().hasBeenPlayed == false) {
            pieceBeingHeld = selectedPiece;
			
			boxColliderComponent = pieceBeingHeld.GetComponent<BoxCollider> ();
			gameLogicComponent = GameLogic.GetComponent<GameLogic>();
			playerPieceComponent = pieceBeingHeld.GetComponent<PlayerPiece>();
			rigidBodyComponent = pieceBeingHeld.GetComponent<Rigidbody> ();
			
			holdingPiece = true;
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        if (GameLogic.GetComponent<GameLogic>().playerTurn == true) {
            if (holdingPiece == true) {
                Vector3 forwardDir = raycastHolder.transform.TransformDirection(Vector3.forward) * 100;
                Debug.DrawRay(raycastHolder.transform.position, forwardDir, Color.green);

                if (Physics.Raycast(raycastHolder.transform.position, (forwardDir), out hit)) {
					gravityAttractor.transform.position = new Vector3(hit.point.x, hit.point.y + hoverHeight, hit.point.z);

					rigidBodyComponent.useGravity = false;
					boxColliderComponent.enabled = false;

					rigidBodyComponent.AddForce(gravityAttractor.transform.position - pieceBeingHeld.transform.position);

                    if (hit.collider.gameObject.tag == "Grid Plate") {
                        if (Input.GetMouseButtonDown(0)) {
                            holdingPiece = false;
                            hit.collider.gameObject.SetActive(false);
                            playerPieceComponent.hasBeenPlayed = true;
                            rigidBodyComponent.useGravity = true;
                            boxColliderComponent.enabled = true;
                            gameLogicComponent.playerMove(hit.collider.gameObject);
                        }
                    }
                }
            }
        }
    }
}
