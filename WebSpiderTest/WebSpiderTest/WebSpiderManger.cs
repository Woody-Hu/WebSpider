using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebSpiderTest
{
    /// <summary>
    /// 爬虫管理器
    /// </summary>
    internal class WebSpiderManger
    {
        #region 私有字段
        /// <summary>
        /// 爬虫最深层级深度
        /// </summary>
        private int m_nMaxDeep = 3;

        /// <summary>
        /// 标题关键词
        /// </summary>
        private string m_strTitlKeyWord = string.Empty;

        /// <summary>
        /// 最大的爬取数量
        /// </summary>
        private int m_nMaxCount = 50;

        /// <summary>
        /// 输入的基地址
        /// </summary>
        private string m_strBaseUrl = string.Empty;

        /// <summary>
        /// 使用的地址队列
        /// </summary>
        private UrlQueue m_useQueue = new UrlQueue();

        /// <summary>
        /// 使用的请求管理器
        /// </summary>
        private WebRequestManger m_useRequestManger = new WebRequestManger();

        /// <summary>
        /// 爬取的结果
        /// </summary>
        private List<HtmlPacker> m_lstResult = new List<HtmlPacker>();
        #endregion

        /// <summary>
        /// 爬取的结果
        /// </summary>
        internal List<HtmlPacker> LstResult
        {
            get
            {
                return m_lstResult;
            }

            private set
            {
                m_lstResult = value;
            }
        }

        internal WebSpiderManger(string inputBaseUrl, string inputKeyTitleWord)
        {
            m_strTitlKeyWord = inputKeyTitleWord;
            m_strBaseUrl = inputBaseUrl;
            //将基地址压入队列
            m_useQueue.Push(inputBaseUrl, 0);
        }

        /// <summary>
        /// 执行爬虫
        /// </summary>
        internal void DoSpider()
        {
            //广度优先执行
            while (m_useQueue.UnUseSize > 0 && m_useQueue.UseSize <= m_nMaxCount)
            {
                DoOneSpiderWork();
            }
        }

        #region 私有方法
        /// <summary>
        /// 执行一次爬虫操作
        /// </summary>
        private void DoOneSpiderWork()
        {
            KeyValuePair<string, int> nowUseUrl = new KeyValuePair<string, int>();
            HtmlPacker tempHtmlPacker = null;
            //若有可被使用的数据
            if (m_useQueue.UnUseSize > 0)
            {
                //从队列中获取结果
                nowUseUrl = m_useQueue.Pop();
                //获取一个Html封装
                tempHtmlPacker = m_useRequestManger.GetHtmlPacker
                    (nowUseUrl.Key, nowUseUrl.Value);
                //获得下层深度
                int nextDeep = nowUseUrl.Value + 1;
                //判断结果是否需要保存
                if (IfHtmlPackerIsWanted(tempHtmlPacker))
                {
                    LstResult.Add(tempHtmlPacker);
                }
                //若成功获取封装且在合理深度
                if (null != tempHtmlPacker && nextDeep <= m_nMaxDeep)
                {
                    foreach (var oneSubUrl in tempHtmlPacker.SubUrls)
                    {
                        //若地址可用
                        if (true == IfUrlCanUse(oneSubUrl))
                        {
                            if (m_useQueue.UnUseSize <= m_nMaxCount)
                            {
                                //压入队列
                                m_useQueue.Push(oneSubUrl, nextDeep);
                            }
                           
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 判断爬取结果是否可用
        /// </summary>
        /// <param name="input">输入的封装</param>
        /// <returns></returns>
        private bool IfHtmlPackerIsWanted(HtmlPacker input)
        {
            //null值检查
            if (null == input || string.IsNullOrEmpty(input.NowTitle))
            {
                return false;
            }
            //null关键字不检查
            else if (string.IsNullOrEmpty(m_strTitlKeyWord))
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(input.NowTitle, m_strTitlKeyWord);
            }
        }

        /// <summary>
        /// 判断输入地址是否可用
        /// </summary>
        /// <param name="inputUrl"></param>
        /// <returns></returns>
        private bool IfUrlCanUse(string inputUrl)
        {
            //若在队列中不可用
            if (m_useQueue.IfInQueue(inputUrl))
            {
                return false;
            }
            //不需要图片
            if (inputUrl.Contains(".jpg") || inputUrl.Contains(".gif")
               || inputUrl.Contains(".png") || inputUrl.Contains(".css")
               || inputUrl.Contains(".js") || inputUrl.Contains(".dtd") || inputUrl.Contains(".xml"))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}