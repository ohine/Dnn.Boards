//
// DotNetNuke® - http://www.dotnetnuke.com
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
using DotNetNuke.Framework;
using DotNetNuke.Modules.Boards.Components.Common;
using DotNetNuke.Modules.Boards.Components.Controllers;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;
using System.Linq;
using DotNetNuke.Web.Client.ClientResourceManagement;

namespace DotNetNuke.Modules.Boards
{

    /// <summary>
    /// The Board control is the user interface for the module when it is placed on typical DNN pages and is seen when these pages are initially viewed. 
    /// </summary>
    public partial class Board : BoardsModuleBase
    {

        /// <summary>
        /// 
        /// </summary>
        public int BoardId
        {
           get
           {
               var cntBoards = new BoardsController();
               var colBoards = cntBoards.GetModuleBoards(ModuleContext.ModuleId);

               if (colBoards.Count > 0)
               {
                   // (we are temporarily going to allow only a single board per moduleid in this view so assume first result is good)
                   var objBoard = colBoards.SingleOrDefault();

                   if (objBoard !=null)
                   {
                       return objBoard.BoardId;
                   }
               }
               if (ModuleContext.PortalSettings.UserId > 0)
               {
                   return CreateBoard();
               }
               return -1;
           }
        }

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ClientResourceManager.RegisterScript(Page, TemplateSourceDirectory + "/js/knockout-sortable.min.js");
            ClientResourceManager.RegisterScript(Page, TemplateSourceDirectory + "/js/Board.js");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ServicesFramework.Instance.RequestAjaxAntiForgerySupport();

            if (BoardId < 1)
            {
                UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("NoBoards", LocalResourceFile), ModuleMessage.ModuleMessageType.BlueInfo);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new board and default lists for the newly created board.
        /// </summary>
        /// <returns></returns>
        private int CreateBoard()
        {
            var cntBoards = new BoardsController();
            var objBoard = new Components.Entities.Board
            {
                Name =
                    Localization.GetString("DefaultBoardName", Constants.SharedResourceFile),
                Description =
                    Localization.GetString("DefaultBoardName", Constants.SharedResourceFile),
                PortalId = ModuleContext.PortalId,
                OrganizerId = ModuleContext.PortalSettings.UserId,
                ModuleId = ModuleContext.ModuleId,
                CreatedByUserId = ModuleContext.PortalSettings.UserId
            };

            // In 'group mode', the board will be associated w/ the specific group
            if (GroupId > -1)
            {
                objBoard.GroupId = GroupId;
            }

            objBoard.BoardId = cntBoards.CreateBoard(objBoard);

            Utils.CreateDefaultBoardLists(objBoard);

            return objBoard.BoardId;
        }

        #endregion

    }
}