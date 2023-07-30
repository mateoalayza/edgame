using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorials.DialogSystem.Scripts
{
    public class DialogueManager : MonoBehaviour
    {

        public DialogGraph activeDialog;

        public GameObject buttonPrefab;

        public Transform buttonParent;

        private int segmentIndex = 0;

        private DialogSegment activeSegment;

        //variables 
        public RectTransformAnimation rect;
        public List<Transform> posiciones; 

        // panel
        void Start()
        {
            // Find Start Node
            foreach (DialogSegment node in activeDialog.nodes)
            {
                if (!node.GetInputPort("input").IsConnected)
                {
                    UpdateDialog(node);
                    break;
                }
            }
        }

        public void AnswerClicked(int clickedIndex)
        {
            GameObject[] buts = GameObject.FindGameObjectsWithTag("but");
            for (int i = 0; i < buts.Length; i++)
            {
                buts[i].GetComponent<Button>().enabled = false;
            }

            StartCoroutine(seq(clickedIndex));        
            


        }

        public IEnumerator seq(int clickIndexgo)
        {
            yield return new WaitForSeconds(1.5f);

            GameObject[] buts = GameObject.FindGameObjectsWithTag("but");
            for (int i = 0; i < buts.Length; i++)
            {
                Destroy(buts[i].gameObject);
            }

            var port = activeSegment.GetPort("Answers " + clickIndexgo);
            if (port.IsConnected)
            {
                UpdateDialog(port.Connection.node as DialogSegment);
            }
            else
            {
                gameObject.SetActive(false);

            }

        }



        private void UpdateDialog(DialogSegment newSegment)
        {
            activeSegment = newSegment;
            rect.texto = newSegment.DialogText;
            rect.AnimateWidthThenExecute(true);

            int answerIndex = 0;
            foreach (Transform child in buttonParent)
            {
                Destroy(child.gameObject);
            }

            Shuffle(posiciones);
            int a = 0;

            // poblar opciones 

            foreach (var answer in newSegment.Answers)
            {
                // ubicaciones
                var btn = Instantiate(buttonPrefab, buttonParent);
                btn.transform.position = posiciones[a].position;

                // Setear enunciado
                btn.GetComponent<opcion_but>().settext(answer.answerText);

                // funcion del boton
                var index = answerIndex;
                btn.GetComponentInChildren<Button>().onClick.AddListener((() => { AnswerClicked(index); }));

                btn.SetActive(false);
                a++;
                answerIndex++;
            }
        }





        // funcion opciones



        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

}
