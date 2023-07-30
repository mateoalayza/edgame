using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Tutorials.DialogSystem.Scripts
{
    [Serializable]
    public struct Connection { }

    [Serializable]
    public class DialogAnswer
    {
        [TextArea (3,5)]
        public string answerText;
        public bool activateFeedback;
        [TextArea(3, 5)]
        public string feedback;
    }

    public class DialogSegment : Node
    {
        [Input]
        public Connection input;

        [TextArea(3, 5)]
        public string DialogText;

        // New list to store the images for each answer
        public List<Texture2D> Images = new List<Texture2D>();

        [Output(dynamicPortList = true)]
        public List<DialogAnswer> Answers = new List<DialogAnswer>();

        // Public field to store the color of the node
        [SerializeField]
        public Color NodeColor = Color.black;

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
