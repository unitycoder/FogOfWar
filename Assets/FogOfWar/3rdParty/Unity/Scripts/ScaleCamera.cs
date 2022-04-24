using UnityEngine;

namespace UnityLibrary
{
    [ExecuteInEditMode]
    public class ScaleCamera : MonoBehaviour
    {
        public int targetWidth = 640;
        public float pixelsToUnits = 100;

        Camera cam;
        private void OnEnable()
        {
            cam = Camera.main;
        }

        void Update()
        {
            int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);
            cam.orthographicSize = height / pixelsToUnits * 0.5f;
        }
    }
}
