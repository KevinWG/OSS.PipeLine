using System.Collections.Generic;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;

namespace OSS.TaskFlow.Tasks.Mos
{
    public class TaskResultMo : ResultMo
    {
        public TaskResultMo()
        {
        }

        public TaskResultMo(int ret, string message = "") : base(ret, message)
        {
        }

        public TaskResultMo(TaskResultType taskRet, int ret=0, string message = "") : base(ret, message)
        {
        }

        public TaskResultMo(ResultTypes ret, string message = "") : base(ret, message)
        {
        }
        
        public TaskResultMo(TaskResultType taskRet, ResultTypes ret=0, string message = "") : base(ret, message)
        {
            task_ret = (int)taskRet;
        }

        public int task_ret { get; set; }
    }

    public class TaskResultMo<TType> : TaskResultMo
    {
        public TaskResultMo(TType data)
        {
            this.data = data;
        }

        public TaskResultMo(int ret, string message = "")
            : base(ret, message)
        {
        }

        public TaskResultMo(TaskResultType taskRet, int ret = 0, string message = "")
            : base(taskRet, ret, message)
        {
        }

        public TaskResultMo(ResultTypes ret, string message = "")
            : base(ret, message)
        {
        }

        public TaskResultMo(TaskResultType taskRet, ResultTypes ret=0, string message = "")
            : base(taskRet,ret, message)
        {
        }
        public TType data { get; set; }
    }

    public class TaskResultListMo<TType> : TaskResultMo
    {
        public TaskResultListMo(IList<TType> data)
        {
            this.data = data;
        }

        public TaskResultListMo(int ret, string message = "")
            : base(ret, message)
        {
        }

        public TaskResultListMo(TaskResultType taskRet, int ret=0, string message = "")
            : base(taskRet, ret, message)
        {
        }

        public TaskResultListMo(ResultTypes ret, string message = "")
            : base(ret, message)
        {
        }

        public TaskResultListMo(TaskResultType taskRet, ResultTypes ret=0, string message = "")
            : base(taskRet, ret, message)
        {
        }

        public IList<TType> data { get; set; }
    }

    public static class TaskResultExtention
    {
        public static TaskResultMo ConvertToTaskResult(this ResultMo res,TaskResultType taskRet)
        {
            return new TaskResultMo(taskRet,res.ret,res.msg);
        }
        
        public static TaskResultMo<T> ConvertToTaskResult<T>(this ResultMo<T> res, TaskResultType taskRet)
        {
            return new TaskResultMo<T>(taskRet, res.ret, res.msg){data = res.data};
        }

        public static TaskResultListMo<T> ConvertToTaskResult<T>(this ResultListMo<T> res, TaskResultType taskRet)
        {
            return new TaskResultListMo<T>(taskRet, res.ret, res.msg) { data = res.data };
        }
    }
}
