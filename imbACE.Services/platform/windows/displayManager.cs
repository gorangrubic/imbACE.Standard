// --------------------------------------------------------------------------------------------------------------------
// <copyright file="displayManager.cs" company="imbVeles" >
//
// Copyright (C) 2017 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbACE.Services
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Services.platform.windows
{
    #region imbVeles using

    using imbSCI.Core.extensions.text;
    using imbSCI.Data.enums;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    /// <summary>
    /// Upravlja podešavanjima prikaza -- pozicioniranjem ekrana, koristi WinApi
    /// </summary>
    public class displayManager
    {
        /*

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        /// <summary>
        /// Postavlja prozor na zadatu poziciju
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
                                                SetWindowPosFlags uFlags);

        private static Rectangle getTargetArea(displayOption displayMode)
        {
            Rectangle targetArea = imbSettingsManager.primaryDisplay.WorkingArea;

            switch (displayMode)
            {
                case displayOption.defaultDisplay:
                    targetArea = imbSettingsManager.primaryDisplay.WorkingArea;
                    break;

                case displayOption.display1:
                    targetArea = imbSettingsManager.displays[0].WorkingArea;
                    break;

                case displayOption.display2:
                    targetArea = imbSettingsManager.displays[1].WorkingArea;
                    break;

                case displayOption.display3:
                    targetArea = imbSettingsManager.displays[2].WorkingArea;
                    break;

                case displayOption.display4:
                    targetArea = imbSettingsManager.displays[3].WorkingArea;
                    break;

                default:
                    break;
            }
            return targetArea;
        }

        private static void deployDisplayMode(displayOption displayMode, IntPtr handle)
        {
            Rectangle targetArea = imbSettingsManager.primaryDisplay.WorkingArea;
            Boolean moveWindow = false;
            switch (displayMode)
            {
                default:

                    break;

                case displayOption.display1:
                case displayOption.display2:
                case displayOption.display3:
                case displayOption.display4:
                case displayOption.primary:
                    targetArea = getTargetArea(displayMode);
                    moveWindow = true;
                    break;
            }

            if (moveWindow)
            {
                SetWindowPos(handle, HWND_TOP, targetArea.Left, targetArea.Top, targetArea.Width, targetArea.Height,
                             SetWindowPosFlags.SWP_SHOWWINDOW);
            }
        }

        public static void showIndications()
        {
            //displayIndicator di1 = new displayIndicator("", displayOption.display1);
            //displayIndicator di2 = new displayIndicator("", displayOption.display2);
            //displayIndicator di3 = new displayIndicator("", displayOption.display3);
            //displayIndicator di4 = new displayIndicator("", displayOption.display4);
        }

        /// <summary>
        /// Postavlja Display mode eksternom procesu
        /// </summary>
        /// <param name="displayMode"></param>
        /// <param name="target"></param>
        public static void deployDisplayModeToProcess(displayOption displayMode, Process target)
        {
            try
            {
                deployDisplayMode(displayMode, target.Handle);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Postavlja prozor u skladu sa podesavanjima
        /// </summary>
        /// <param name="displayMode"></param> devNotesView =
        /// <param name="target"></param>
        public static void deployDisplayMode(displayOption displayMode, Window target, Boolean dialogMode = false)
        {
            try
            {
                if (dialogMode)
                {
                    target.ShowDialog();
                    return;
                }
                else
                {
                    target.Show();
                }
            }
            catch (Exception ex)
            {
                var isb = imbStringBuilder.start("deployDisplayMode() failed :: " + ex.Message);
                isb.AppendPair("Display mode: ", displayMode.toStringSafe());
                isb.AppendPair("Dialog mode: ", dialogMode.ToString());
                isb.AppendPair("Window title: ", target.Title);
                isb.AppendPair("Window type: ", target.getTypeSignature());

                //     isb.AppendPair("Exception call stack: ", ex.StackTrace);

                //ex.makeMessageFromException("", reporting.enums.reportOutputFormat.htmlReport);

                //Exception exd = new aceGeneralException(isb.ToString(), ex);

                //devNoteManager.note(ex, "", )

                // logSystem.log(ex, "Display deploy mode problem: ", logType.CriticalWarning);
            }

            switch (displayMode)
            {
                default:
                case displayOption.defaultDisplay:
                    target.WindowState = WindowState.Normal;
                    break;

                case displayOption.hidden:
                    try
                    {
                        target.Hide();
                    }
                    catch
                    {
                    }
                    break;

                case displayOption.maximize:
                    target.WindowState = WindowState.Maximized;
                    break;

                case displayOption.minimize:
                    target.WindowState = WindowState.Minimized;
                    break;
            }

            target.MaxWidth = imbSettingsManager.primaryDisplay.WorkingArea.Width;
            target.MaxHeight = imbSettingsManager.primaryDisplay.WorkingArea.Height;

            WindowInteropHelper winHelp = new WindowInteropHelper(target);
            deployDisplayMode(displayMode, (IntPtr) winHelp.Handle);
        }

        #region Nested type: HWND

        /// <summary>
        /// Window handles (HWND) used for hWndInsertAfter
        /// </summary>
        internal static class HWND
        {
            public static IntPtr
                NoTopMost = new IntPtr(-2),
                TopMost = new IntPtr(-1),
                Top = new IntPtr(0),
                Bottom = new IntPtr(1);
        }

        #endregion Nested type: HWND

        #region Nested type: SWP

        /// <summary>
        /// SetWindowPos Flags
        /// </summary>
        internal static class SWP
        {
            public static readonly int
                NOSIZE = 0x0001,
                NOMOVE = 0x0002,
                NOZORDER = 0x0004,
                NOREDRAW = 0x0008,
                NOACTIVATE = 0x0010,
                DRAWFRAME = 0x0020,
                FRAMECHANGED = 0x0020,
                SHOWWINDOW = 0x0040,
                HIDEWINDOW = 0x0080,
                NOCOPYBITS = 0x0100,
                NOOWNERZORDER = 0x0200,
                NOREPOSITION = 0x0200,
                NOSENDCHANGING = 0x0400,
                DEFERERASE = 0x2000,
                ASYNCWINDOWPOS = 0x4000;
        }

        #endregion Nested type: SWP

        #region Nested type: SetWindowPosFlags

        /// <summary>
        /// InterOP flags za Window poziciju
        /// </summary>
        [Flags]
        internal enum SetWindowPosFlags : uint
        {
            // ReSharper disable InconsistentNaming

            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,

            /// <summary>
            ///     Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            SWP_DEFERERASE = 0x2000,

            /// <summary>
            ///     Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            SWP_DRAWFRAME = 0x0020,

            /// <summary>
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            SWP_FRAMECHANGED = 0x0020,

            /// <summary>
            ///     Hides the window.
            /// </summary>
            SWP_HIDEWINDOW = 0x0080,

            /// <summary>
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOACTIVATE = 0x0010,

            /// <summary>
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            SWP_NOCOPYBITS = 0x0100,

            /// <summary>
            ///     Retains the current position (ignores X and Y parameters).
            /// </summary>
            SWP_NOMOVE = 0x0002,

            /// <summary>
            ///     Does not change the owner window's position in the Z order.
            /// </summary>
            SWP_NOOWNERZORDER = 0x0200,

            /// <summary>
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
            /// </summary>
            SWP_NOREDRAW = 0x0008,

            /// <summary>
            ///     Same as the SWP_NOOWNERZORDER flag.
            /// </summary>
            SWP_NOREPOSITION = 0x0200,

            /// <summary>
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            /// </summary>
            SWP_NOSENDCHANGING = 0x0400,

            /// <summary>
            ///     Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            SWP_NOSIZE = 0x0001,

            /// <summary>
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOZORDER = 0x0004,

            /// <summary>
            ///     Displays the window.
            /// </summary>
            SWP_SHOWWINDOW = 0x0040,

            // ReSharper restore InconsistentNaming
        }

        #endregion Nested type: SetWindowPosFlags

        #region Nested type: SpecialWindowHandles

        /// <summary>
        ///     Special window handles
        /// </summary>
        internal enum SpecialWindowHandles
        {
            // ReSharper disable InconsistentNaming
            /// <summary>
            ///     Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
            /// </summary>
            HWND_TOP = 0,

            /// <summary>
            ///     Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
            /// </summary>
            HWND_BOTTOM = 1,

            /// <summary>
            ///     Places the window at the top of the Z order.
            /// </summary>
            HWND_TOPMOST = -1,

            /// <summary>
            ///     Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
            /// </summary>
            HWND_NOTOPMOST = -2
            // ReSharper restore InconsistentNaming
        }

        #endregion Nested type: SpecialWindowHandles

    */
    }
}