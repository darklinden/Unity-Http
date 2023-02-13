using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogsShow : MonoBehaviour
{
    [SerializeField] private Text m_Text;

    private void Awake()
    {
        Log.D("LogsShow.Awake");
        m_Text.text = Log.LogStringBuilder.ToString();
        Log.OnLog += Log_OnLog;
    }

    private void Log_OnLog(string obj)
    {
        m_Text.text = Log.LogStringBuilder.ToString();
    }
}
