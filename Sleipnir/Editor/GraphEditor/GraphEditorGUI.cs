#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sleipnir;

namespace Sleipnir.Editor
{        
    public partial class GraphEditor
    {
        private static GraphEditor m_Window;
        
        private Texture2D _gridTexture;
        public Texture2D gridTexture {
            get {
                if (_gridTexture == null) _gridTexture = GenerateGridTexture(new Color(0.2f, 0.2f, 0.2f, 1f), Color.gray);
                return _gridTexture;
            }
        }
        private Texture2D _crossTexture;
        public Texture2D crossTexture {
            get {
                if (_crossTexture == null) _crossTexture = GenerateCrossTexture(Color.white);
                return _crossTexture;
            }
        }
        public static Texture2D GenerateGridTexture(Color line, Color bg) {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++) {
                for (int x = 0; x < 64; x++) {
                    Color col = bg;
                    if (y % 16 == 0 || x % 16 == 0) col = Color.Lerp(line, bg, 0.65f);
                    if (y == 63 || x == 63) col = Color.Lerp(line, bg, 0.35f);
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }

        public static Texture2D GenerateCrossTexture(Color line) {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++) {
                for (int x = 0; x < 64; x++) {
                    Color col = line;
                    if (y != 31 && x != 31) col.a = 0;
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Cross";
            tex.Apply();
            return tex;
        }
        
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool AutoOpenCanvas (int instanceID, int line) 
        {
            if (Selection.activeObject != null && Selection.activeObject.GetType () == typeof(Graph))
            {
                if (m_Window == null)
                {
                    m_Window = GetWindow<GraphEditor>();
                    m_Window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
                    m_Window.titleContent = new GUIContent("Graph Editor");
                }
                m_Window.LoadGraph((Graph)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(instanceID), typeof(Graph)));
                m_Window.Repaint();
                return true;
            }
            return false;
        }
        
        private void BeginZoomed() {
            GUI.EndClip();
            GUI.EndClip();

            GUIUtility.ScaleAroundPivot(Vector2.one / Zoom, position.size * 0.5f);
            Vector4 padding = new Vector4(0, 22, 0, 0);
            padding *= Zoom;
            GUI.BeginClip(new Rect(-((position.width * Zoom) - position.width) * 0.5f, -(((position.height * Zoom) - position.height) * 0.5f) + (22 * Zoom),
                position.width * Zoom,
                position.height * Zoom));
        }

        private void EndZoomed() {
            GUIUtility.ScaleAroundPivot(Vector2.one * Zoom, position.size * 0.5f);
            GUI.BeginClip(GUIHelper.CurrentWindow.position);
        }
        
        private void DrawGrid()
        {
            Rect rect = new Rect(Vector2.zero, position.size);
            Vector2 center = rect.size / 2f;
            // Offset from origin in tile units
            float xOffset = -(center.x * Zoom + Pan.x) / gridTexture.width;
            float yOffset = ((center.y - rect.size.y) * Zoom + Pan.y) / gridTexture.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            // Amount of tiles
            float tileAmountX = Mathf.Round(rect.size.x * Zoom) / gridTexture.width;
            float tileAmountY = Mathf.Round(rect.size.y * Zoom) / gridTexture.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

            // Draw tiled background
            GUI.DrawTextureWithTexCoords(rect, gridTexture, new Rect(tileOffset, tileAmount));
            GUI.DrawTextureWithTexCoords(rect, crossTexture, new Rect(tileOffset + new Vector2(0.5f, 0.5f), tileAmount));
        }
        
        private void DrawConnectionToMouse()
        {
            if (selectedOut != null && selectedIn == null)
            {
                Handles.DrawBezier(
                    GridToGUIDrawRect(selectedOut.OutputKnob()).center,
                    Event.current.mousePosition,
                    GridToGUIDrawRect(selectedOut.OutputKnob()).center + Vector2.right * 50f,
                    Event.current.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );
            }
        }
    }
}
#endif