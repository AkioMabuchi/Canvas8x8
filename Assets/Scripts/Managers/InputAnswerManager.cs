using System;
using UniRx;
using UnityEngine;

namespace Managers
{
    public class InputAnswerManager : SingletonMonoBehaviour<InputAnswerManager>
    {
        private readonly Subject<char> _onInputKey = new Subject<char>();
        public IObservable<char> OnInputKey => _onInputKey;

        private void OnGUI()
        {
            if (Event.current.type == EventType.KeyDown)
            {
                _onInputKey.OnNext(GetCharFromKeyCode(Event.current.keyCode));
            }
        }

        private char GetCharFromKeyCode(KeyCode keyCode)
        {
            return keyCode switch
            {
                KeyCode.A => 'a',
                KeyCode.B => 'b',
                KeyCode.C => 'c',
                KeyCode.D => 'd',
                KeyCode.E => 'e',
                KeyCode.F => 'f',
                KeyCode.G => 'g',
                KeyCode.H => 'h',
                KeyCode.I => 'i',
                KeyCode.J => 'j',
                KeyCode.K => 'k',
                KeyCode.L => 'l',
                KeyCode.M => 'm',
                KeyCode.N => 'n',
                KeyCode.O => 'o',
                KeyCode.P => 'p',
                KeyCode.Q => 'q',
                KeyCode.R => 'r',
                KeyCode.S => 's',
                KeyCode.T => 't',
                KeyCode.U => 'u',
                KeyCode.V => 'v',
                KeyCode.W => 'w',
                KeyCode.X => 'x',
                KeyCode.Y => 'y',
                KeyCode.Z => 'z',
                KeyCode.Minus => '-',
                KeyCode.Backspace => '\b',
                KeyCode.Return => '\r',
                _ => '\0'
            };
        }
    }
}
