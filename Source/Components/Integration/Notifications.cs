//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2011
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
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Modules.Boards.Components.Common;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Social.Notifications;

namespace DotNetNuke.Modules.Boards.Components.Integration
{
    public class Notifications
    {

        ///// <summary>
        ///// This method will send a core notification to idea moderators when a new idea is pending publishing approval.
        ///// </summary>
        ///// <param name="objIdea"></param>
        ///// <param name="portalId"></param>
        ///// <param name="summary"></param>
        ///// <param name="title"></param>
        ///// <param name="objModule"></param>
        //internal void IdeaPendingApproval(IdeaInfo objIdea, int portalId, string summary, string title, ModuleInfo objModule)
        //{
        //    var notificationType = NotificationsController.Instance.GetNotificationType(Constants.NotificationIdeaTypeName);
        //    var notificationKey = string.Format("{0}:{1}:{2}", Constants.ContentTypeName, objIdea.IdeaId, objIdea.ModuleID);

        //    var objNotification = new Notification
        //    {
        //        NotificationTypeID = notificationType.NotificationTypeId,
        //        Subject = title,
        //        Body = summary,
        //        IncludeDismissAction = false,
        //        SenderUserID = objIdea.CreatedByUserID,
        //        Context = notificationKey
        //    };

        //    var permc = new PermissionController();
        //    var rc = new RoleController();
        //    var colRoles = new List<RoleInfo>();
        //    var colUsers = new List<UserInfo>();

        //    foreach (PermissionInfo pi in permc.GetPermissionByCodeAndKey(Constants.PermissionCode, Constants.PermissionKey))
        //    {
        //        foreach (ModulePermissionInfo mpi in objModule.ModulePermissions)
        //        {
        //            if (mpi.PermissionID != pi.PermissionID) continue;
        //            if (mpi.RoleID >= 0)
        //            {
        //                var objRole = rc.GetRole(mpi.RoleID, portalId);
        //                colRoles.Add(objRole);
        //            }

        //            if (mpi.UserID > 0)
        //            {
        //                var objUser = UserController.GetUserById(portalId, mpi.UserID);
        //                colUsers.Add(objUser);
        //            }
        //        }
        //    }

        //    if ((colUsers.Count > 0) || (colRoles.Count > 0))
        //    {
        //        NotificationsController.Instance.SendNotification(objNotification, portalId, colRoles, colUsers);
        //    }
        //}

        ///// <summary>
        ///// Removes any notifications associated w/ a specific idea pending approval.
        ///// </summary>
        ///// <param name="ideaId"></param>
        ///// <param name="moduleId"></param>
        ///// <remarks></remarks>
        //internal void RemoveIdeaPendingNotification(int ideaId, int moduleId)
        //{
        //    var notificationType = NotificationsController.Instance.GetNotificationType(Constants.NotificationIdeaTypeName);
        //    var notificationKey = string.Format("{0}:{1}:{2}", Constants.ContentTypeName, ideaId, moduleId);

        //    var objNotify = NotificationsController.Instance.GetNotificationByContext(notificationType.NotificationTypeId, notificationKey).SingleOrDefault();

        //    if (objNotify != null)
        //    {
        //        NotificationsController.Instance.DeleteAllNotificationRecipients(objNotify.NotificationID);
        //    }
        //}

        #region Install Methods

        /// <summary>
        /// This will create a notification type associated w/ the module and also handle the actions that must be associated with it.
        /// </summary>
        internal static void AddNotificationTypes()
        {
            var actions = new List<NotificationTypeAction>();
            var deskModuleId = DesktopModuleController.GetDesktopModuleByFriendlyName("Boards").DesktopModuleID;

            var objNotificationType = new NotificationType
            {
                Name = Constants.NotificationBoardsAssignedTypeName,
                Description = "Boards: Task Assigned",
                DesktopModuleId = deskModuleId
            };

            if (NotificationsController.Instance.GetNotificationType(objNotificationType.Name) == null)
            {
                var objAction = new NotificationTypeAction
                {
                    NameResourceKey = "MarkDone",
                    DescriptionResourceKey = "MarkDone_Desc",
                    APICall = "DesktopModules/DNNCorp/Boards/API/NotificationService.ashx/MarkDone",
                    Order = 1
                };
                actions.Add(objAction);

                objAction = new NotificationTypeAction
                {
                    NameResourceKey = "RejectAssignement",
                    DescriptionResourceKey = "RejectAssignement_Desc",
                    APICall = "DesktopModules/DNNCorp/Boards/API/NotificationService.ashx/RejectAssignement",
                    ConfirmResourceKey = "RejectAssignementConfirm",
                    Order = 3
                };
                actions.Add(objAction);

                NotificationsController.Instance.CreateNotificationType(objNotificationType);
                NotificationsController.Instance.SetNotificationTypeActions(actions, objNotificationType.NotificationTypeId);
            }
        }

        #endregion

    }
}