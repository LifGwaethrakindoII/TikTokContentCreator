using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OpenAI
{
    [Serializable]
    public class ChatGPTHistory
    {
        [SerializeField, HideInInspector] private List<string> messages;

        public ChatGPTHistory()
        {
            messages = new List<string>();
        }

        public void AddMessage(string message)
        {
            messages.Add(message);
        }

        public override string ToString()
        {
            if(messages == null || messages.Count == 0) return string.Empty;

            StringBuilder builder = new StringBuilder();
            
            foreach(string message in messages)
            {
                builder.AppendLine(message);
            }
            
            return builder.ToString();
        }
    }
}