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

using System.Data;
using System;

namespace DotNetNuke.Modules.Boards.Providers.Data
{

    /// <summary>
    /// An abstract class for the data access layer
    /// </summary>
    public interface IDataProvider {

        #region Abstract methods

        #region Board

        int CreateBoard(string name, string description, int portalId, int organizerId, int groupId, int moduleId, int createdByUserId);

        IDataReader GetBoard(int boardId);

        IDataReader GetGroupBoards(int groupId);

        IDataReader GetModuleBoards(int moduleId);

        IDataReader GetUserBoards(int userId, int portalId);

        void UpdateBoard(int boardId, string name, string description, int portalId, int organizerId, int groupId, int moduleId, int lastModifieddByUserId, DateTime lastModifiedOnDate);

        void DeleteBoard(int boardId, int portalId);    

        #endregion

        #region BoardList

        int CreateBoardList(string name, int boardId);

        IDataReader GetBoardList(int boardListId);

        IDataReader GetBoardLists(int boardId);

        void UpdateBoardList(int boardListId, string name, int boardId, bool archived, int sortOrder);

        void DeleteBoardList(int boardListId, int boardId);

        #endregion

        #region Card

        int CreateCard(int contentItemId, DateTime dueDate, int boardListId, string labels, string members);

        IDataReader GetCard(int cardId);

        IDataReader GetBoardCards(int boardId);

        IDataReader GetBoardListCards(int boardListId);

        void UpdateCard(int cardId, int contentItemId, bool archived, DateTime dueDate, int boardListId, int sortOrder, string labels, string members);

        void DeleteCard(int cardId, int boardListId);

        #endregion

        #region CardItem

        int CreateCardItem(int cardId, string item, string itemGroup);

        IDataReader GetCardItem(int cardItemId);

        IDataReader GetCardItems(int cardId);

        void UpdateCardItem(int cardItemId, int cardId, string item, string itemGroup, bool completed, int sortOrder, bool archived);

        void DeleteCardItem(int cardItemId, int cardId);

        #endregion

        #endregion

    }
}