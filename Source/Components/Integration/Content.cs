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

using System.Linq;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Content;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Boards.Components.Common;
using DotNetNuke.Modules.Boards.Components.Entities;

namespace DotNetNuke.Modules.Boards.Components.Integration
{

    /// <summary>
    /// This class contains all the interactions with the core API for content items.
    /// </summary>
	public class Content
	{

		/// <summary>
		/// Adds a content item in the core ContentItems table.
		/// </summary>
		/// <param name="objCard"></param>
		/// <returns></returns>
		internal ContentItem CreateContentItem(Card objCard)
		{
			var typeController = new ContentTypeController();
			var colContentTypes = (from t in typeController.GetContentTypes() where t.ContentType == Constants.ContentTypeName select t);
			int contentTypeID;

			if (colContentTypes.Any())
			{
				var contentType = colContentTypes.Single();
				contentTypeID = contentType == null ? CreateContentType() : contentType.ContentTypeId;
			}
			else
			{
				contentTypeID = CreateContentType();
			}

			var objContent = new ContentItem
								{
									Content = objCard.Content,
									ContentTypeId = contentTypeID,
									Indexed = false,
									ContentTitle = objCard.Title,
									ModuleID = objCard.ModuleID,
									TabID = objCard.TabID
								};

			objContent.ContentItemId = Util.GetContentController().AddContentItem(objContent);

			return objContent;
		}

		/// <summary>
		/// This is used to update the content in the ContentItems table. 
		/// </summary>
		internal void UpdateContentItem(Card objCard)
		{
			var objContent = Util.GetContentController().GetContentItem(objCard.ContentItemId);

			if (objContent == null) return;
			objContent.Content = objCard.Content;
			objContent.TabID = objCard.TabID;
		    objContent.ContentTitle = objCard.Title;

			Util.GetContentController().UpdateContentItem(objContent);
		}

		/// <summary>
		/// This removes a content item associated with a card from the data store. 
		/// </summary>
		/// <param name="contentItemID"></param>
		internal void DeleteContentItem(int contentItemID)
		{
			if (contentItemID <= Null.NullInteger) return;
			var objContent = Util.GetContentController().GetContentItem(contentItemID);
			if (objContent == null) return;

			// Remove Terms (if applicable)

			Util.GetContentController().DeleteContentItem(objContent);
		}

		/// <summary>
		/// This is used to determine the ContentTypeID (part of the Core API) based on this module's content type. If the content type doesn't exist yet for the module, it is created.
		/// </summary>
		/// <returns>The primary key value (ContentTypeID) from the core API's Content Types table.</returns>
		internal static int GetContentTypeID()
		{
			var typeController = new ContentTypeController();
			var colContentTypes = (from t in typeController.GetContentTypes() where t.ContentType == Constants.ContentTypeName select t);
			int contentTypeId;

			if (colContentTypes.Any())
			{
				var contentType = colContentTypes.Single();
				contentTypeId = contentType == null ? CreateContentType() : contentType.ContentTypeId;
			}
			else
			{
				contentTypeId = CreateContentType();
			}

			return contentTypeId;
		}

		#region Private Methods

		/// <summary>
		/// Creates a Content Type (necessary for contentItems and taxonomy) in the data store.
		/// </summary>
		/// <returns>The primary key value of the new ContentType.</returns>
		private static int CreateContentType()
		{
			var typeController = new ContentTypeController();
			var objContentType = new ContentType { ContentType = Constants.ContentTypeName };

			return typeController.AddContentType(objContentType);
		}

		#endregion

	}
}