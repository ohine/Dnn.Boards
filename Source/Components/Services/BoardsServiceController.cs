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
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Modules.Boards.Components.Common;
using DotNetNuke.Modules.Boards.Components.Controllers;
using DotNetNuke.Modules.Boards.Components.Entities;
using DotNetNuke.Modules.Boards.Components.Integration;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Api;

namespace DotNetNuke.Modules.Boards.Components.Services
{

    /// <summary>
    /// The methods in this class are exposed via the services framework for use in the boards module.
    /// </summary>
    /// <remarks>This class will need updated when updating to DotNetNuke 7, due to changes in the Services Framework.</remarks>
    public class BoardsServiceController : DnnApiController
    {

        [DnnAuthorize]
        [HttpGet]
        public HttpResponseMessage GetBoardLists(int boardId)
        {
            try
            {
                if (boardId > 0)
                {
                    var cntBoard = new BoardsController();
                    var colLists = cntBoard.GetBoardLists(boardId);

                    if (colLists.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, colLists });
                    }
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "There are no board lists setup." });
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "A board is not currently selected" });
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "An error occurred during board list retrieval. " + exc.Message });
            }
        }

        [DnnAuthorize]
        [HttpGet]
        public HttpResponseMessage CreateCard(int boardListId, string title)
        {
            try
            {
                var cntBoard = new BoardsController();
                var objCard = new Card
                    {
                        BoardListId = boardListId,
                        ContentTitle = title,
                        Title = title,
                        Content = "",
                        ModuleID = PortalSettings.ActiveTab.ModuleID,
                        TabID = PortalSettings.ActiveTab.TabID
                    };

                objCard.CardId = cntBoard.CreateCard(objCard);

                if (objCard.CardId > 0)
                {
                    var newCard = cntBoard.GetCard(objCard.CardId);

                    // journal integration
                    var cntIntegration = new Journal();
                    cntIntegration.AddTaskToJournal(objCard, objCard.Title, "", PortalSettings.PortalId, PortalSettings.UserId, DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", ""));

                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, newCard });
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "There is no currently selected card." });
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "An error occurred during card creation.  " + exc.Message });
            }
        }

        public class CardDeleteDto
        {
            public int CardId { get; set; }
            public int BoardListId { get; set; }
            public int ContentItemId { get; set; }
        }

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage DeleteCard(CardDeleteDto postData)
        {
            var success = false;
            try
            {
                if (postData.ContentItemId > 0)
                {
                    var cntBoard = new BoardsController();
                    cntBoard.DeleteCard(postData.CardId, postData.BoardListId, postData.ContentItemId);

                    // journal integration
                    var cntIntegration = new Journal();
                    cntIntegration.RemoveTaskFromJournal(postData.CardId, PortalSettings.ActiveTab.ModuleID, PortalSettings.PortalId);

                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "invalid data. "  });
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "An error occurred while trying to delete the card.  " + exc.Message });
            }
        }

        public class CardListDto
        {
            public int CardId { get; set; }
            public int BoardListId { get; set; }
            public int SortOrder { get; set; }
        }

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage UpdateCardList(CardListDto postData)
        {
            var success = false;
            try
            {
                var cntBoard = new BoardsController();
                var objCard = cntBoard.GetCard(postData.CardId);

                if (objCard != null)
                {
                    objCard.BoardListId = postData.BoardListId;
                    objCard.SortOrder = postData.SortOrder;

                    cntBoard.UpdateCard(objCard);
                    success = true;
                }
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
            }
            return success ? Request.CreateResponse(HttpStatusCode.OK, new { Result = "success" }) : Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "unable to change lists.");
        }

        public class CardOrderDto
        {
            public int CardId { get; set; }
            public int SortOrder { get; set; }
        }

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage UpdateCardOrder(CardOrderDto postData)
        {
            var success = false;
            try
            {
                var cntBoard = new BoardsController();
                var objCard = cntBoard.GetCard(postData.CardId);

                if (objCard != null)
                {
                    objCard.SortOrder = postData.SortOrder;

                    cntBoard.UpdateCard(objCard);

                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "Could not update card. " });
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "An error occurred updating the card.  " + exc.Message });
            }
        }

        public class CardDescriptionDto
        {
            public int CardId { get; set; }
            public string Content { get; set; }
        }

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage UpdateCardDescription(CardDescriptionDto postData)
        {
            var success = false;
            try
            {
                var cntBoard = new BoardsController();
                var objCard = cntBoard.GetCard(postData.CardId);

                if (objCard != null)
                {
                    objCard.Content = postData.Content;

                    cntBoard.UpdateCard(objCard);

                    // journal integration
                    var cntIntegration = new Journal();
                    cntIntegration.AddTaskUpdateToJournal(objCard, (int)Constants.ActionKey.DescriptionUpdate, objCard.Title, PortalSettings.PortalId, PortalSettings.UserId, DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", ""), Localization.GetString("DescriptionUpdate", Constants.SharedResourceFile));

                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "Could not update card. " });
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "An error occurred updating the card description. " + exc.Message });
            }
        }

        public class CardTitleDto
        {
            public int CardId { get; set; }
            public string Title { get; set; }
        }

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage UpdateCardTitle(CardTitleDto postData)
        {
            var success = false;
            try
            {
                var cntBoard = new BoardsController();
                var objCard = cntBoard.GetCard(postData.CardId);

                if (objCard != null)
                {
                    objCard.ContentTitle = postData.Title;
                    objCard.Title = postData.Title;

                    cntBoard.UpdateCard(objCard);


                    // journal integration
                    var cntIntegration = new Journal();
                    cntIntegration.AddTaskUpdateToJournal(objCard, (int)Constants.ActionKey.TitleUpdate, objCard.Title, PortalSettings.PortalId, PortalSettings.UserId, DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", ""), Localization.GetString("TitleUpdate", Constants.SharedResourceFile));

                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "Could not update card. " });
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, reason = "An error occurred updating the card title.  " + exc.Message });
            }
        }

    }
}