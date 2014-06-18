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
using DotNetNuke.Modules.Boards.Components.Common;
using DotNetNuke.Modules.Boards.Components.Entities;
using DotNetNuke.Services.Journal;

namespace DotNetNuke.Modules.Boards.Components.Integration
{
    public class Journal
    {

        #region Internal Methods

        /// <summary>
        /// Informs the core journal that the user has added a new task.
        /// </summary>
        /// <param name="objCard"></param>
        /// <param name="title"></param>
        /// <param name="voteId"></param>
        /// <param name="summary"></param>
        /// <param name="portalId"></param>
        /// <param name="journalUserId"></param>
        /// <param name="url"></param>
        internal void AddTaskToJournal(Card objCard, string title, string summary, int portalId, int journalUserId, string url)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalTaskAddName + "_" + string.Format("{0}:{1}", objCard.ModuleID, objCard.CardId);
            var ji = JournalController.Instance.GetJournalItemByKey(portalId, objectKey);

            if ((ji != null))
            {
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
            }

            ji = new JournalItem
                     {
                         PortalId = portalId,
                         ProfileId = journalUserId,
                         UserId = journalUserId,
                         ContentItemId = objCard.ContentItemId,
                         Title = title,
                         ItemData = new ItemData {Url = url},
                         Summary = summary,
                         Body = null,
                         JournalTypeId = GetTaskAddJournalTypeId(portalId),
                         ObjectKey = objectKey,
                         SecuritySet = "E,"
                     };

            JournalController.Instance.SaveJournalItem(ji, objCard.TabID);
        }

        /// <summary>
        /// Informs the core journal that we have to delete an item (task). 
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="moduleId"></param>
        /// <param name="portalId"></param>
        internal void RemoveTaskFromJournal(int cardId, int moduleId, int portalId)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalTaskAddName + "_" + string.Format("{0}:{1}", moduleId, cardId);
            JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
        }

        /// <summary>
        /// Informs the core journal that the user has updated a task (assigned it to someone, edited description or other associated data not available on creation, etc.)
        /// </summary>
        /// <param name="objCard"></param>
        /// <param name="actionKey"></param>
        /// <param name="title"></param>
        /// <param name="portalId"></param>
        /// <param name="journalUserId"></param>
        /// <param name="url"></param>
        /// <param name="summary"> </param>
        internal void AddTaskUpdateToJournal(Card objCard, int actionKey, string title, int portalId, int journalUserId, string url, string summary)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalTaskUpdateName + "_" + string.Format("{0}:{1}", objCard.CardId, actionKey);
            var ji = JournalController.Instance.GetJournalItemByKey(portalId, objectKey);

            if ((ji != null))
            {
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
            }

            ji = new JournalItem
            {
                PortalId = portalId,
                ProfileId = journalUserId,
                UserId = journalUserId,
                ContentItemId = objCard.ContentItemId,
                Title = title,
                ItemData = new ItemData { Url = url },
                Summary = summary,
                Body = null,
                JournalTypeId = GetTaskUpdateJournalTypeId(portalId),
                ObjectKey = objectKey,
                SecuritySet = "E,"
            };

            JournalController.Instance.SaveJournalItem(ji, objCard.TabID);
        }

        ///// <summary>
        ///// Deletes a journal item associated with the specific task update.
        ///// </summary>
        ///// <param name="cardId"></param>
        ///// <param name="actionKey"></param>
        ///// <param name="portalId"></param>
        //internal void RemoveTaskUpdateFromJournal(int cardId, int actionKey, int portalId)
        //{
        //    var objectKey = Constants.ContentTypeName + "_" + Constants.JournalTaskUpdateName + "_" + string.Format("{0}:{1}", cardId, actionKey);
        //    JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
        //}

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a journal type associated with adding a task.
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        private static int GetTaskAddJournalTypeId(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == Constants.JournalTaskAddName select t);
            int journalTypeId;

            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 28;
            }

            return journalTypeId;
        }

        /// <summary>
        /// Returns a journal type associated with commenting (using one of the core built in journal types)
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        private static int GetTaskUpdateJournalTypeId(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == Constants.JournalTaskUpdateName select t);
            int journalTypeId;

            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 29;
            }

            return journalTypeId;
        }

        #endregion

    }
}