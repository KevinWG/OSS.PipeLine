# OSS事件流

流式事件处理，自定义流式事件集合，多种触发源（消息队列，HTTP，定时器），多种处理执行（HTTP调用，Azure Function，Ali ServerLess，自定义扩展实现）。
可以配置 重试/并发 等执行策略。事件节点参数具有多数据源组装的能力。




workernode
	  work   只能顺序执行

Processer

	Task  可以并行执行



Strategy 
	重试策略等

Metric -  Http，消息队列，数据库






Entry
Call
Process