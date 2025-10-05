# DbusSmsForward
用于部分随身wifi刷了Debian后的短信的Email、PushPlus、企业微信自建应用、TG机器人、钉钉机器人消息转发、Bark推送转发以及短信发送，通过监听dbus实时获取新接收的短信并转发以及调用dbus发送短信
# 使用教程
1.输入
sudo apt install libicu67
安装libicu

2.解压程序至home文件夹下

3.输入 
sudo chmod -R 777 DbusSmsForward
配置程序可执行权限

4.输入
sudo ./DbusSmsForward
运行程序

5.根据提示配置相关邮箱信息

6.带参数运行跳过程序初始的运行模式选择以达到快速运行程序

输入
sudo ./DbusSmsForward -fE
跳过运行模式选择直接进入邮箱转发模式

输入
sudo ./DbusSmsForward -fP
跳过运行模式选择直接进入PushPlus转发模式

输入
sudo ./DbusSmsForward -fW
跳过运行模式选择直接进入企业微信转发模式

输入
sudo ./DbusSmsForward -fT
跳过运行模式选择直接进入TGBot转发模式

输入
sudo ./DbusSmsForward -fD
跳过运行模式选择直接进入钉钉转发模式

输入
sudo ./DbusSmsForward -fB
跳过运行模式选择直接进入Bark转发模式

输入 
sudo ./DbusSmsForward -fE -fP -fD
可同时将接收的短信转发到email、pushplus和钉钉机器人，转发哪几个渠道取决于你添加的-f*指令

输入
sudo ./DbusSmsForward -sS
跳过运行模式选择直接进入短信发送界面

# 编译linux-arm64的native aot版本
拉取最新源码，进入程序根目录，使用docker进行交叉编译

docker build --pull -t cross-build-arm64 -f Dockerfile.cross-build-x64-arm64 . 

docker run --rm -it -v $(pwd):/source -w /source cross-build-arm64 dotnet publish -a arm64 -o app-arm64 -p:SysRoot=/crossrootfs/arm64 -p:LinkerFlavor=lld

# 编译linux-musl-arm64的native aot版本
拉取最新源码，进入程序根目录，使用docker进行交叉编译

docker build --pull -t cross-build-arm64-musl -f Dockerfile.cross-build-x64-arm64-musl . 

docker run --rm -it -v $(pwd):/source -w /source cross-build-arm64-musl dotnet publish -a musl-arm64 -o app-arm64-musl -p:SysRoot=/crossrootfs/arm64 -p:LinkerFlavor=lld

# 参考
1. [ModemManager API document](https://www.freedesktop.org/software/ModemManager/api/latest/)
2. [Tmds.DBus](https://github.com/tmds/Tmds.DBus)
