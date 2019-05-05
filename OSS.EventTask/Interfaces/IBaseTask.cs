using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public class IBaseTask
    {
        public InstanceType InstanceType { get; protected set; }

        public FollowType RunType { get; set; }
    }
}
