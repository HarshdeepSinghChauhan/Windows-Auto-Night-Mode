﻿#region copyright
//  Copyright (C) 2022 Auto Dark Mode
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDarkModeSvc.Timers
{
    static class TimerFrequency
    {
        // short timer is 60s
        public const int Short = 60000;
        // IO Timer is 2h
        public const int IO = 7200000;
        // location Timer is 1h
        public const int Location = 3600000;
        // update timer for system state
        public const int StateUpdate = 300000;
        // timer for sensor polling
        public const int SensorPoll = 1000;

    }
}
