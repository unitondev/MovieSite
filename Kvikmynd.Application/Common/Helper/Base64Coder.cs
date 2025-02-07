﻿using System;
using System.IO;

namespace Kvikmynd.Application.Common.Helper
{
    public static class Base64Coder
    {
        public static string EncodeImageToString(string path)
        {
            byte[] imageArray = File.ReadAllBytes(Path.GetFullPath(path));
            return Convert.ToBase64String(imageArray);
        }
    }
}