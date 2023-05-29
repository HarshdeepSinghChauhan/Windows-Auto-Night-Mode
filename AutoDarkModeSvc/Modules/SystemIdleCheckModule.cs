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
using AutoDarkModeLib;
using AutoDarkModeSvc.Core;
using AutoDarkModeSvc.Timers;
using System;
using System.Runtime.InteropServices;


namespace AutoDarkModeSvc.Modules
{
    public class SystemIdleCheckModule : AutoDarkModeModule
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public override string TimerAffinity => TimerName.Main;
        private GlobalState State { get; } = GlobalState.Instance();
        private AdmConfigBuilder builder { get; } = AdmConfigBuilder.Instance();

        public SystemIdleCheckModule(string name, bool fireOnRegistration) : base(name, fireOnRegistration) { }

        public override void Fire()
        {
            LASTINPUTINFO lastinputStruct = new();
            lastinputStruct.cbSize = (uint)Marshal.SizeOf(lastinputStruct);
            GetLastInputInfo(ref lastinputStruct);

            DateTime lastInputTime = DateTime.Now.AddMilliseconds(-(Environment.TickCount - lastinputStruct.dwTime));
            if (lastInputTime <= DateTime.Now.AddMinutes(-builder.Config.IdleChecker.Threshold))
            {
                State.SystemIdle.SystemIsIdle = true;
                Logger.Info($"allow theme switch, system idle since {lastInputTime}, which is longer than {builder.Config.IdleChecker.Threshold} minute(s)");
                State.PostponeManager.Remove(Name);
            }
            else if (State.PostponeManager.Add(new(Name, isUserClearable: false)))
            {
                State.SystemIdle.SystemIsIdle = false;
                Logger.Info("postponing theme switch due to system idle timer");
            }
        }

        public override void DisableHook()
        {
            State.SystemIdle.SystemIsIdle = false;
            State.PostponeManager.Remove(Name);
        }

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }
    }

}
