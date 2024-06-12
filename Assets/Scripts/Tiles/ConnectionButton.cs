using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tiles
{
    public class ConnectionButton : MonoBehaviour
    {
        public Vector2Int pos1;
        public Vector2Int pos2;

        public MeshRenderer meshRenderer;
        public Color highLighColor;

        Color transparentColor;
        bool alreadyHighlighted = false;
        float lastHighlighCallTime = -1;

        private void Awake()
        {
            transparentColor = highLighColor;
            transparentColor.a = 0;
        }

        public void HighLight()
        {
            lastHighlighCallTime = Time.time;
            if (!alreadyHighlighted)
            {
                StartCoroutine(StartHighligh());
            }
        }

        IEnumerator StartHighligh()
        {
            alreadyHighlighted = true;
            while (lastHighlighCallTime + 1f > Time.time)
            {
                meshRenderer.material.color = Color.Lerp(highLighColor, transparentColor, Time.time - lastHighlighCallTime);
                yield return new WaitForEndOfFrame();
            }
            meshRenderer.material.color = transparentColor;
            alreadyHighlighted = false;
        }
    }
}