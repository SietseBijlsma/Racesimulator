﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public interface IEquipement
    {

        int Quality { get; set; }
        int Performance { get; set; }
        int Speed { get; set; }
        bool IsBroken { get; set; }
    }
}
