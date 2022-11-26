/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GameObject;

using static Studio.MeowToon.Env;

namespace Studio.MeowToon {
    /// <summary>
    /// name of Dictionary is too long, it be named Map.
    /// </summary>
    public class Map<K, V> : Dictionary<K, V> {
    }

    /// <summary>
    /// changed event args.
    /// </summary>
    public class EvtArgs : EventArgs {
        public EvtArgs(string name) {
            Name = name;
        }
        public string Name { get; }
    }

    /// <summary>
    /// changed event handler.
    /// </summary>
    public delegate void Changed(object sender, EvtArgs e);

    /// <summary>
    /// generic utility class
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public static class Utils {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Fields [noun, adjectives] 

        /// <summary>
        /// color.
        /// </summary>
        static Color _red, _orange, _yellow, _lime, _green, _cyan, _azure, _blue, _purple, _magenta, _white;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // static Constructor

        static Utils() {
            ColorUtility.TryParseHtmlString(htmlString: COLOR_RED, color: out _red);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_ORANGE, color: out _orange);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_YELLOW, color: out _yellow);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_LIME, color: out _lime);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_GREEN, color: out _green);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_CYAN, color: out _cyan);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_AZURE, color: out _azure);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_BLUE, color: out _blue);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_PURPLE, color: out _purple);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_MAGENTA, color: out _magenta);
            ColorUtility.TryParseHtmlString(htmlString: COLOR_WHITE, color: out _white);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Properties [noun, noun phrase, adjective]

        public static Color red { get => _red; }
        public static Color orange { get => _orange; }
        public static Color yellow { get => _yellow; }
        public static Color lime { get => _lime; }
        public static Color green { get => _green; }
        public static Color cyan { get => _cyan; }
        public static Color azure { get => _azure; }
        public static Color blue { get => _blue; }
        public static Color purple { get => _purple; }
        public static Color magenta { get => _magenta; }
        public static Color white { get => _white; }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb]

        #region has the component.

        /// <summary>
        /// has level.
        /// </summary>
        //public static bool HasLevel() {
        //    GameObject game_object = Find(name: LEVEL_TYPE);
        //    if (game_object is not null) {
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// has vehicle.
        /// </summary>
        //public static bool HasVehicle() {
        //    GameObject game_object = Find(name: VEHICLE_TYPE);
        //    if (game_object is not null) {
        //        return true;
        //    }
        //    return false;
        //}

        #endregion

        /// <summary>
        /// set the rendering mode of the material.
        /// </summary>
        //public static void SetRenderingMode(Material material, RenderingMode rendering_mode) {
        //    switch (rendering_mode) {
        //        case RenderingMode.Opaque:
        //            material.SetOverrideTag("RenderType", "");
        //            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        //            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        //            material.SetInt("_ZWrite", 1);
        //            material.DisableKeyword("_ALPHATEST_ON");
        //            material.DisableKeyword("_ALPHABLEND_ON");
        //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        //            material.renderQueue = -1;
        //            break;
        //        case RenderingMode.Cutout:
        //            material.SetOverrideTag("RenderType", "TransparentCutout");
        //            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        //            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        //            material.SetInt("_ZWrite", 1);
        //            material.EnableKeyword("_ALPHATEST_ON");
        //            material.DisableKeyword("_ALPHABLEND_ON");
        //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        //            material.renderQueue = 2450;
        //            break;
        //        case RenderingMode.Fade:
        //            material.SetOverrideTag("RenderType", "Transparent");
        //            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //            material.SetInt("_ZWrite", 0);
        //            material.DisableKeyword("_ALPHATEST_ON");
        //            material.EnableKeyword("_ALPHABLEND_ON");
        //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        //            material.renderQueue = 3000;
        //            break;
        //        case RenderingMode.Transparent:
        //            material.SetOverrideTag("RenderType", "Transparent");
        //            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        //            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //            material.SetInt("_ZWrite", 0);
        //            material.DisableKeyword("_ALPHATEST_ON");
        //            material.DisableKeyword("_ALPHABLEND_ON");
        //            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        //            material.renderQueue = 3000;
        //            break;
        //    }
        //}
    }

    /// <summary>
    /// class for vibrate an Android phone.
    /// @author h.adachi
    /// </summary>
    public static class AndroidVibrator {
#if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
#endif
        public static void Vibrate(long milliseconds) {
            if (isAndroid()) {
                vibrator.Call(methodName: "vibrate", args: milliseconds);
            } else {
                Handheld.Vibrate();
            }
        }

        static bool isAndroid() {
#if UNITY_ANDROID && !UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}