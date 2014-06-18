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
using System.Data;
using DotNetNuke.Common.Utilities;
using Microsoft.ApplicationBlocks.Data;

namespace DotNetNuke.Modules.Boards.Providers.Data.SqlDataProvider
{

    /// <summary>
    /// 
    /// </summary>
    public class SqlDataProvider : IDataProvider
    {

        #region Private Members

        //private const string ProviderType = "data";
        private const string ModuleQualifier = "Boards_";
        private string _connectionString = String.Empty;
        private string _databaseOwner = String.Empty;
        private string _objectQualifier = String.Empty;

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return string.IsNullOrEmpty(_connectionString) ? DotNetNuke.Data.DataProvider.Instance().ConnectionString : _connectionString;
            }
            set { _connectionString = value; }
        }

        public string DatabaseOwner
        {
            get
            {
                return string.IsNullOrEmpty(_databaseOwner) ? DotNetNuke.Data.DataProvider.Instance().DatabaseOwner : _databaseOwner;
            }
            set { _databaseOwner = value; }
        }

        public string ObjectQualifier
        {
            get
            {
                return string.IsNullOrEmpty(_objectQualifier) ? DotNetNuke.Data.DataProvider.Instance().ObjectQualifier : _objectQualifier;
            }
            set { _objectQualifier = value; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object field)
        {
            return Null.GetNull(field, DBNull.Value);
        }

        private string GetFullyQualifiedName(string name)
        {
            return DatabaseOwner + ObjectQualifier + ModuleQualifier + name;
        }

        #endregion

        #region Public Methods

        #region Board

        public int CreateBoard(string name, string description, int portalId, int organizerId, int groupId, int moduleId, int createdByUserId)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Board_Create"), name, GetNull(description), portalId, organizerId, groupId, moduleId, createdByUserId));
        }

        public IDataReader GetBoard(int boardId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Board_Get"), boardId);
        }

        public IDataReader GetGroupBoards(int groupId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Board_GetGroup"), groupId);
        }

        public IDataReader GetModuleBoards(int moduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Board_GetModule"), moduleId);
        }

        public IDataReader GetUserBoards(int userId, int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Board_GetUser"), userId, portalId);
        }

        public void UpdateBoard(int boardId, string name, string description, int portalId, int organizerId, int groupId, int moduleId, int lastModifieddByUserId, DateTime lastModifiedOnDate)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Board_Update"), boardId, name, GetNull(description), portalId, organizerId, moduleId, GetNull(groupId), lastModifieddByUserId, lastModifiedOnDate);
        }

        public void DeleteBoard(int boardId, int portalId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Board_Delete"), boardId, portalId);
        }

        #endregion

        #region BoardList

        public int CreateBoardList(string name, int boardId)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Board_List_Create"), name, boardId));
        }

        public IDataReader GetBoardList(int boardListId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Board_List_Get"), boardListId);
        }

        public IDataReader GetBoardLists(int boardId)
        {
            return  SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Board_List_GetBoard"), boardId);
        }

        public void UpdateBoardList(int boardListId, string name, int boardId, bool archived, int sortOrder)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Board_List_Update"), boardListId, name, boardId, archived, sortOrder);
        }

        public void DeleteBoardList(int boardListId, int boardId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Board_List_Delete"), boardListId, boardId);
        }

        #endregion

        #region Card

        public int CreateCard(int contentItemId, DateTime dueDate, int boardListId, string labels, string members)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Card_Create"), contentItemId, GetNull(dueDate), boardListId, GetNull(labels), GetNull(members)));
        }

        public IDataReader GetCard(int cardId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Card_Get"), cardId);
        }

        public IDataReader GetBoardCards(int boardId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Card_GetBoard"), boardId);
        }

        public IDataReader GetBoardListCards(int boardListId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Card_GetList"), boardListId);
        }

        public void UpdateCard(int cardId, int contentItemId, bool archived, DateTime dueDate, int boardListId, int sortOrder, string labels, string members)
        {
            SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Card_Update"), cardId, contentItemId, archived, GetNull(dueDate), boardListId, sortOrder, labels, members);
        }

        public void DeleteCard(int cardId, int boardListId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Card_Delete"), cardId, boardListId);
        }

        #endregion

        #region CardItem

        public int CreateCardItem(int cardId, string item, string itemGroup)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Card_Item_Create"), cardId, item, GetNull(itemGroup)));
        }

        public IDataReader GetCardItem(int cardItemId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Card_Item_Get"), cardItemId);
        }

        public IDataReader GetCardItems(int cardId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Card_Item_GetCard"), cardId);
        }

        public void UpdateCardItem(int cardItemId, int cardId, string item, string itemGroup, bool completed, int sortOrder, bool archived)
        {
            SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Card_Item_Update"), cardItemId, cardId, item, GetNull(itemGroup), completed, sortOrder, archived);
        }

        public void DeleteCardItem(int cardItemId, int cardId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Card_Item_Delete"), cardItemId, cardId);
        }

        #endregion

        #endregion

    }
}