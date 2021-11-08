using UniRx;
using UnityEngine;

namespace Models
{
    public static class ThemeModel
    {
        private static readonly ReactiveCollection<string> _themes = new ReactiveCollection<string>();
        public static IReadOnlyReactiveCollection<string> Themes => _themes;

        private static readonly ReactiveDictionary<string, string> _themeSynonyms =
            new ReactiveDictionary<string, string>();

        public static IReadOnlyReactiveDictionary<string, string> ThemeSynonyms => _themeSynonyms;
        

        public static void AddTheme(string theme)
        {
            if (_themeSynonyms.ContainsKey(theme))
            {
                Debug.LogWarning("重複したお題があります");
                return;
            }
            _themes.Add(theme);
            _themeSynonyms.Add(theme, theme);
        }

        public static void AddSynonym(string theme, string synonym)
        {
            if (_themeSynonyms.ContainsKey(synonym))
            {
                Debug.LogWarning("重複した関連があります");
                return;
            }
            _themeSynonyms.Add(synonym, theme);
        }

        public static string GetRandomTheme()
        {
            return _themes[UnityEngine.Random.Range(0, _themes.Count)];
        }

        public static string Answer(string answer)
        {
            if (_themeSynonyms.ContainsKey(answer))
            {
                return _themeSynonyms[answer];
            }

            return "";
        }

        public static bool CanBeAnswer(string answer)
        {
            return _themeSynonyms.ContainsKey(answer);
        }
    }
}
