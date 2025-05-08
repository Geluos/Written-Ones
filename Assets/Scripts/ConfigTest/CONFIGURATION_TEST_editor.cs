#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CONFIGURATION_TEST))]
public class CONFIGURATION_TEST_editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CONFIGURATION_TEST tester = (CONFIGURATION_TEST)target;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            fixedHeight = 30,
            padding = new RectOffset(10, 10, 5, 5),
            richText = true
        };

        // Цвета для нормального и нажатого состояния
        buttonStyle.normal.background = MakeTex(2, 2, new Color(0.1f, 0.5f, 0.8f, 0.9f));
        buttonStyle.hover.background = MakeTex(2, 2, new Color(0.2f, 0.6f, 0.9f, 0.9f));
        buttonStyle.active.background = MakeTex(2, 2, new Color(0.05f, 0.4f, 0.7f, 0.9f));
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.hover.textColor = Color.white;

        // Отступ перед кнопкой
        EditorGUILayout.Space(15);

        EditorGUILayout.BeginVertical(GUI.skin.box);
        if (GUILayout.Button("<size=14><color=white></color> <b>►😊Запустить тестирование</b></size>", buttonStyle))
        {
            EditorApplication.Beep();
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
        }

        EditorGUILayout.HelpBox("Обновление конфигурации и сохранение отчета", MessageType.Info);
        EditorGUILayout.EndVertical();

    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
#endif

