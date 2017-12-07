using MateralTools.MEnum;

namespace MissYangQA.Model
{
    /// <summary>
    /// 日志类型枚举
    /// </summary>
    public enum ApplicationLogTypeEnum : byte
    {
        [EnumShowName("操作日志")]
        Options = 1,
        [EnumShowName("调试日志")]
        Debug = 2,
        [EnumShowName("异常日志")]
        Exception = byte.MaxValue,
    }
}
