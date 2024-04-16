using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace TikTokContentCreator
{
    [CustomEditor(typeof(QuizUI))]
    public class QuizUIEditor : Editor
    {
        private QuizUI _quizUI;
        private string json;

        public QuizUI quizUI
        {
            get
            {
                if(_quizUI == null) _quizUI = target as QuizUI;
                return _quizUI;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Update Data")) quizUI.UpdateData();
            json = GUILayout.TextField(json);
            if (!string.IsNullOrEmpty(json) && GUILayout.Button("Generate Data from JSON")) GenerateDataFromJSON();

            serializedObject.ApplyModifiedProperties();
        }

        private void GenerateDataFromJSON()
        {
            QuizData data = default;

            try
            {
                data =  JsonUtility.FromJson<QuizData>(json);
                quizUI.SetData(data);
                quizUI.UpdateData();

            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}