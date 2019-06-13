using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartQuickbook.Data
{
    class ElementResponseLog
    {
        string Accion {get; set;}
        string Send {get; set;}
        string Recived{get; set;}
        bool Success { get; set; }
        public ElementResponseLog(string accion , string send, string recived, bool success) {
            Accion = accion;
            Send = send;
            Recived = recived;
            Success = success;
        }
        
    }
}
