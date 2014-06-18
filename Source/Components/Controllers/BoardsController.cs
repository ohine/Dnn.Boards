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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Boards.Components.Entities;
using DotNetNuke.Modules.Boards.Providers.Data;
using DotNetNuke.Modules.Boards.Providers.Data.SqlDataProvider;

namespace DotNetNuke.Modules.Boards.Components.Controllers
{

	public class BoardsController 
	{

		private readonly IDataProvider _dataProvider;

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public BoardsController() {
			_dataProvider = ComponentModel.ComponentFactory.GetComponent<IDataProvider>();
			if (_dataProvider != null) return;

			// get the provider configuration based on the type
			var defaultprovider = Data.DataProvider.Instance().DefaultProviderName;
			const string dataProviderNamespace = "DotNetNuke.Modules.Boards.Providers.Data";

			if (defaultprovider == "SqlDataProvider") {
				_dataProvider = new SqlDataProvider();
			} else {
				var providerType = dataProviderNamespace + "." + defaultprovider;
				_dataProvider = (IDataProvider)Framework.Reflection.CreateObject(providerType, providerType, true);
			}

			ComponentModel.ComponentFactory.RegisterComponentInstance<IDataProvider>(_dataProvider);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataProvider"></param>
		public BoardsController(IDataProvider dataProvider)
		{
			//DotNetNuke.Common.Requires.NotNull("dataProvider", dataProvider);
			_dataProvider = dataProvider;
		}

		#endregion

		#region Public Methods

		#region Board

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objBoard"></param>
		/// <returns></returns>
		/// <remarks>This should implement caching.</remarks>
		public int CreateBoard(Entities.Board objBoard)
		{
			return _dataProvider.CreateBoard(objBoard.Name, objBoard.Description, objBoard.PortalId, objBoard.OrganizerId, objBoard.GroupId, objBoard.ModuleId, objBoard.CreatedByUserId);
		}

		public Entities.Board GetBoard(int boardId)
		{
			return CBO.FillObject<Entities.Board>(_dataProvider.GetBoard(boardId));
		}

		public List<Entities.Board> GetGroupBoards(int groupId)
		{
			return CBO.FillCollection<Entities.Board>(_dataProvider.GetGroupBoards(groupId));
		}

		public List<Entities.Board> GetModuleBoards(int moduleId)
		{
			return CBO.FillCollection<Entities.Board>(_dataProvider.GetModuleBoards(moduleId));
		}

		public List<Entities.Board> GetUserBoards(int userId, int portalId)
		{
			return CBO.FillCollection<Entities.Board>(_dataProvider.GetUserBoards(userId, portalId));
		}

		public void UpdateBoard(Entities.Board objBoard, int tabId)
		{
			_dataProvider.UpdateBoard(objBoard.BoardId, objBoard.Name, objBoard.Description, objBoard.PortalId, objBoard.OrganizerId, objBoard.GroupId, objBoard.ModuleId, objBoard.LastModifiedByUserId, objBoard.LastModifiedOnDate);
		}

		public void DeleteBoard(int boardId, int portalId)
		{
			_dataProvider.DeleteBoard(boardId, portalId); 
		}

		#endregion

		#region Board List

		public int CreateBoardList(BoardList objBoardList)
		{
			return _dataProvider.CreateBoardList(objBoardList.Name, objBoardList.BoardId);
		}

		public BoardList GetBoardList(int boardListId)
		{
			return CBO.FillObject<BoardList>(_dataProvider.GetBoardList(boardListId));
		}

		public List<BoardList> GetBoardLists(int boardId)
		{
			return CBO.FillCollection<BoardList>(_dataProvider.GetBoardLists(boardId));
		}

		public void UpdateBoardList(BoardList objBoardList, int tabId)
		{
			_dataProvider.UpdateBoardList(objBoardList.BoardListId, objBoardList.Name, objBoardList.BoardId, objBoardList.Archived, objBoardList.SortOrder);
		}

		public void DeleteBoardList(int boardListId, int boardId)
		{
			_dataProvider.DeleteBoardList(boardListId, boardId);
		}

		#endregion

		#region Card

		public int CreateCard(Card objCard)
		{
			var cntIntegration = new Integration.Content();
			var objContent = cntIntegration.CreateContentItem(objCard);

			if (objContent != null)
			{
				objCard.ContentItemId = objContent.ContentItemId;

				return _dataProvider.CreateCard(objCard.ContentItemId, objCard.DueDate, objCard.BoardListId, objCard.Labels, objCard.Members);
			}
			return -1;
		}

		public Card GetCard(int cardId)
		{
			return CBO.FillObject<Card>(_dataProvider.GetCard(cardId));
		}

		public List<Card> GetBoardCards(int boardId)
		{
			return CBO.FillCollection<Card>(_dataProvider.GetBoardCards(boardId));
		}

		public List<Card> GetBoardListCards(int boardListId)
		{
			return CBO.FillCollection<Card>(_dataProvider.GetBoardListCards(boardListId));
		}

        public void UpdateCard(Card objCard)
        {
            var cntIntegration = new Integration.Content();
            cntIntegration.UpdateContentItem(objCard);

            _dataProvider.UpdateCard(objCard.CardId, objCard.ContentItemId, objCard.Archived, objCard.DueDate, objCard.BoardListId, objCard.SortOrder, objCard.Labels, objCard.Members);
        }

		public void DeleteCard(int cardId, int boardListId, int contentItemId)
		{
			var cntContent = new Integration.Content();
			cntContent.DeleteContentItem(contentItemId);

			_dataProvider.DeleteCard(cardId, boardListId);
		}

		#endregion

		#region Card Item

		public int CreateCardItem(CardItem objCardItem)
		{
			return _dataProvider.CreateCardItem(objCardItem.CardId, objCardItem.Item, objCardItem.ItemGroup);
		}

		public CardItem GetCardItem(int cardIdItemId)
		{
			return CBO.FillObject<CardItem>(_dataProvider.GetCardItem(cardIdItemId));
		}

		public List<CardItem> GetCardItems(int cardId)
		{
			return CBO.FillCollection<CardItem>(_dataProvider.GetCardItems(cardId));
		}

		public void UpdateCardItem(CardItem objCardItem, int tabId)
		{
			_dataProvider.UpdateCardItem(objCardItem.CardItemId, objCardItem.CardId, objCardItem.Item, objCardItem.ItemGroup, objCardItem.Completed, objCardItem.SortOrder, objCardItem.Archived);
		}

		public void DeleteCardItem(int cardItemId, int cardId)
		{
			_dataProvider.DeleteCardItem(cardItemId, cardId);
		}

		#endregion

		#endregion

	}
}