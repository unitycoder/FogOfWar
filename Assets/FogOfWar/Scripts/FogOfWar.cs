using UnityEngine;
using System.Collections;
using UnityLibrary.CharacterController;

// optimize: Make grid (each grid contains list of vert(s), then can check nearest grids first, if blocked, stop going that direction
// optimize: take triangle centroid, scan from that! (then set all 3 vertices connected to it, could use angle or distance for each vert)
// extras: can use RGB for adding "fixed" data, for example, this vertex color is locked, or this is half visible..

namespace UnityCoder.Demos
{
    public class FogOfWar : MonoBehaviour
    {
        public Transform[] players;
        public float viewRange = 2;

        public float fadeOutSpeed = 2;
        public float restoreSpeed = 0.2f;

        public bool isDynamic = false;
        public bool checkColliders = false;
        public bool checkViewDirection = false;

        public float viewAngleLimit = 0;
        public float viewCenterDistance = 1;

        public LayerMask layerMask;

        Mesh mesh;
        MeshFilter meshFilter;

        Vector3[] verts;
        Color[] cols;

        // Use this for initialization
        void Awake()
        {
            GetComponent<Renderer>().enabled = true;

            meshFilter = GetComponent<MeshFilter>();
            mesh = meshFilter.mesh;

            verts = mesh.vertices;

            cols = new Color[verts.Length];
            for (int i = 0; i < verts.Length; i++)
            {
                cols[i] = new Color(0, 0, 0, 1);
            }
            mesh.colors = cols;
        }

        void Update()
        {
            if (Input.GetKeyDown("1"))
            {
                isDynamic = !isDynamic;

            }
            int i = 0;
            for (int j = 0; j < verts.Length; j++)
            {
                // TODO: use 2D distance or others for faster
                float distance = Vector3.Distance(players[i].position, transform.TransformPoint(verts[j]));
                //float distance = Vector3.Distance(players[i].position+new Vector3(players[i].rigidbody2D.velocity.x,players[i].rigidbody2D.velocity.y,0),transform.TransformPoint(verts[j]));
                //bool hitCollider = false;

                RaycastHit2D[] hitColliders = new RaycastHit2D[100];
                int hits = 0;
                hits = Physics2D.LinecastNonAlloc(players[i].position, transform.TransformPoint(verts[j]), hitColliders, layerMask);

                //Vector3 perpendicular = Vector3.Cross(p1.position-p2.position,Vector3.forward);
                // TODO: check player DOT
                // TODO cache getcomponent
                Vector3 facingVector = players[i].GetComponent<PlayerController>().facingVector;
                Vector2 viewDirection = Vector3.Cross(facingVector, Vector3.forward);
                facingVector = new Vector3(facingVector.y, -facingVector.x, 0);
                Vector2 vertexDirection = (transform.TransformPoint(verts[j]) - (players[i].position + facingVector * viewCenterDistance)).normalized;
                bool skip = false;

                if (checkViewDirection)
                {
                    if (Vector2.Dot(vertexDirection, viewDirection) < viewAngleLimit)
                    {
                        //Debug.DrawLine(players[i].position, transform.TransformPoint(verts[j]), Color.red);
                        //continue;
                        skip = true;
                    }
                }

                if ((distance < viewRange && hits == 0) && !skip)
                {
                    //if (distance<viewRange/2) // TODO: near circle stronger, or falloff?
                    //{
                    cols[j].a = Mathf.Clamp01(cols[j].a - (viewRange - distance) * Time.deltaTime * fadeOutSpeed); // fading
                                                                                                                   //cols[j].a = Mathf.Clamp01(distance/viewRange); // fixed 0-1, sphere
                                                                                                                   //}else{
                                                                                                                   //cols[j].a =0.5f;// Mathf.Clamp01(cols[j].a-(viewRange-distance)*Time.deltaTime*fadeSpeed);
                                                                                                                   //}
                                                                                                                   //Debug.DrawLine(players[i].position, transform.TransformPoint(verts[j]), Color.green);
                }
                else if (isDynamic)
                {
                    // restore color slowly
                    cols[j].a = Mathf.Clamp01(cols[j].a + Time.deltaTime * restoreSpeed);
                    //Debug.DrawLine(players[i].position, transform.TransformPoint(verts[j]), Color.red);
                }
            } // for verts

            // TODO: update only if had changes
            mesh.colors = cols;
            meshFilter.mesh = mesh;
        }
    }
}
