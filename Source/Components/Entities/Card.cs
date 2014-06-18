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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace DotNetNuke.Modules.Boards.Components.Entities
{

    /// <summary>
    /// This will represents the card table and content item table combined (since we inherit from the base ContentItem class). 
    /// </summary>
    public class Card : ContentItem
    {

        public int CardId { get; set; }

        public bool Archived { get; set; }

        public DateTime DueDate { get; set; }

        public int BoardListId { get; set; }

        public int SortOrder { get; set; }

        public int Comments { get; set; }

        public int OpenItems { get; set; }

        public int CompletedItems { get; set; }

        /// <summary>
        /// Array
        /// </summary>
        /// <remarks>50 chars</remarks>
        public string Labels { get; set; }

        /// <summary>
        /// Array
        /// </summary>
        /// <remarks>50 chars</remarks>
        public string Members { get; set; }

        public string Title { get; set; }

        public int Id
        {
            get { return ContentItemId; }
        }

        #region IHydratable Implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public override void Fill(System.Data.IDataReader dr)
        {
            //Call the base classes fill method to populate base class proeprties
            base.FillInternal(dr);

            CardId = Null.SetNullInteger(dr["CardId"]);
            Archived = Null.SetNullBoolean(dr["Archived"]);
            DueDate = Null.SetNullDateTime(dr["DueDate"]);
            BoardListId = Null.SetNullInteger(dr["BoardListId"]);
            SortOrder = Null.SetNullInteger(dr["SortOrder"]);
            Comments = Null.SetNullInteger(dr["Comments"]);
            OpenItems = Null.SetNullInteger(dr["OpenItems"]);
            CompletedItems = Null.SetNullInteger(dr["CompletedItems"]);
            Labels = Null.SetNullString(dr["Labels"]);
            Members = Null.SetNullString(dr["Members"]);
            Title = Null.SetNullString(dr["Title"]);
        }

        /// <summary>
        /// Gets and sets the Key ID
        /// </summary>
        /// <returns>An Integer</returns>
        public override int KeyID
        {
            get { return CardId; }
            set { CardId = value; }
        }

        #endregion

    }
}