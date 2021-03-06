//
// DotNetNukeŽ - http://www.dotnetnuke.com
// Copyright (c) 2002-2012
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Modules.Boards.Components.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Client.ClientResourceManagement;

namespace DotNetNuke.Modules.Boards
{
    using DotNetNuke.UI.Modules;

    /// <summary>
    /// This dispatch control determines which user interface to display based on page the module is placed on in addition to some various URL parameters.
    /// </summary>
    public partial class Dispatch : BoardsModuleBase
    {

        #region Private Members

        private const string CtlBoard = "/Board.ascx";
        private const string CtlProfile = "/MyTasks.ascx";
        //private const string CtlShared = "/SharedBoard.ascx";

        public string ControlToLoad { get; set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            jQuery.RequestDnnPluginsRegistration();
            ClientResourceManager.RegisterScript(Page, "~/Resources/Shared/scripts/knockout.js", 90);
            ClientResourceManager.RegisterScript(Page, "~/DesktopModules/DNNCorp/Boards/js/ServiceCaller.js");

            if ((ModuleContext.PortalSettings.ActiveTab.ParentId == ModuleContext.PortalSettings.UserTabId) || (ModuleContext.TabId == ModuleContext.PortalSettings.UserTabId))
            {
                // profile mode
                ControlToLoad = CtlProfile;
            }
            else
            {
                //if (GroupId > -1)
                //{
                //    ControlToLoad = CtlShared;
                //}
                //else
                //{
                //    ControlToLoad = CtlBoard;
                //}
                ControlToLoad = CtlBoard;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                var pathToControl = TemplateSourceDirectory + ControlToLoad;

                var objControl = LoadControl(pathToControl) as PortalModuleBase;
                if (objControl == null)
                {
                    var objUserControl = LoadControl(pathToControl) as ModuleUserControlBase;

                    if (objUserControl == null) return;

                    phUserControl.Controls.Clear();
                    objUserControl.ModuleContext.Configuration = ModuleContext.Configuration;
                    objUserControl.ID = System.IO.Path.GetFileNameWithoutExtension(pathToControl);
                    phUserControl.Controls.Add(objUserControl);
                }
                else
                {
                    phUserControl.Controls.Clear();
                    objControl.ModuleContext.Configuration = ModuleContext.Configuration;
                    objControl.ID = System.IO.Path.GetFileNameWithoutExtension(pathToControl);
                    phUserControl.Controls.Add(objControl);
                }

                if ((string)ViewState["CtlToLoad"] != ControlToLoad)
                {
                    ViewState["CtlToLoad"] = ControlToLoad;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

    }
}