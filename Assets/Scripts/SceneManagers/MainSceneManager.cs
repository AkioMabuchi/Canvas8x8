using System;
using System.Collections.Generic;
using Canvases;
using Managers;
using Models;
using UniRx;
using UnityEngine;

namespace SceneManagers
{
    public class MainSceneManager : MonoBehaviour
    {
        private enum Mode
        {
            Initial = 0,
            Waiting,
            Examiner,
            Answerer
        }

        private Mode _currentMode = Mode.Answerer;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private void Start()
        {
            ChangeMode(_currentMode);
        }

        private void OnDestroy()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
        }

        private void ChangeMode(Mode mode)
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();

            switch (mode)
            {
                case Mode.Initial:
                {
                    break;
                }
                case Mode.Waiting:
                {
                    break;
                }
                case Mode.Examiner:
                {
                    break;
                }
                case Mode.Answerer:
                {
                    _disposables.Add(AnswerInputModel.InputText.Subscribe(text =>
                    {
                        CanvasAnswer.Instance.SetText(text);
                    }));
                    
                    _disposables.Add(InputAnswerManager.Instance.OnInputKey.Subscribe(inputChar =>
                    {
                        switch (inputChar)
                        {
                            case '\r':
                            {
                                if (true)
                                {
                                    AnswerInputModel.Clear();
                                }
                                break;
                            }
                            case '\b':
                            {
                                AnswerInputModel.Delete();
                                break;
                            }
                            default:
                            {
                                if (inputChar == '\0')
                                {
                                    break;
                                }

                                AnswerInputModel.Input(inputChar);
                                break;
                            }
                        }
                    }));
                    break;
                }
            }

            _currentMode = mode;
        }
    }
}
