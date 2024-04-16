using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TikTokContentCreator
{
    public class QuizCreator : MonoBehaviour
    {
        public const string MODEL = "gpt-3.5-turbo-0613";
        public const string INITIALPROMPT = "";

        private ChatGPTHistory chatHistory;
        private OpenAIApi openAIAPI;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}