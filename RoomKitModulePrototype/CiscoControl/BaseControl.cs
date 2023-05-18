﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class BaseControl
    {
        public string[] Path { get; set; }
        public LogicModule.SendCommandToCodecDelegate SendCommandToCodecHandler { get; set; }
        protected virtual void OnControlStateChanged() { } 
        
    }
}
