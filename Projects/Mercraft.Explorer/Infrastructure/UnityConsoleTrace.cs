﻿using System;
using Mercraft.Infrastructure.Diagnostic;
using UnityEngine;

namespace Mercraft.Explorer.Infrastructure
{
    public class UnityConsoleTrace: DefaultTrace
    {
        protected override void WriteRecord(RecordType type, string category, string message, Exception exception)
        {
            switch (type)
            {
                case RecordType.Error:
                    Debug.LogException(exception);
                    break;
                case RecordType.Warning:
                    Debug.LogWarning(String.Format("{0}:{1}", category, message));
                    break;
                default:
                    Debug.Log(String.Format("{0}:{1}", category, message));
                    break;
            }
        }

    }
}
