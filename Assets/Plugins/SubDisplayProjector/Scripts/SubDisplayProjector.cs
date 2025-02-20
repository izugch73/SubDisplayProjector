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

        private void Start()
        {
            DontDestroyOnLoad(this);
            InitializeRenderTexture();

            var displaySize = GetSecondDisplaySettings();
            AdjustDisplaySize(displaySize);
        }

        private Vector2 GetSecondDisplaySettings()
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
            if (adjustMode == AdjustMode.HeightBasis)
            {
                var scale = size.y / Screen.height;
                subDisplay.rectTransform.sizeDelta = new Vector2(Screen.width * scale, Screen.height * scale);
            }
            else if (adjustMode == AdjustMode.WidthBasis)
            {
                var scale = size.x / Screen.width;
                subDisplay.rectTransform.sizeDelta = new Vector2(Screen.width * scale, Screen.height * scale);
            }
            else // whole basis
            {
                var scaleWidth = size.x / Screen.width;
                var scaleHeight = size.y / Screen.height;

                if (scaleWidth < scaleHeight)
                {
                    // 幅に合わせる（WidthBasisの処理と同様）
                    var scale = scaleWidth;
                    subDisplay.rectTransform.sizeDelta = new Vector2(Screen.width * scale, Screen.height * scale);
                }
                else
                {
                    // 高さに合わせる（HeightBasisの処理と同様）
                    var scale = scaleHeight;
                    subDisplay.rectTransform.sizeDelta = new Vector2(Screen.width * scale, Screen.height * scale);
                }
            }
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