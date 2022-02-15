﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LodeRunner.API.Test.IntegrationTests.Extensions
{
    internal static class SecretVolumeExtension
    {
        public static string GetTempSecretVolume(this string volume)
        {
            string dirName = System.Environment.CurrentDirectory;
            if (System.OperatingSystem.IsLinux())
            {
                return $"../../../../../temp/{volume}";
            }
            else
            {
                return volume;
            }
        }
    }
}
