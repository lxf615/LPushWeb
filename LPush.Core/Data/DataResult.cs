
namespace LPush.Core.Data
{
    public class DataResult
    {
        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 消息代码
        /// </summary>
        public string MsgCode { get; set; }
    }

    public class DataResult<T>:DataResult
    {
        public T Content { get; set; }
    }
}
