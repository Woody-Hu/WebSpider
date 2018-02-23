using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpiderTest
{
    /// <summary>
    /// 地址队列
    /// </summary>
    internal class UrlQueue
    {
        /// <summary>
        /// 未使用的地址值/键对
        /// 网络地址/深度
        /// </summary>
        private Dictionary<string, int> m_dicUnUseUrlAndDeep = new Dictionary<string, int>();

        /// <summary>
        /// 所有的已使用过的地址
        /// </summary>
        private List<string> m_lstUsedUrls = new List<string>();

        /// <summary>
        /// 当前未使用地址的数量
        /// </summary>
        internal int UnUseSize
        {
            get
            {
                return m_dicUnUseUrlAndDeep.Count;
            }
        }

        /// <summary>
        /// 当前使用过的地址数量
        /// </summary>
        internal int UseSize
        {
            get
            {
                return m_lstUsedUrls.Count;
            }
        }

        /// <summary>
        /// 判断输入地址是否在队列中
        /// </summary>
        /// <param name="inputUrl">输入的地址</param>
        /// <returns></returns>
        internal bool IfInQueue(string inputUrl)
        {
            return m_dicUnUseUrlAndDeep.ContainsKey(inputUrl)
                || m_lstUsedUrls.Contains(inputUrl);
        }

        /// <summary>
        /// 将一个地址压入队列
        /// </summary>
        /// <param name="inputUrl">输入的地址</param>
        /// <param name="inputDeep">输入的深度</param>
        internal void Push(string inputUrl, int inputDeep)
        {
            if (!m_dicUnUseUrlAndDeep.ContainsKey(inputUrl))
            {
                m_dicUnUseUrlAndDeep.Add(inputUrl, inputDeep);
            }
        }

        /// <summary>
        /// 从队列顶端拿一个地址
        /// </summary>
        /// <returns></returns>
        internal KeyValuePair<string, int> Pop()
        {
            KeyValuePair<string, int> returnValue = new KeyValuePair<string, int>();
            if (m_dicUnUseUrlAndDeep.Count > 0)
            {
                //获得返回值
                returnValue = new KeyValuePair<string, int>
                    (m_dicUnUseUrlAndDeep.ElementAt(0).Key, m_dicUnUseUrlAndDeep.ElementAt(0).Value);
                //移除
                m_dicUnUseUrlAndDeep.Remove(returnValue.Key);
                m_lstUsedUrls.Add(returnValue.Key);
            }

            return returnValue;
        }

    }
}