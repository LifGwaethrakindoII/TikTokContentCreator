using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TikTokContentCreator
{
    public enum Language { C, CPP, CSharp, JAVA, JavaScript, Python, General }

    public enum Difficulty { Beginner, Medium, Advanced }

    [Serializable]
    public struct QuizData
    {
        public const string HASHTAGS_GENERAL = "#quiz #quiztime #quizz #quizchallenge #quizchallengepote #programming#programmingquiz #programmingquestion #codequiz #coder #coders #quizshow #quizzes #programacion #codigo";
        public const string HASHTAGS_C = "#C #CQuiz #CQuizzes";
        public const string HASHTAGS_CPP = "#CPP #CPPQuiz #CPPQuizzes";
        public const string HASHTAGS_CSHARP = "#CSharp #CSharpQuiz #CSharpQuizzes";
        public const string HASHTAGS_JAVA = "#JAVA #JAVAQuiz #JAVAQuizzes";
        public const string HASHTAGS_JAVASCRIPT = "#JavaScript #JavaScriptQuiz #JavaScriptQuizzes";
        public const string HASHTAGS_PYTHON = "#Python #PythonQuiz #PythonQuizzes";

        public Language language;
        public Difficulty difficulty;
        public string topic;
        public string question;
        public string code;
        public string optionA;
        public string optionB;
        public string optionC;
        public string optionD;
        public string explanation;
        public string explanationCode;
        public int correctAnswer;

        public string FormattedLanguage()
        {
            switch(language)
            {
                case Language.CPP:      return "C++";
                case Language.CSharp:   return "C#";
                default:                return language.ToString();
            }
        }

        public string GetLanguageHashtags()
        {
            switch (language)
            {
                case Language.C:            return HASHTAGS_C;
                case Language.CPP:          return HASHTAGS_CPP;
                case Language.CSharp:       return HASHTAGS_CSHARP;
                case Language.JAVA:         return HASHTAGS_JAVA;
                case Language.JavaScript:   return HASHTAGS_JAVASCRIPT;
                case Language.Python:       return HASHTAGS_PYTHON;
                default:                    return string.Empty;
            }
        }

        public string GetTikTokTitle()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(FormattedLanguage());
            builder.Append(" Programming Quiz - ");
            builder.Append(difficulty.ToString());
            builder.AppendLine(" Level.");
            builder.Append('\n');
            builder.Append(HASHTAGS_GENERAL);
            builder.Append(" ");
            builder.Append(GetLanguageHashtags());

            return builder.ToString();
        }
    }
}