using UnityEngine;
using UnityEngine.UI;

namespace SubDisplayProjector
{
    public class SubDisplayProjector : MonoBehaviour
    {
        [SerializeField] private RawImage subDisplay;
        private RenderTexture _subDisplayRenderTexture;

        /**
         * Adjustment Modes
         * WidthBasis: Render based on the width. The top and bottom may be cropped or black bars added.
         * HeightBasis: Render based on the height. The left and right may be cropped or black bars added.
         * WholeScreen: Render the entire screen with black bars added.
         */
        [SerializeField] private AdjustMode adjustMode = AdjustMode.WholeScreen;

        private void Start()
        {
            DontDestroyOnLoad(this);
            InitializeRenderTexture();

            var displaySize = GetAnotherScreenResolution();
            AdjustDisplaySize(displaySize);
        }

        private static Vector2 GetAnotherScreenResolution()
        {
            if (Display.displays.Length < 2) return new Vector2(1920, 1080);

            var display2 = Display.displays[1];
            var height = display2.systemHeight;
            var width = display2.systemWidth;

            return new Vector2(width, height);
        }

        private void InitializeRenderTexture()
        {
            _subDisplayRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            subDisplay.texture = _subDisplayRenderTexture;
        }

        private void AdjustDisplaySize(Vector2 size)
        {
            var scaleWidth = size.x / Screen.width;
            var scaleHeight = size.y / Screen.height;

            subDisplay.rectTransform.sizeDelta = adjustMode switch
            {
                AdjustMode.HeightBasis => AdjustScale(scaleHeight),
                AdjustMode.WidthBasis => AdjustScale(scaleWidth),
                _ => scaleWidth < scaleHeight ? AdjustScale(scaleWidth) : AdjustScale(scaleHeight)
            };
        }
        
        private static Vector2 AdjustScale(float scale)
        {
            return new Vector2(Screen.width * scale, Screen.height * scale);
        }

        private void OnGUI()
        {
            Graphics.Blit(null, _subDisplayRenderTexture);
        }
    }

    public enum AdjustMode
    {
        WholeScreen,
        HeightBasis,
        WidthBasis
    }
}