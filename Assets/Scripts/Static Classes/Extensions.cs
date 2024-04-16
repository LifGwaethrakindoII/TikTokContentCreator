using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TikTokContentCreator
{
    public static class Extensions
    {
        public static readonly char[] WORDS_SPLITTER = { ' ', ',', '.', ';', '!', '?' };
        public static readonly string[] CKeywords =
        {
            "auto", "break", "case", "const", "continue", "default", "do", "else", "enum", "extern", "for",
            "goto", "if", "inline", "register", "return", "sizeof", "static", "struct", "switch", "typedef",
            "union", "unsigned", "void", "volatile", "while"
        };

        public static readonly string[] CPPKeywords =
        {
            "alignas", "alignof", "and", "and_eq", "asm", "atomic_cancel", "atomic_commit", "atomic_noexcept",
            "bitand", "bitor", "compl", "concept", "consteval", "constexpr", "const_cast", "decltype", "delete",
            "dynamic_cast", "explicit", "export", "false", "friend", "mutable", "namespace", "new", "noexcept",
            "not", "not_eq", "nullptr", "operator", "or", "or_eq", "reinterpret_cast", "requires", "static_assert",
            "static_cast", "template", "this", "thread_local", "throw", "true", "try", "typeid", "typename", "xor",
            "xor_eq"
        };

        public static readonly string[] CSharpKeywords =
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class",
            "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event",
            "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in",
            "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator",
            "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte",
            "sealed", "short", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try",
            "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
        };

        public static readonly string[] JAVAKeywords =
        {
            "abstract", "assert", "boolean", "break", "byte", "case", "catch", "char", "class", "continue",
            "default", "do", "double", "else", "enum", "extends", "final", "finally", "float", "for", "if",
            "implements", "import", "instanceof", "interface", "long", "native", "new", "package", "private",
            "protected", "public", "return", "short", "static", "strictfp", "super", "switch", "synchronized",
            "this", "throw", "throws", "transient", "try", "void", "volatile", "while"
        };

        public static readonly string[] JavaScriptKeywords =
        {
            "abstract", "arguments", "await", "boolean", "break", "byte", "case", "catch", "char", "class",
            "const", "continue", "debugger", "default", "delete", "do", "double", "else", "enum", "eval",
            "export", "extends", "false", "final", "finally", "float", "for", "function", "goto", "if",
            "implements", "import", "in", "instanceof", "int", "interface", "let", "long", "native", "new",
            "null", "package", "private", "protected", "public", "return", "short", "static", "super",
            "switch", "synchronized", "this", "throw", "throws", "transient", "true", "try", "typeof", "var",
            "void", "volatile", "while", "with", "yield"
        };

        public static readonly string[] PythonKeywords =
        {
            "False", "None", "True", "and", "as", "assert", "async", "await", "break", "class", "continue",
            "def", "del", "elif", "else", "except", "finally", "for", "from", "global", "if", "import", "in",
            "is", "lambda", "nonlocal", "not", "or", "pass", "raise", "return", "try", "while", "with", "yield"
        };

        public static readonly string[] CVariables =
        {
            "auto", "char", "const", "double", "extern", "float", "int", "long", "register", "short", "signed",
            "static", "struct", "typedef", "union", "unsigned", "void", "volatile"
        };

        public static readonly string[] CPPVariables =
        {
            "alignas", "alignof", "and", "and_eq", "asm", "atomic_cancel", "atomic_commit", "atomic_noexcept",
            "bitand", "bitor", "bool", "char", "char8_t", "char16_t", "char32_t", "class", "compl", "concept",
            "const", "consteval", "constexpr", "const_cast", "decltype", "delete", "double", "dynamic_cast",
            "explicit", "export", "extern", "false", "float", "int", "long", "mutable", "namespace", "new",
            "noexcept", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "private", "protected", "public",
            "reinterpret_cast", "requires", "return", "short", "signed", "sizeof", "static", "static_assert",
            "static_cast", "struct", "template", "this", "thread_local", "throw", "true", "try", "typeid", "typename",
            "union", "unsigned", "virtual", "void", "volatile", "wchar_t", "xor", "xor_eq"
        };

        public static readonly string[] CSharpVariables =
        {
            "bool", "byte", "char", "decimal", "double", "float", "int", "long", "sbyte", "short", "uint", "ulong",
            "ushort", "var"
        };

        public static readonly string[] JAVAVariables =
        {
            "boolean", "byte", "char", "double", "float", "int", "long", "short"
        };

        public static readonly string[] JavaScriptVariables =
        {
            "const", "let", "var"
        };

        public static readonly string[] PythonVariables =
        {
            // Python does not have variable type declarations, so there are no specific keywords reserved for variable types
        };

        public const float SECONDS_PERWORD = 0.25f;
        public const string TAG_COLOR_OPEN = "<color>";
        public const string TAG_COLOR_CLOSED = "</color>";
        public const string HEXCOLOR_HIGHLIGHT = "#5b89b7";
        public const string HEXCOLOR_KEYWORD = "#e15c63";
        public const string HEXCOLOR_VARIABLE = "#9d80ac";

        public static float CalculateReadTime(this string _text, float _secondsPerWord = SECONDS_PERWORD)
        {
            string[] words = _text.Split(WORDS_SPLITTER, StringSplitOptions.RemoveEmptyEntries);
            float count = words.Length * 1.0f;

            return count * _secondsPerWord;
        }

        public static T Random<T>(this T[] _array, T _default = default)
        {
            return _array == null || _array.Length == 0 ? _default : _array[UnityEngine.Random.Range(0, _array.Length)];
        }

        public static void SetLayoutPivot(this RectTransform _rectTransform, Vector2 _pivot)
        {
            _rectTransform.pivot = _pivot;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        public static Vector2 TextAnchorPivot(this TextAnchor _anchor)
        {
            switch(_anchor)
            {                 
                case TextAnchor.UpperLeft:      return new Vector2(0.0f, 1.0f);
                case TextAnchor.UpperCenter:    return new Vector2(0.5f, 1.0f);
                case TextAnchor.UpperRight:     return new Vector2(1.0f, 1.0f);
                case TextAnchor.MiddleLeft:     return new Vector2(0.0f, 0.5f);
                case TextAnchor.MiddleCenter:   return new Vector2(0.5f, 0.5f);
                case TextAnchor.MiddleRight:    return new Vector2(1.0f, 0.5f);
                case TextAnchor.LowerLeft:      return new Vector2(0.0f, 0.0f);
                case TextAnchor.LowerCenter:    return new Vector2(0.5f, 0.0f);
                case TextAnchor.LowerRight:     return new Vector2(1.0f, 0.0f);
                default:                        return new Vector2(0.0f, 0.0f);
            }
        }

        public static string[] GetKeywords(this Language _language)
        {
            switch (_language)
            {
                case Language.C:            return CKeywords;
                case Language.CPP:          return CPPKeywords;
                case Language.CSharp:       return CSharpKeywords;
                case Language.JAVA:         return JAVAKeywords;
                case Language.JavaScript:   return JavaScriptKeywords;
                case Language.Python:       return PythonKeywords;
                default:                    return null;
            }
        }

        public static string[] GetVariables(this Language _language)
        {
            switch (_language)
            {
                case Language.C:            return CVariables;
                case Language.CPP:          return CPPVariables;
                case Language.CSharp:       return CSharpVariables;
                case Language.JAVA:         return JAVAVariables;
                case Language.JavaScript:   return JavaScriptVariables;
                case Language.Python:       return PythonVariables;
                default:                    return null;
            }
        }

        public static string Highlighted(this string _text, Language _language)
        {
            StringBuilder builder = new StringBuilder();
            string[] keywords = _language.GetKeywords();
            string[] variables = _language.GetVariables();

            Action<string> Highlight = word =>
            {
                builder.Append("<b>");
                builder.Append("<i>");
                builder.Append("<color=");
                builder.Append(HEXCOLOR_KEYWORD);
                builder.Append(">");
                builder.Append(word);
                builder.Append("</color>");
                builder.Append("</i>");
                builder.Append("</b>");
                _text.Replace(word, builder.ToString());
                builder.Clear();
            };

            if(keywords != null) foreach (string keyword in keywords)
            {
                if (_text.ContainsExactWord(keyword))
                {
                    Highlight(keyword);
                }
            }

            if(variables != null) foreach (string variable in variables)
            {
                if (_text.ContainsExactWord(variable))
                {
                    Highlight(variable);
                }
            }

            return _text;
        }

        public static string CodeHighlighted(this string _text, Language _language)
        {
            StringBuilder builder = new StringBuilder();
            string[] keywords = _language.GetKeywords();
            string[] variables = _language.GetVariables();
            string result = _text;

            if(keywords != null) foreach(string keyword in keywords)
            {
                if(_text.ContainsExactWord(keyword))
                {
                    builder.Append("<color=");
                    builder.Append(HEXCOLOR_KEYWORD);
                    builder.Append(">");
                    builder.Append(keyword);
                    builder.Append("</color>");
                    result = result.Replace(keyword, builder.ToString());
                    builder.Clear();
                }
            }

            if(variables != null) foreach(string variable in variables)
            {
                if (_text.ContainsExactWord(variable))
                {
                    builder.Append("<color=");
                    builder.Append(HEXCOLOR_VARIABLE);
                    builder.Append(">");
                    builder.Append(variable);
                    builder.Append("</color>");
                    result = result.Replace(variable, builder.ToString());
                    builder.Clear();
                }
            }

            return result;
        }

        public static bool ContainsExactWord(this string input, string word)
        {
            string pattern = "\\b" + Regex.Escape(word) + "\\b";
            return Regex.IsMatch(input, pattern);
        }
    }
}