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

namespace Studio.MeowToon {
    /// <summary>
    /// envelope class
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public static class Env {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants

        public const int FPS = 60; // 30fps

        public const string SCENE_TITLE = "Title";
        public const string SCENE_SELECT = "Select";
        public const string SCENE_LEVEL_1 = "Level1";
        public const string SCENE_LEVEL_2 = "Level2";
        public const string SCENE_LEVEL_3 = "Level3";
        public const string SCENE_ENDING = "Ending";

        public const string MODE_EASY = "easy";
        public const string MODE_NORMAL = "normal";
        public const string MODE_HARD = "hard";

        public const string CAMERA_SYSTEM = "CameraSystem";
        public const string GAME_SYSTEM = "GameSystem";
        public const string NOTICE_SYSTEM = "NoticeSystem";

        public const string BLOCK_TYPE = "Block";
        public const string GROUND_TYPE = "Ground";
        public const string WALL_TYPE = "Wall";
        //public const string BALLOON_TYPE = "Balloon";
        //public const string COIN_TYPE = "Coin";
        public const string Player_TYPE = "Orange";
        public const string HOME_TYPE = "Home";
        public const string LEVEL_TYPE = "Level";

        //public const string MESSAGE_LEVEL_START = "Get items!";
        public const string MESSAGE_LEVEL_CLEAR = "Level Clear!";
        //public const string MESSAGE_GAME_OVER = "Game Over!";
        public const string MESSAGE_GAME_PAUSE = "Pause";

        /// https://www.color-sample.com/colorschemes/rule/dominant/
        public const string COLOR_RED = "#FF0000";
        public const string COLOR_ORANGE = "#FF7F00";
        public const string COLOR_YELLOW = "#FFFF00";
        public const string COLOR_LIME = "#7FFF00";
        public const string COLOR_GREEN = "#00FF00";
        public const string COLOR_CYAN = "#00FFFF";
        public const string COLOR_AZURE = "#007FFF";
        public const string COLOR_BLUE = "#002AFF";
        public const string COLOR_PURPLE = "#D400FF";
        public const string COLOR_MAGENTA = "#FF007F";
        public const string COLOR_WHITE = "#FFFFFF";
    }
}