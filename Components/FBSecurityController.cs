using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace GIBS.FBSecurity.Components
{
    public class FBSecurityController : ISearchable, IPortable
    {

        #region public method

        /// <summary>
        /// Gets all the FBSecurityInfo objects for items matching the this moduleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<FBSecurityInfo> GetFBSecuritys(int moduleId)
        {
            return CBO.FillCollection<FBSecurityInfo>(DataProvider.Instance().GetFBSecuritys(moduleId));
        }

        /// <summary>
        /// Get an info object from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public FBSecurityInfo GetFBSecurity(int moduleId, int itemId)
        {
            return (FBSecurityInfo)CBO.FillObject(DataProvider.Instance().GetFBSecurity(moduleId, itemId), typeof(FBSecurityInfo));
        }


        /// <summary>
        /// Adds a new FBSecurityInfo object into the database
        /// </summary>
        /// <param name="info"></param>
        public void AddFBSecurity(FBSecurityInfo info)
        {
            //check we have some content to store
            if (info.Content != string.Empty)
            {
                DataProvider.Instance().AddFBSecurity(info.ModuleId, info.Content, info.CreatedByUser);
            }
        }

        /// <summary>
        /// update a info object already stored in the database
        /// </summary>
        /// <param name="info"></param>
        public void UpdateFBSecurity(FBSecurityInfo info)
        {
            //check we have some content to update
            if (info.Content != string.Empty)
            {
                DataProvider.Instance().UpdateFBSecurity(info.ModuleId, info.ItemId, info.Content, info.CreatedByUser);
            }
        }


        /// <summary>
        /// Delete a given item from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>
        public void DeleteFBSecurity(int moduleId, int itemId)
        {
            DataProvider.Instance().DeleteFBSecurity(moduleId, itemId);
        }


        #endregion

        #region ISearchable Members

        /// <summary>
        /// Implements the search interface required to allow DNN to index/search the content of your
        /// module
        /// </summary>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            SearchItemInfoCollection searchItems = new SearchItemInfoCollection();

            List<FBSecurityInfo> infos = GetFBSecuritys(modInfo.ModuleID);

            foreach (FBSecurityInfo info in infos)
            {
                SearchItemInfo searchInfo = new SearchItemInfo(modInfo.ModuleTitle, info.Content, info.CreatedByUser, info.CreatedDate,
                                                    modInfo.ModuleID, info.ItemId.ToString(), info.Content, "Item=" + info.ItemId.ToString());
                searchItems.Add(searchInfo);
            }

            return searchItems;
        }

        #endregion

        #region IPortable Members

        /// <summary>
        /// Exports a module to xml
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public string ExportModule(int moduleID)
        {
            StringBuilder sb = new StringBuilder();

            List<FBSecurityInfo> infos = GetFBSecuritys(moduleID);

            if (infos.Count > 0)
            {
                sb.Append("<FBSecuritys>");
                foreach (FBSecurityInfo info in infos)
                {
                    sb.Append("<FBSecurity>");
                    sb.Append("<content>");
                    sb.Append(XmlUtils.XMLEncode(info.Content));
                    sb.Append("</content>");
                    sb.Append("</FBSecurity>");
                }
                sb.Append("</FBSecuritys>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// imports a module from an xml file
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Content"></param>
        /// <param name="Version"></param>
        /// <param name="UserID"></param>
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            XmlNode infos = DotNetNuke.Common.Globals.GetContent(Content, "FBSecuritys");

            foreach (XmlNode info in infos.SelectNodes("FBSecurity"))
            {
                FBSecurityInfo FBSecurityInfo = new FBSecurityInfo();
                FBSecurityInfo.ModuleId = ModuleID;
                FBSecurityInfo.Content = info.SelectSingleNode("content").InnerText;
                FBSecurityInfo.CreatedByUser = UserID;

                AddFBSecurity(FBSecurityInfo);
            }
        }

        #endregion
    }
}
