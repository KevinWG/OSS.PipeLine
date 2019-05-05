# 事件流-事件节点内部任务项

同一个事件节点内，可能会有多个任务项
每个项必须有返回值，表示任务执行结果
每个项可以由消息队列，Http等触发器独立触发，也可以由worker 内部触发



待完善功能：

2. 对结果的影响（无直接影响，整体回退）



重要错误标识信息：

  Do方法
  --【出参】
	sys_ret = (int)SysResultTypes.RunFailed 系统会字段判断是否满足重试条件执行重试 

  RunEnd
  --【入参】   
	sys_ret = (int)SysResultTypes.RunFailed 表明最终执行失败，
	sys_ret = (int)SysResultTypes.RunPause 表示符合间隔重试条件，会通过 contextKeeper 保存信息后续唤起

