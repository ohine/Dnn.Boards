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

using System.Collections.Generic;
using DotNetNuke.Modules.Boards.Components.Controllers;
using DotNetNuke.Modules.Boards.Components.Entities;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Boards.Components.Common
{

    public class Utils
    {

        /// <summary>
        /// This method creates a set of 3 default boards, the board list names are localized based on the creator's (current user) profile language preference.
        /// </summary>
        /// <param name="objBoard"></param>
        /// <returns></returns>
        /// <remarks>This is can be altered to include additional (or fewer) boards.</remarks>
        public static IEnumerable<BoardList> CreateDefaultBoardLists(Entities.Board objBoard)
        {
            var cntBoards = new BoardsController();
            var colBoardLists = new List<BoardList>();

            var objBoardList = new BoardList
                                   {
                                       Name = Localization.GetString("ToDoList", Constants.SharedResourceFile),
                                       BoardId = objBoard.BoardId
                                   };

            objBoardList.BoardListId = cntBoards.CreateBoardList(objBoardList);
            colBoardLists.Add(objBoardList);

            objBoardList = new BoardList
                               {
                                   Name = Localization.GetString("NeedToDoList", Constants.SharedResourceFile),
                                   BoardId = objBoard.BoardId
                               };

            objBoardList.BoardListId = cntBoards.CreateBoardList(objBoardList);
            colBoardLists.Add(objBoardList);

            objBoardList = new BoardList
                               {
                                   Name = Localization.GetString("DoneList", Constants.SharedResourceFile),
                                   BoardId = objBoard.BoardId
                               };

            objBoardList.BoardListId = cntBoards.CreateBoardList(objBoardList);
            colBoardLists.Add(objBoardList);

            return colBoardLists;
        }

    }
}