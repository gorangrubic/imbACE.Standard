using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Services.terminal.dialogs
{
    using imbACE.Core;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.dialogs.core;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using System.IO;

    /// <summary>
    /// Helper classes to get answers by temporary dialog instances
    /// </summary>
    public static class dialogs
    {
        /// <summary>
        /// Opens the dialog menu asking user to choose from options
        /// </summary>
        /// <typeparam name="T">Option type</typeparam>
        /// <param name="options">List of available options</param>
        /// <param name="title">The title to show</param>
        /// <param name="description">The description to show</param>
        /// <param name="style">The style to use for dialog box rendering</param>
        /// <param name="size">The size of the dialog box</param>
        /// <returns>Answer</returns>
        public static T openDialogWithOptions<T>(T[] options, String title = "", String description = "", dialogStyle style = dialogStyle.blueDialog, dialogSize size = dialogSize.mediumBox)
        {
            if (title == "") title = "Select option";
            if (description == "") description = "Please select one of the " + typeof(T).Name.imbTitleCamelOperation(true).ToLower() + "";
            dialogMessageBoxWithOptions<T> dialog = new dialogMessageBoxWithOptions<T>(aceCommons.platform, title, description, options);
            var response = dialog.open(aceCommons.platform, new dialogFormatSettings(style, size));

            return response.getResultObject<T>(options.First());
        }

        /// <summary>
        /// Opens dialog to edit properties of the instance
        /// </summary>
        /// <param name="instanceToEdit">The instance to edit properties of</param>
        /// <param name="title">Title to show</param>
        /// <param name="description">Description</param>
        /// <param name="style">The style.</param>
        /// <param name="size">The size.</param>
        /// <returns>Edited instance</returns>
        public static Object openEditProperties(Object instanceToEdit, String title = "", String description = "", dialogStyle style = dialogStyle.redDialog, dialogSize size = dialogSize.fullScreenBox)
        {
            dialogEditProperties editDialog = new dialogEditProperties(aceCommons.platform, instanceToEdit, title, description);
            var response = editDialog.open(aceCommons.platform, new dialogFormatSettings(style, size));

            return editDialog.editor.getObject();
        }

        /// <summary>
        /// Opens dialog to select file of extension
        /// </summary>
        /// <param name="__mode">Dialog mode</param>
        /// <param name="extension">The extension to filter files to</param>
        /// <param name="startPath">Initial file or directory to start selection from</param>
        /// <param name="comment">Aditional explanation</param>
        /// <param name="style">The style.</param>
        /// <param name="size">The size.</param>
        /// <returns>Selected path</returns>
        public static String openSelectFile(dialogSelectFileMode __mode, String extension = "*.*", String startPath = "", String comment = "", dialogStyle style = dialogStyle.redDialog, dialogSize size = dialogSize.fullScreenBox)
        {
            if (startPath == "") startPath = appManager.Application.folder.path;
            var format = new dialogFormatSettings(style, size);

            dialogSelectFile dialog = new dialogSelectFile(aceCommons.platform, startPath, __mode, extension, comment);

            inputResultCollection result = dialog.open(aceCommons.platform, format);

            FileSystemInfo defOutput;
            if (Path.HasExtension(startPath))
            {
                defOutput = new FileInfo(startPath);
            }
            else
            {
                defOutput = new DirectoryInfo(startPath);
            }

            defOutput = result.getResultObject<FileSystemInfo>(defOutput);

            return defOutput.FullName;
        }
    }
}