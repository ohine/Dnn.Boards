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

namespace DotNetNuke.Modules.Boards.Components.Entities
{

    public class BoardList
    {

        public int BoardListId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>50 chars</remarks>
        public string Name { get; set; }

        public int BoardId { get; set; }

        public bool Archived { get; set; }

        public int SortOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Card> Cards 
        {
            get 
            {
                if (BoardListId > 0)
                {
                    var cntBoard = new BoardsController();
                    return cntBoard.GetBoardListCards(BoardListId);
                }
                return null;
            }
        }

    }
}