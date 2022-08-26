namespace OSS.Pipeline;

/// <summary>
///  通行信号
/// </summary>
public enum SignalFlag
{
    /// <summary>
    ///  正常通过
    /// </summary>
    Green_Pass,

    /// <summary>
    ///  警告等待
    /// </summary>
    Yellow_Wait,

    /// <summary>
    /// 异常阻塞
    /// </summary>
    Red_Block
}