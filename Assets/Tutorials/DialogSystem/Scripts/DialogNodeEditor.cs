using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Tutorials.DialogSystem.Scripts
{
    [CustomNodeEditor(typeof(DialogSegment))]
    public class DialogNodeEditor : NodeEditor
    {
        public override int GetWidth()
        {
            return 400; // Aumentamos el ancho del nodo para acomodar el contenido
        }

        public override void OnHeaderGUI()
        {
            var segment = target as DialogSegment;

            // Obtener el estilo original del encabezado del nodo
            GUIStyle headerStyle = NodeEditorResources.styles.nodeHeader;

            // Dibujar el fondo personalizado solo para el encabezado del nodo
            Rect headerRect = GUILayoutUtility.GetRect(0, 0);
            headerRect.width = 350;
            headerRect.height = headerStyle.fixedHeight;
            GUI.backgroundColor = segment.NodeColor;
            GUI.Box(headerRect, GUIContent.none, headerStyle);
            GUI.backgroundColor = Color.white;

            // Display the node's name as the title with larger and bold font
            GUIStyle titleStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold
            };
            EditorGUI.LabelField(headerRect, segment.name, titleStyle);
        }


        public override void OnBodyGUI()
        {

            serializedObject.Update();
            RectOffset rctOff;

            var segment = serializedObject.targetObject as DialogSegment;
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16, 
                margin = new RectOffset(10, 10, 10, 10)

            };

            GUILayout.Label(segment.name, titleStyle);


            NodeEditorGUILayout.PortField(segment.GetPort("input"));



            // Add a color selector to change the color of the node
            GUILayout.Label("Node Color");
            segment.NodeColor = EditorGUILayout.ColorField(segment.NodeColor);

            GUILayout.Label("Dialog Text");
            segment.DialogText = EditorGUILayout.TextArea(segment.DialogText, GUILayout.MinHeight(50));

            GUILayout.Space(10);

            GUILayout.Label("Images");

            for (int i = 0; i < segment.Images.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                Texture2D image = segment.Images[i];
                if (image != null)
                {
                    GUILayout.Box(image, GUILayout.Width(64), GUILayout.Height(64));
                }
                else
                {
                    GUILayout.Box("", GUILayout.Width(64), GUILayout.Height(64));
                }

                EditorGUILayout.BeginVertical();
                EditorGUI.BeginChangeCheck();
                Texture2D newImage = EditorGUILayout.ObjectField(segment.Images[i], typeof(Texture2D), false) as Texture2D;
                if (EditorGUI.EndChangeCheck())
                {
                    segment.Images[i] = newImage;
                }

                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    segment.Images.RemoveAt(i);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Image"))
            {
                segment.Images.Add(null);
            }

            GUILayout.Space(10);

            GUILayout.Label("Answers - Conectar");

            NodeEditorGUILayout.DynamicPortList(
                "Answers", // field name
                typeof(string), // field type
                serializedObject, // serializable object
                NodePort.IO.Input, // new port i/o
                Node.ConnectionType.Override, // new port connection type
                Node.TypeConstraint.None,
                OnCreateReorderableList); // onCreate override. This is where the magic 



            for (int i = 0; i < segment.Answers.Count; i++)
            {
                GUILayout.Label("Answer " + i);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                //EditorGUILayout.LabelField("Answer Text");
                EditorGUILayout.LabelField(segment.Answers[i].answerText);

                //segment.Answers[i].answerText = EditorGUILayout.TextArea(segment.Answers[i].answerText, GUILayout.MinHeight(30));

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Activate Feedback");
                segment.Answers[i].activateFeedback = EditorGUILayout.Toggle(segment.Answers[i].activateFeedback);

                EditorGUILayout.Space();

                if (segment.Answers[i].activateFeedback)
                {
                    EditorGUILayout.LabelField("Feedback");
                    segment.Answers[i].feedback = EditorGUILayout.TextArea(segment.Answers[i].feedback, GUILayout.MinHeight(30));
                }

               
                EditorGUILayout.EndVertical();


                GUILayout.Space(5);
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override Color GetTint()
        {
            var segment = target as DialogSegment;
            return segment.NodeColor;
        }

        void OnCreateReorderableList(ReorderableList list)
        {
            list.elementHeightCallback = (int index) =>
            {
                return 60;
            };

            // Override drawHeaderCallback to display node's name instead
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var segment = serializedObject.targetObject as DialogSegment;

                NodePort port = segment.GetPort("Answers " + index);

                segment.Answers[index].answerText = GUI.TextArea(rect, segment.Answers[index].answerText);

                if (port != null)
                {
                    Vector2 pos = rect.position + (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));
                    NodeEditorGUILayout.PortField(pos, port);
                }
            };
        }

        public override void OnRename()
        {
            var segment = target as DialogSegment;
            string newName = segment.name;

            segment.name = newName;

            // Notify the node editor that a rename action occurred
            NodeEditorWindow[] nodeEditorWindows = Resources.FindObjectsOfTypeAll<NodeEditorWindow>();
            if (nodeEditorWindows != null && nodeEditorWindows.Length > 0)
            {
                foreach (NodeEditorWindow window in nodeEditorWindows)
                {
                    window.Repaint();
                }
            }
        }
    }
}
