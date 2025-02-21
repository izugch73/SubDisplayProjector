using UnityEngine;
using UnityEngine.Serialization;
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

        /**
         * Safe Area Consideration
         * If true, the safe area is considered when adjusting the display size.
         */
        [SerializeField] private bool isBasedSafeArea = false;

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
            var width = display2.systemWidth;
            var height = display2.systemHeight;

            return new Vector2(width, height);
        }

        private void InitializeRenderTexture()
        {
            _subDisplayRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            subDisplay.texture = _subDisplayRenderTexture;
        }

        private void AdjustDisplaySize(Vector2 size)
        {
            float baseWidth = Screen.width;
            float baseHeight = Screen.height;
            var scale = 1f;

            if (isBasedSafeArea)
            {
                var safeArea = Screen.safeArea;

                // Calculate the offset from the center of the screen
                var screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
                var offset = safeArea.center - screenCenter;
                
                // Calculate the scale based on the safe area
                var scaleWidth = size.x / safeArea.width;
                var scaleHeight = size.y / safeArea.height;

                scale = adjustMode switch
                {
                    AdjustMode.HeightBasis => scaleHeight,
                    AdjustMode.WidthBasis => scaleWidth,
                    _ => Mathf.Min(scaleWidth, scaleHeight)
                };

                subDisplay.rectTransform.anchoredPosition = -offset * scale;
            }
            else
            {
                var scaleWidth = size.x / baseWidth;
                var scaleHeight = size.y / baseHeight;

                scale = adjustMode switch
                {
                    AdjustMode.HeightBasis => scaleHeight,
                    AdjustMode.WidthBasis => scaleWidth,
                    _ => Mathf.Min(scaleWidth, scaleHeight)
                };
            }

            var newSizeDelta = new Vector2(baseWidth * scale, baseHeight * scale);
            subDisplay.rectTransform.sizeDelta = newSizeDelta;
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
