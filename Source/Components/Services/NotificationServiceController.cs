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

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using DotNetNuke.Services.Social.Notifications;

namespace DotNetNuke.Modules.Boards.Components.Services
{

    public class NotificationServiceController : DnnApiController
	{

        public class NotificationDto
        {
            public int NotificationId { get; set; }
        }

		#region Private Members

        private int _cardId = -1;
        //private int _moduleId = -1;

		#endregion

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage MarkDone(NotificationDto postData)
		{
            var success = false;
            try
            {
                var notify = NotificationsController.Instance.GetNotification(postData.NotificationId);
                ParsePublishKey(notify.Context);

                var cntBoard = new Controllers.BoardsController();
                var objCard = cntBoard.GetCard(_cardId);

                if (objCard != null)
                {
                    // add logic to change board list 

                    success = true;
                    NotificationsController.Instance.DeleteNotification(postData.NotificationId);
                }
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
            }

            return success ? Request.CreateResponse(HttpStatusCode.OK, new { Result = "success" }) : Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "unable to process notification");
		}

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage RejectAssignement(NotificationDto postData)
		{
            var success = false;
            try
            {
                var notify = NotificationsController.Instance.GetNotification(postData.NotificationId);
                ParsePublishKey(notify.Context);

                var cntBoard = new Controllers.BoardsController();
                var objCard = cntBoard.GetCard(_cardId);

                if (objCard != null)
                {
                    success = true;
                    NotificationsController.Instance.DeleteNotification(postData.NotificationId);
                }
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
            }

            return success ? Request.CreateResponse(HttpStatusCode.OK, new { Result = "success" }) : Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "unable to process notification");
		}

		#region Private Methods

		private void ParsePublishKey(string key)
		{
			var keys = key.Split(Convert.ToChar(":"));
			// 0 is content type string, to ensure unique key
			_cardId = int.Parse(keys[1]);
            //_moduleId = int.Parse(keys[2]);
		}

		#endregion

		}
} 