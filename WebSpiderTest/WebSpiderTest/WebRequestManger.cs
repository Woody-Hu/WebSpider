using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebSpiderTest
{
    /// <summary>
    /// Web请求管理器
    /// </summary>
    internal class WebRequestManger
    {
        #region 私有字段

        /// <summary>
        /// 当前使用的请求封装
        /// </summary>
        private HttpWebRequest m_useWebRequest = null;

        /// <summary>
        /// 使用的请求方法
        /// </summary>
        private string m_strUseWebMethod = "GET";

        /// <summary>
        /// 使用的接收类型
        /// </summary>
        private string m_strUseAcceptType = "text/html";

        /// <summary>
        /// 使用的身份标示
        /// </summary>
        private string m_strUseAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";

        /// <summary>
        /// 使用的GB解码器
        /// </summary>
        private Encoding m_useEncodingUTF = Encoding.UTF8;
        #endregion

        /// <summary>
        /// 获取输入地址的Html数据封装
        /// </summary>
        /// <param name="inputUrl">输入的地址</param>
        /// <param name="nowDeep">当前的深度</param>
        /// <returns>Html封装</returns>
        internal HtmlPacker GetHtmlPacker(string inputUrl, int nowDeep)
        {
            m_useWebRequest = null;
            m_useWebRequest = CreatHttpWebRequestByUrl(inputUrl);
            PrepareHttpWebRequest
                (ref m_useWebRequest, m_strUseWebMethod, m_strUseAcceptType, m_strUseAgent);
            string tempHtmlValue = null;

            if (null != m_useWebRequest)
            {
                HttpWebResponse tempResponse = GetResponseNonAsy(m_useWebRequest);
                tempHtmlValue = GetResponseValueNonAsy(tempResponse);
            }

            if (null != tempHtmlValue)
            {
                return new HtmlPacker(inputUrl, tempHtmlValue, nowDeep);
            }
            else
            {
                return null;
            }
        }

        #region 私有方法

        /// <summary>
        /// 输入字符串网络地址获得请求封装
        /// </summary>
        /// <param name="inputUrl">输入的地址</param>
        /// <returns>请求封装</returns>
        private HttpWebRequest CreatHttpWebRequestByUrl(string inputUrl)
        {
            HttpWebRequest returnValue = null;

            try
            {
                returnValue = WebRequest.CreateHttp(inputUrl);
            }
            catch (Exception)
            {
                returnValue = null;
            }

            return returnValue;
        }

        /// <summary>
        /// 预设置请求
        /// </summary>
        /// <param name="inputRequest">输入的请求</param>
        /// <param name="inputMethod">输入的方法</param>
        /// <param name="inputAcceptType">输入的文件接收标头类型</param>
        /// <param name="inputAgent">输入的身份标示</param>
        private void PrepareHttpWebRequest
            (ref HttpWebRequest inputRequest, string inputMethod, string inputAcceptType, string inputAgent)
        {
            //null值检查
            if (null == inputRequest)
            {
                return;
            }

            try
            {
                inputRequest.Method = inputMethod;
                inputRequest.Accept = inputAcceptType;
                inputRequest.UserAgent = inputAgent;
            }
            //异常保护
            catch (Exception)
            {

                ;
            }

        }

        /// <summary>
        /// 单线程获取网络请求的结果
        /// </summary>
        /// <param name="inputRequest">输入的请求</param>
        /// <returns></returns>
        private HttpWebResponse GetResponseNonAsy(HttpWebRequest inputRequest)
        {
            HttpWebResponse returnValue = null;

            if (null != inputRequest)
            {
                try
                {
                    returnValue = (HttpWebResponse)inputRequest.GetResponse();
                }
                catch (Exception)
                {

                    returnValue = null;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 单线程获得请求的结果文本标示
        /// </summary>
        /// <param name="inputResponse">输入的请求</param>
        /// <returns>返回的结果</returns>
        private string GetResponseValueNonAsy(HttpWebResponse inputResponse)
        {
            string returnValue = null;

            if (null != inputResponse)
            {
                try
                {
                    //获得请求输入流
                    using (var st = inputResponse.GetResponseStream())
                    {
                        Encoding nowUseEncoding = null;
                        //获得编码方法
                        string useCodingType = inputResponse.ContentEncoding;

                        try
                        {
                            //动态获取
                            nowUseEncoding = Encoding.GetEncoding(useCodingType);
                        }
                        catch (Exception ex)
                        {
                            nowUseEncoding = null;
                        }

                        //若没有获取则使用国标尝试
                        if (null == nowUseEncoding)
                        {
                            nowUseEncoding = m_useEncodingUTF;
                        }

                        //串流解码
                        using (var codingSt = new StreamReader(st, nowUseEncoding))
                        {
                            //读取所有文本
                            returnValue = codingSt.ReadToEnd();
                        }
                    }
                }
                catch (Exception)
                {
                    returnValue = null;
                }

            }


            return returnValue;
        }
        #endregion
    }
}