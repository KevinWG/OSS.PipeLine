namespace OSS.EventFlow.Mos
{
    public enum GatewayType
    {
        /// <summary>
        ///  顺序串行
        /// </summary>
        Serial = 10,

        /// <summary>
        ///  分支并行
        /// </summary>
        Branch = 20,

        /// <summary>
        ///  排他唯一串行
        /// </summary>
        ExclusiveSerial = 30,

        /// <summary>
        /// 动态分支
        /// </summary>
        InclusiveBranch = 30,

        /// <summary>
        ///  复杂多路分支
        /// </summary>
        ComplexBranch = 80,
    }
}
