namespace OSS.EventNode.Mos
{
    public enum NodeStatus
    {
        ProcessFailedRevert = -60,

        ProcessFailed = -50,
        
        Triggered =0,

        WaitProcess=10,

        ProcessPaused=20,
        
        BreakOut=30,

        ProcessCompoleted=50,


    }
}
