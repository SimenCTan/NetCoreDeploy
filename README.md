# NetCoreDeploy
本开源项目的主要目的是结合时下流行的Docker容器技术，
利用Nginx、Consul、Kubernetes开源工具为.NET Core应用程序
构建分布式容器集群、实现自动部署，弹性伸缩，资源调度和负载均衡提供整套解决方法的实例
NetCoreDeploy Introduction文档里面详细介绍了本项目的架构和流程，并划分了项目各个阶段。

如果你也喜欢.NetCore 请为此项目打Call(Star)。



--------------------进度------------------------------

目前已基本实现项目的第一阶段开发工作
ConsulClientSite项目是用来做测试的Core MVC项目
ConsulCommon是一个共用的Consul进行服务注册的项目
ConsulModels是一个共用类的项目
ConsulService是一个Core API项目用来进行服务注册

进行测试的时候----》先运行Consul下载地址：https://www.consul.io 
----》再运行ConsulService项目 ----》再运行ConsulClientSite

----------------------结束-----------------------------