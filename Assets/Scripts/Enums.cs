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
    /// an enumeration that represents the direction.
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public enum Direction {
#nullable enable

        PositiveZ,
        NegativeZ,
        PositiveX,
        NegativeX,
        None
    };

    #region RenderingMode

    /// <summary>
    /// enum representing the render mode of the material.
    /// </summary>
    public enum RenderingMode {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    #endregion
}