// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartFilePathSelector.cs" company="imbVeles" >
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
using imbACE.Core.core.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Services.textBlocks.smart
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.input;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.terminal.dialogs;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System.IO;

    /// <summary>
    /// Smart section
    /// </summary>
    public class smartFilePathSelector : textInputMenuBase, IRefresh, IRead
    {
        #region --- files ------- objekat za pronalazenje fajlova

        private fileSystemList _files;

        /// <summary>
        /// objekat za pronalazenje fajlova
        /// </summary>
        public fileSystemList files
        {
            get
            {
                return _files;
            }
            set
            {
                _files = value;
                OnPropertyChanged("files");
            }
        }

        #endregion --- files ------- objekat za pronalazenje fajlova

        #region --- path ------- selected path

        private String _path;

        /// <summary>
        /// selected path
        /// </summary>
        protected String path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged("path");
            }
        }

        #endregion --- path ------- selected path

        #region --- filename ------- Fajl koji treba da icita

        private String _filename;

        /// <summary>
        /// Fajl koji treba da icita
        /// </summary>
        public String filename
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
                OnPropertyChanged("filename");
            }
        }

        #endregion --- filename ------- Fajl koji treba da icita

        #region --- extension ------- ekstenzija koju treba da selektuje

        private String _extension;

        /// <summary>
        /// ekstenzija koju treba da selektuje
        /// </summary>
        protected String extension
        {
            get
            {
                return _extension;
            }
            set
            {
                _extension = value;
                OnPropertyChanged("extension");
            }
        }

        #endregion --- extension ------- ekstenzija koju treba da selektuje

        #region --- selectMode ------- m

        private dialogSelectFileMode _selectMode = dialogSelectFileMode.selectFileToOpen;

        /// <summary>
        /// Bindable property
        /// </summary>
        protected dialogSelectFileMode selectMode
        {
            get
            {
                return _selectMode;
            }
            set
            {
                _selectMode = value;
                OnPropertyChanged("selectMode");
            }
        }

        #endregion --- selectMode ------- m

        #region --- doAllowFileOps ------- da li dozvoljava operacije nad fajlovima

        private Boolean _doAllowFileOps;

        /// <summary>
        /// da li dozvoljava operacije nad fajlovima
        /// </summary>
        public Boolean doAllowFileOps
        {
            get
            {
                return _doAllowFileOps;
            }
            set
            {
                _doAllowFileOps = value;
                OnPropertyChanged("doAllowFileOps");
            }
        }

        #endregion --- doAllowFileOps ------- da li dozvoljava operacije nad fajlovima

        public smartFilePathSelector(String __path, dialogSelectFileMode __mode, String __ext, int _height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0) : base(new aceMenu(), _height, __width, __leftRightMargin, __leftRightPadding)
        {
            path = __path;
            selectMode = __mode;
            extension = __ext;
            margin.top = 1;
            margin.bottom = 2;
            pageManager = new textPageManager<aceMenuItem>();
            refresh();
        }

        /// <summary>
        /// Izvrsava se svaki put kad treba azurirati strukturu sadrzaja prema DataModel izvoru
        /// </summary>
        public void refresh()
        {
            if (String.IsNullOrEmpty(path)) path = "";

            if (File.Exists(path))
            {
                filename = Path.GetFileName(path);
                path = Path.GetDirectoryName(path);
            }

            files = new fileSystemList(path, selectMode, extension);

            menu = new aceMenu();

            switch (selectMode)
            {
                case dialogSelectFileMode.selectPath:
                    menu.menuTitle = "Select directory path";
                    //items.AddRange(Directory.EnumerateDirectories(path));
                    break;

                case dialogSelectFileMode.selectFileToOpen:
                    menu.menuTitle = "Select file to load (".add(files.pattern, " ").add(")", " ");
                    //items.AddRange(Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly));
                    //items.AddRange(Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly));
                    break;

                case dialogSelectFileMode.selectFileToSave:
                    menu.menuTitle = "Select file or type filename (*".add(extension, ".") + ")";
                    //items.AddRange(Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly));
                    //items.AddRange(Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly));
                    break;
            }

            foreach (var di in files.directories)
            {
                String nm = "";
                aceMenuItem menuItem = null;
                if (files.rootDirectory.Parent != null)
                {
                    if (di.FullName == files.rootDirectory.Parent.FullName)
                    {
                        nm = "..";
                        menuItem = new aceMenuItem(nm, "", "<dir>", "<lock>", di);
                        menuItem.helpLine = "Move to parent";
                    }
                }

                if (files.rootDirectory != null)
                {
                    if (di.FullName == files.rootDirectory.FullName)
                    {
                        nm = ".";
                        menuItem = new aceMenuItem(nm, "", "<dir>", "<lock>", di);
                        menuItem.helpLine = "This directory";
                    }
                }

                if (String.IsNullOrEmpty(nm))
                {
                    nm = di.FullName.Replace(files.rootDirectory.FullName, "");
                    menuItem = new aceMenuItem(nm, "", "<dir>", "<lock>", di);
                    menuItem.helpLine = di.FullName;
                }

                menu.setItem(menuItem);

                //if (path == di.FullName)
                //{
                //    menu.selected = menuItem;
                //}
            }

            foreach (var di in files.files)
            {
                aceMenuItem menuItem = new aceMenuItem(di.Name, "", di.LastWriteTime.ToShortDateString(), "", di);
                menuItem.helpLine = di.FullName;
                menu.setItem(menuItem);

                //if (path == di.FullName)
                //{
                //    menu.selected = menuItem;
                //}
            }

            pageManager = new textPageManager<aceMenuItem>(10);
            pageManager.refresh(menu);
        }

        /// <summary>
        /// Izvrsava se neposredno pre renderinga
        /// </summary>
        public override void resetContent()
        {
            base.resetContent();
            setupFieldFormat("{0}", 10, 30);
            cursor.moveToCorner(textCursorZoneCorner.UpLeft);

            if (!String.IsNullOrEmpty(menu.menuTitle))
            {
                writeField("Directory: ", printHorizontal.left);
                writeField(files.rootDirectory.FullName, printHorizontal.middle);
                insertSplitLine();
            }

            #region PAGINATED CONTENT DISPLAY

            var items = pageManager.getPageElements(menu);
            Int32 c = 0;
            foreach (aceMenuItem it in items)
            {
                var id = renderSelectBox(it, menu.isDisabled(it), menu.isSelected(it), menu.isDefault(it)).add(it.itemName);
                FileSystemInfo fsi = it.metaObject as FileSystemInfo;
                writeField(id, printHorizontal.left);
                if (fsi != null)
                {
                    writeField(it.itemRemarkEnabled, printHorizontal.middle);
                    writeField(fsi.CreationTime.ToShortDateString().add(fsi.CreationTime.ToShortTimeString(), ":"), printHorizontal.right);
                }

                c++;
                cursor.nextLine();
            }
            cursor.nextLine(pageManager.pageCapacaty - c);

            #endregion PAGINATED CONTENT DISPLAY

            insertSplitLine();

            if (doAllowFileOps)
            {
                writeLine("[F3] View | [F4] Edit | [F5] Duplicate | [F6] Rename | [F7] New directory | [F8] Delete", -1, false);
            }

            // insertLine("[HOME] First page | [END] Last page");

            writeLine("[UP][DOWN] Select | [HOME][END][PgUp][PgDn] Page selection | Page ".add(pageManager.getPageString()), -1, false);
            writeLine("[ESC] Cancel | [ENTER] Select | [TAB] Modify filename", -1, false);

            writeLine("File [".add(filename, " ") + "]", -1, false);
            writeLine("at [".add(path) + "]", -1, false);

            cutSectionAtCursor();
        }

        /// <summary>
        /// Primena procitanog unosa
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="__currentOutput"></param>
        /// <returns></returns>
        public override textInputResult applyReading(IPlatform platform, textInputResult __currentOutput)
        {
            FileSystemInfo fsi = null;
            if (menu.selected != null)
            {
                fsi = menu.selected.metaObject as FileSystemInfo;
            }

            switch (currentOutput.consoleKey.Key)
            {
                case ConsoleKey.Enter:
                    if (fsi != null)
                    {
                        if (fsi is DirectoryInfo)
                        {
                            path = fsi.FullName;
                            refresh();
                            currentOutput.doKeepReading = true;
                        }
                        else
                        {
                            path = fsi.FullName;
                            filename = Path.GetFileName(fsi.FullName);
                            currentOutput.doKeepReading = false;
                        }
                    }
                    // closeAndSaveChanges();

                    break;

                case ConsoleKey.Backspace:
                    if (path.Length > 1) path.PadRight(1);
                    // closeAndSaveChanges();
                    currentOutput.doKeepReading = false;
                    break;

                case ConsoleKey.Tab:
                    //platform.read(currentOutput, inputReadMode.readLine,
                    break;

                case ConsoleKey.PageDown:
                    pageManager.selectNext();
                    //menu.selected = menu[pageManager.currentPageStartIndex];
                    break;

                case ConsoleKey.PageUp:
                    pageManager.selectPrev();
                    break;

                case ConsoleKey.Escape:
                    //menu.selected = null;
                    //    closeAndDefault();
                    currentOutput.doKeepReading = false;
                    break;

                case ConsoleKey.Home:
                    pageManager.selectFirst();
                    break;

                case ConsoleKey.End:
                    pageManager.selectLast();
                    break;

                //case ConsoleKey.F5:
                //    break;
                //case ConsoleKey.F2:
                //    break;
                //case ConsoleKey.F12:
                //    break;
                //case ConsoleKey.LeftArrow:
                //case ConsoleKey.RightArrow:
                //case ConsoleKey.OemPlus:
                //case ConsoleKey.Spacebar:
                //case ConsoleKey.OemMinus:
                //    changeValue(spec, currentOutput.consoleKey);
                //    break;

                case ConsoleKey.UpArrow:
                    menu.selectPrev();
                    break;

                case ConsoleKey.DownArrow:
                    menu.selectNext();
                    break;

                default:

                    break;
            }

            if (menu.selected != null)
            {
                fsi = menu.selected.metaObject as FileSystemInfo;
            }
            if (fsi != null)
            {
                path = fsi.FullName;

                currentOutput.result = fsi;
                currentOutput.section = this;
            }

            return currentOutput;
        }
    }
}