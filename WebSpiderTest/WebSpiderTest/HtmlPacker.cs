using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebSpiderTest
{
    /// <summary>
    /// Html信息封装
    /// </summary>
    internal class HtmlPacker
    {
        #region 私有字段
        /// <summary>
        /// 使用的Html地址
        /// </summary>
        private string m_strThisUrl = string.Empty;

        /// <summary>
        /// 使用的Html数据类型
        /// </summary>
        private string m_strThisHtmlData = string.Empty;

        /// <summary>
        /// 当前的标题
        /// </summary>
        private string m_nowTitle = string.Empty;

        /// <summary>
        /// 当前页面所在的深度
        /// </summary>
        private int m_nNowDeep = 0;

        /// <summary>
        /// 下游页面的Url地址列表
        /// </summary>
        private List<string> m_lstSubUrls = new List<string>();
        #endregion

        /// <summary>
        /// 当前的地址
        /// </summary>
        internal string ThisUrl
        {
            get
            {
                return m_strThisUrl;
            }

            private set
            {
                m_strThisUrl = value;
            }
        }

        /// <summary>
        /// 下游页面的地址列表
        /// </summary>
        internal List<string> SubUrls
        {
            get
            {
                return m_lstSubUrls;
            }

            private set
            {
                m_lstSubUrls = value;
            }
        }

        /// <summary>
        /// 当前的标题
        /// </summary>
        internal string NowTitle
        {
            get
            {
                return m_nowTitle;
            }

            private set
            {
                m_nowTitle = value;
            }
        }

        /// <summary>
        /// 构造的页面封装
        /// </summary>
        /// <param name="inputBaseUrlName">输入的基路径地址</param>
        /// <param name="inputUrl">输入的地址</param>
        /// <param name="inputHtmlData">输入的Html数据</param>
        /// <param name="inputDeep">输入的深度</param>
        internal HtmlPacker
            (string inputUrl, string inputHtmlData, int inputDeep)
        {
            ThisUrl = inputUrl;
            m_strThisHtmlData = inputHtmlData;
            m_nNowDeep = inputDeep;
            //准备标题
            PrepareTitle();
            //准备下层地址
            PrepareSubUrls();
        }

        /// <summary>
        /// 准备下游地址列表
        /// </summary>
        private void PrepareSubUrls()
        {
            //链接正则规则
            string usePattern = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            //获得所有子链接
            foreach (Match oneMatch in Regex.Matches(m_strThisHtmlData, usePattern))
            {
                SubUrls.Add(oneMatch.Value);
            }
        }

        /// <summary>
        /// 准备当前标题
        /// </summary>
        private void PrepareTitle()
        {
            //零宽断言
            string usePattern = @"(?<=<title>).*(?=</title>)";
            //寻找标题
            foreach (Match oneMatch in Regex.Matches(m_strThisHtmlData, usePattern))
            {
                //获取标题
                this.NowTitle = oneMatch.Value;
                break;
            }
        }
    }
}