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

using DotNetNuke.Modules.Boards.Components.Integration;

namespace DotNetNuke.Modules.Boards.Components.Controllers
{

    //uncomment the interfaces to add the support.
    public class FeatureController //: IPortable, ISearchable, IUpgradeable
    {

        #region Optional Interfaces

        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="moduleID">The Id of the module to be exported</param>
        public string ExportModule(int moduleID)
        {
            //string strXML = "";

            //List<BoardsInfo> colBoards = GetBoards(moduleID);
            //if (colBoards.Count != 0)
            //{
            //    strXML += "<Boards>";

            //    foreach (BoardsInfo objBoards in colBoards)
            //    {
            //        strXML += "<Boards>";
            //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objBoards.Content) + "</content>";
            //        strXML += "</Boards>";
            //    }
            //    strXML += "</Boards>";
            //}

            //return strXML;

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="moduleID">The Id of the module to be imported</param>
        /// <param name="content">The content to be imported</param>
        /// <param name="version">The version of the module to be imported</param>
        /// <param name="userId">The Id of the user performing the import</param>
        public void ImportModule(int moduleID, string content, string version, int userId)
        {
            //XmlNode xmlBoards = DotNetNuke.Common.Globals.GetContent(Content, "Boards");
            //foreach (XmlNode xmlBoards in xmlBoardss.SelectNodes("Boards"))
            //{
            //    BoardsInfo objBoards = new BoardsInfo();
            //    objBoards.ModuleId = moduleID;
            //    objBoards.Content = xmlBoards.SelectSingleNode("content").InnerText;
            //    objBoards.CreatedByUser = userId;
            //    AddBoards(objBoards);
            //}

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="modInfo">The ModuleInfo for the module to be Indexed</param>
        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo modInfo)
        {
            //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

            //List<BoardsInfo> colBoards = GetBoards(modInfo.ModuleID);

            //foreach (BoardsInfo objBoards in colBoards)
            //{
            //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objBoards.Content, objBoards.CreatedByUserId, objBoards.CreatedOnDate, modInfo.ModuleID, objBoards.ItemId.ToString(), objBoards.Content, "ItemId=" + objBoards.ItemId.ToString());
            //    SearchItemCollection.Add(SearchItem);
            //}

            //return SearchItemCollection;

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="version">The current version of the module</param>
        public string UpgradeModule(string version)
        {

            var message = "";

            switch (version)
            {
                case "01.00.00":
                    Notifications.AddNotificationTypes();
                    message = "Added notification types for the Boards module.";
                    break;
            }

            return message;
        }

        #endregion

    }
}