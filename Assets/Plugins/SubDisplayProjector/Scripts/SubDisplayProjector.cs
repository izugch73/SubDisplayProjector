using UnityEngine;
using UnityEngine.UI;

namespace SubDisplayProjector
{
    public class SubDisplayProjector : MonoBehaviour
    {
        [SerializeField] private int displayHeight = 1080;
        [SerializeField] private RawImage subDisplay;
        private RenderTexture _subDisplayRenderTexture;

        private void Start()
        {
            DontDestroyOnLoad(this);
            InitializeRenderTexture();
            AdjustDisplaySize();
        }

        private void InitializeRenderTexture()
        {
            _subDisplayRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            subDisplay.texture = _subDisplayRenderTexture;
        }

        private void AdjustDisplaySize()
        {
            var scale = (float)displayHeight / Screen.height;
            subDisplay.rectTransform.sizeDelta = new Vector2(Screen.width * scale, Screen.height * scale);
        }

        private void OnGUI()
        {
            Graphics.Blit(null, _subDisplayRenderTexture);
        }
    }
}
