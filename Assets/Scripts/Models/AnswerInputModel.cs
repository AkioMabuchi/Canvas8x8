using System;
using System.Collections.Generic;
using Managers;
using UniRx;
using UnityEngine;

namespace Models
{
    public static class AnswerInputModel
    {
        private static readonly char[] _inputProcess = new char[16];
        private static int _inputProcessIndex;

        private static readonly Dictionary<string, string> _ime4 = new Dictionary<string, string>
        {
            {"ltsu", "っ"},
            {"xtsu", "っ"}
        };
        private static readonly Dictionary<string, string> _ime3 = new Dictionary<string, string>
        {
            {"bya","びゃ"},
            {"byi","びぃ"},
            {"byu","びゅ"},
            {"bye","びぇ"},
            {"byo","びょ"},
            {"cha","ちゃ"},
            {"chi","ち"},
            {"chu","ちゅ"},
            {"che","ちぇ"},
            {"cho","ちょ"},
            {"cya","ちゃ"},
            {"cyi","ちぃ"},
            {"cyu","ちゅ"},
            {"cye","ちぇ"},
            {"cyo","ちょ"},
            {"dha","でゃ"},
            {"dhi","でぃ"},
            {"dhu","でゅ"},
            {"dhe","でぇ"},
            {"dho","でょ"},
            {"dwa","どぁ"},
            {"dwi","どぃ"},
            {"dwu","どぅ"},
            {"dwe","どぇ"},
            {"dwo","どぉ"},
            {"dya","ぢゃ"},
            {"dyi","ぢぃ"},
            {"dyu","ぢゅ"},
            {"dye","ぢぇ"},
            {"dyo","ぢょ"},
            {"fwa","ふぁ"},
            {"fwi","ふぃ"},
            {"fwu","ふぅ"},
            {"fwe","ふぇ"},
            {"fwo","ふぉ"},
            {"fya","ふゃ"},
            {"fyi","ふぃ"},
            {"fyu","ふゅ"},
            {"fye","ふぇ"},
            {"fyo","ふょ"},
            {"gwa","ぐぁ"},
            {"gwi","ぐぃ"},
            {"gwu","ぐぅ"},
            {"gwe","ぐぇ"},
            {"gwo","ぐぉ"},
            {"gya","ぎゃ"},
            {"gyi","ぎぃ"},
            {"gyu","ぎゅ"},
            {"gye","ぎぇ"},
            {"gyo","ぎょ"},
            {"hya","ひゃ"},
            {"hyi","ひぃ"},
            {"hyu","ひゅ"},
            {"hye","ひぇ"},
            {"hyo","ひょ"},
            {"jya","じゃ"},
            {"jyi","じぃ"},
            {"jyu","じゅ"},
            {"jye","じぇ"},
            {"jyo","じょ"},
            {"kwa","くぁ"},
            {"kwi","くぃ"},
            {"kwu","くぅ"},
            {"kwe","くぇ"},
            {"kwo","くぉ"},
            {"kya","きゃ"},
            {"kyi","きぃ"},
            {"kyu","きゅ"},
            {"kye","きぇ"},
            {"kyo","きょ"},
            {"ltu","っ"},
            {"lya","ゃ"},
            {"lyu","ゅ"},
            {"lye","ぇ"},
            {"lyo","ょ"},
            {"mya","みゃ"},
            {"myi","みぃ"},
            {"myu","みゅ"},
            {"mye","みぇ"},
            {"myo","みょ"},
            {"nya","にゃ"},
            {"nyi","にぃ"},
            {"nyu","にゅ"},
            {"nye","にぇ"},
            {"nyo","にょ"},
            {"pya","ぴゃ"},
            {"pyi","ぴぃ"},
            {"pyu","ぴゅ"},
            {"pye","ぴぇ"},
            {"pyo","ぴょ"},
            {"rya","りゃ"},
            {"ryi","りぃ"},
            {"ryu","りゅ"},
            {"rye","りぇ"},
            {"ryo","りょ"},
            {"sha","しゃ"},
            {"shi","し"},
            {"shu","しゅ"},
            {"she","しぇ"},
            {"sho","しょ"},
            {"sya","しゃ"},
            {"syi","しぃ"},
            {"syu","しゅ"},
            {"sye","しぇ"},
            {"syo","しょ"},
            {"tha","てゃ"},
            {"thi","てぃ"},
            {"thu","てゅ"},
            {"the","てぇ"},
            {"tho","てょ"},
            {"tsa","つぁ"},
            {"tsi","つぃ"},
            {"tsu","つ"},
            {"tse","つぇ"},
            {"tso","つぉ"},
            {"twa","とぁ"},
            {"twi","とぃ"},
            {"twu","とぅ"},
            {"twe","とぇ"},
            {"two","とぉ"},
            {"tya","ちゃ"},
            {"tyi","ちぃ"},
            {"tyu","ちゅ"},
            {"tye","ちぇ"},
            {"tyo","ちょ"},
            {"vya","ゔゃ"},
            {"vyu","ゔゅ"},
            {"vyo","ゔょ"},
            {"wha","うぁ"},
            {"whi","うぃ"},
            {"whe","うぇ"},
            {"who","うぉ"},
            {"xtu","っ"},
            {"xya","ゃ"},
            {"xyu","ゅ"},
            {"xye","ぇ"},
            {"xyo","ょ"},
            {"zya","じゃ"},
            {"zyi","じぃ"},
            {"zyu","じゅ"},
            {"zye","じぇ"},
            {"zyo","じょ"},
        };

        private static readonly Dictionary<string, string> _ime2 = new Dictionary<string, string>
        {
            {"ba","ば"},
            {"bi","び"},
            {"bu","ぶ"},
            {"be","べ"},
            {"bo","ぼ"},
            {"da","だ"},
            {"di","ぢ"},
            {"du","づ"},
            {"de","で"},
            {"do","ど"},
            {"fa","ふぁ"},
            {"fi","ふぃ"},
            {"fu","ふ"},
            {"fe","ふぇ"},
            {"fo","ふぉ"},
            {"ga","が"},
            {"gi","ぎ"},
            {"gu","ぐ"},
            {"ge","げ"},
            {"go","ご"},
            {"ha","は"},
            {"hi","ひ"},
            {"hu","ふ"},
            {"he","へ"},
            {"ho","ほ"},
            {"ja","じゃ"},
            {"ji","じ"},
            {"ju","じゅ"},
            {"je","じぇ"},
            {"jo","じょ"},
            {"ka","か"},
            {"ki","き"},
            {"ku","く"},
            {"ke","け"},
            {"ko","こ"},
            {"la","ぁ"},
            {"li","ぃ"},
            {"lu","ぅ"},
            {"le","ぇ"},
            {"lo","ぉ"},
            {"ma","ま"},
            {"mi","み"},
            {"mu","む"},
            {"me","め"},
            {"mo","も"},
            {"na","な"},
            {"ni","に"},
            {"nu","ぬ"},
            {"ne","ね"},
            {"no","の"},
            {"nn","ん"},
            {"pa","ぱ"},
            {"pi","ぴ"},
            {"pu","ぷ"},
            {"pe","ぺ"},
            {"po","ぽ"},
            {"ra","ら"},
            {"ri","り"},
            {"ru","る"},
            {"re","れ"},
            {"ro","ろ"},
            {"sa","さ"},
            {"si","し"},
            {"su","す"},
            {"se","せ"},
            {"so","そ"},
            {"ta","た"},
            {"ti","ち"},
            {"tu","つ"},
            {"te","て"},
            {"to","と"},
            {"va","ゔぁ"},
            {"vi","ゔぃ"},
            {"vu","ゔ"},
            {"ve","ゔぇ"},
            {"vo","ゔぉ"},
            {"wa","わ"},
            {"wi","うぃ"},
            {"wu","う"},
            {"we","うぇ"},
            {"wo","を"},
            {"xa","ぁ"},
            {"xi","ぃ"},
            {"xu","ぅ"},
            {"xe","ぇ"},
            {"xo","ぉ"},
            {"xn","ん"},
            {"ya","や"},
            {"yu","ゆ"},
            {"ye","いぇ"},
            {"yo","よ"},
            {"za","ざ"},
            {"zi","じ"},
            {"zu","ず"},
            {"ze","ぜ"},
            {"zo","ぞ"},
            {"bb","っb"},
            {"cc","っc"},
            {"dd","っd"},
            {"ff","っf"},
            {"gg","っg"},
            {"hh","っh"},
            {"jj","っj"},
            {"kk","っk"},
            {"ll","っl"},
            {"mm","っm"},
            {"pp","っp"},
            {"qq","っq"},
            {"rr","っr"},
            {"ss","っs"},
            {"tt","っt"},
            {"vv","っv"},
            {"ww","っw"},
            {"xx","っx"},
            {"yy","っy"},
            {"zz","っz"},
            {"nb","んb"},
            {"nc","んc"},
            {"nd","んd"},
            {"nf","んf"},
            {"ng","んg"},
            {"nh","んj"},
            {"nj","んj"},
            {"nk","んk"},
            {"nl","んl"},
            {"nm","んm"},
            {"np","んp"},
            {"nq","んq"},
            {"nr","んr"},
            {"ns","んs"},
            {"nt","んt"},
            {"nv","んv"},
            {"nw","んw"},
            {"nx","んx"},
            {"nz","んz"}
        };

        private static readonly Dictionary<char, string> _ime1 = new Dictionary<char, string>
        {
            {'a', "あ"},
            {'i', "い"},
            {'u', "う"},
            {'e', "え"},
            {'o', "お"},
            {'-', "ー"}
        };
        
        private static readonly ReactiveProperty<string> _inputText = new ReactiveProperty<string>("");
        public static IReadOnlyReactiveProperty<string> InputText => _inputText;


        static AnswerInputModel()
        {
            for (int i = 0; i < 16; i++)
            {
                _inputProcess[i] = '\0';
            }

            if (SystemInfo.operatingSystem.Contains("Windows"))
            {
                _ime2.Add("ca", "か");
                _ime2.Add("ci", "し");
                _ime2.Add("cu", "く");
                _ime2.Add("ce", "せ");
                _ime2.Add("co", "こ");
            }

            if (SystemInfo.operatingSystem.Contains("Mac"))
            {
                
            }
        }
        
        public static void Clear()
        {
            for (int i = 0; i < 16; i++)
            {
                _inputProcess[i] = '\0';
            }

            _inputProcessIndex = 0;
            TextOutput();
        }

        public static void Input(char inputChar)
        {
            bool hasImeActed = false; 
            _inputProcess[_inputProcessIndex] = inputChar;
            
            if (_inputProcessIndex >= 3)
            {
                string context = "";
                context += _inputProcess[_inputProcessIndex - 3];
                context += _inputProcess[_inputProcessIndex - 2];
                context += _inputProcess[_inputProcessIndex - 1];
                context += _inputProcess[_inputProcessIndex];
                if (_ime4.ContainsKey(context))
                {
                    _inputProcess[_inputProcessIndex - 3] = '\0';
                    _inputProcess[_inputProcessIndex - 2] = '\0';
                    _inputProcess[_inputProcessIndex - 1] = '\0';
                    _inputProcess[_inputProcessIndex] = '\0';
                    _inputProcessIndex -= 3;
                    char[] characters = _ime4[context].ToCharArray();
                    foreach (char character in characters)
                    {
                        _inputProcess[_inputProcessIndex] = character;
                        _inputProcessIndex++;
                    }
                    hasImeActed = true;
                }
            }
            
            if (_inputProcessIndex >= 2)
            {
                string context = "";
                context += _inputProcess[_inputProcessIndex - 2];
                context += _inputProcess[_inputProcessIndex - 1];
                context += _inputProcess[_inputProcessIndex];
                if (_ime3.ContainsKey(context))
                {
                    _inputProcess[_inputProcessIndex - 2] = '\0';
                    _inputProcess[_inputProcessIndex - 1] = '\0';
                    _inputProcess[_inputProcessIndex] = '\0';
                    _inputProcessIndex -= 2;
                    char[] characters = _ime3[context].ToCharArray();
                    foreach (char character in characters)
                    {
                        _inputProcess[_inputProcessIndex] = character;
                        _inputProcessIndex++;
                    }
                    hasImeActed = true;
                }
            }

            if (_inputProcessIndex >= 1)
            {
                string context = "";
                context += _inputProcess[_inputProcessIndex - 1];
                context += _inputProcess[_inputProcessIndex];
                if (_ime2.ContainsKey(context))
                {
                    _inputProcess[_inputProcessIndex - 1] = '\0';
                    _inputProcess[_inputProcessIndex] = '\0';
                    _inputProcessIndex -= 1;
                    char[] characters = _ime2[context].ToCharArray();
                    foreach (char character in characters)
                    {
                        _inputProcess[_inputProcessIndex] = character;
                        _inputProcessIndex++;
                    }
                    hasImeActed = true;
                }
            }

            if (_ime1.ContainsKey(_inputProcess[_inputProcessIndex]))
            {
                char[] characters = _ime1[_inputProcess[_inputProcessIndex]].ToCharArray();
                foreach (char character in characters)
                {
                    _inputProcess[_inputProcessIndex] = character;
                    _inputProcessIndex++;
                    hasImeActed = true;
                }
            }

            if (!hasImeActed)
            {
                _inputProcessIndex++;
            }
            for (int i = 10; i < 16; i++)
            {
                _inputProcess[i] = '\0';
            }

            if (_inputProcessIndex > 10)
            {
                _inputProcessIndex = 10;
            }

            TextOutput();
        }

        public static void Delete()
        {
            if (_inputProcessIndex >= 1)
            {
                _inputProcess[_inputProcessIndex - 1] = '\0';
                _inputProcessIndex--;
            }

            TextOutput();
        }

        static void TextOutput()
        {
            string text = String.Empty;
            foreach (char character in _inputProcess)
            {
                text += character;
            }

            _inputText.Value = text;
        }
    }
}
