using System;

namespace HuaLiangWindow.Common.MateralTools.MWeChat.Manager
{
    /// <inheritdoc />
    /// <summary>
    /// 创建模型属性
    /// </summary>
    public sealed class CreateModelAttribute:Attribute
    {
        /// <summary>
        /// 是否是必须的
        /// </summary>
        public bool Require { get; set; }

        /// <summary>
        /// 空值信息
        /// </summary>
        public string NullOrEmptyMessage { get; set; }
    }
}
