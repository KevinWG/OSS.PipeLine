﻿using OSS.EventFlow.MetaMos;

namespace OSS.EventFlow.Mos
{
    public class FlowContext
    {
        public string run_id { get; set; }

        public string link_run_id { get; set; }

        /// <summary>
        ///  当前流元信息
        /// </summary>
        public FlowMeta flow_meta { get; set; }
    }



}
