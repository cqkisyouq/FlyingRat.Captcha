# FlyingRat.Captcha

 做为net core 的滑块、点击等验证码通用生成与验证库 FlyingRat.Captcha：后台服务,FlyingRat.Captcha.Scripts：js、css。验证码分为生成和验证二步，抽象后统一处理方便扩展自定义验证码的实现，js也是使用的扩展生成与验证，并且使用模型进行统一处理，留有扩展方式。
 
## 使用

  ### service
  添加服务 services.AddCaptchaCore() 使用captcha
  
  添加过程处理 AddCaptchaHandler<SliderCaptchValidateHandler>() 可处理生成与验证过程
  
  图片路径配置 CaptchaImageOptions 当前实现的点击与滑块所需使用的文件路径
  
  点击验证配置 PointOptions 
  
  滑块验证配置 SliderOptions 
  
  实现 BaseCaptcha 可扩展自定义验证码生成
  
  实现 BaseValidator<T> : IValidator where T:ICaptcha,new() 可扩展自定义验证码验证方式
  
  例：service.AddCaptcha<SliderCaptcha>();service.AddCaptchaValidator<SliderValidator>(); 完成注册
  
  ICaptchaManager 进行统一维护生成验证码和验证数据操作
 
 ### js
 引用 flyingrat.captcha.js，flyingrat.captcha.css 到页面，captcha_icon.png 放在同级 image 文件夹中
 
 let cap =new captcha(element,options) //为空默认查找 id="flyingrat" 的元素
 
 cap.drawCaptcha() 进行调用显示 设置好url并打开自动刷新会自动请求数据并刷新页面
 
 divCap.drawCaptcha(data) 主动刷新页面
 
 ### js captcha 提供开放api
 "name"：服务返回的当前验证的name

"type": 定义的类型

"data": 生成返回的验证码数据

"result": 验证结果

"captchas": 可用验证码列表

"tools": 可用工具列表

"invokeCaptcha": 调用对应验证码的方法

"current": 当前使用的验证码

"points": 相应点数据

"maxPoint": 最大能拥有的点

"addPoint": 添加一个点

"clearPoints": 清空点数据

"addCaptcha": 添加一个自定义验证实现

 "updateCaptcha": 更新验证数据 { events:null,options:null},只能更新事件及配置数据

"addTool": 添加一个自定义工具

"createTool": 创建一个工具

"updateTool": 更新工具配置

"drawCaptcha": 根据数据展示验证码

"refresh": 刷新验证码,

"destroy": 删除当前对象数据,

"destroyCurrent": 删除当前验证码数据对象,

"validate": 验证,

"validated": 验证完成,

"validatePoint": 验证数据点,

"createJsonData": 创建验证所需要的数据,

"delayedFunction": 延迟执行方法,

"autoValidate": 自动验证：true|false,

"autoRefresh": 自动刷新：true|false,

"mapData": 验证码数据,

"mapResult": 验证结果数据,

"models": { captcha, tool, data, result} 所使用的数据模型

"updateTips": 刷新提示信息,

"show": 显示对象,

"hidden": 隐藏对象,

"modal": {  show , hidden } 如果存在此节点 可执行对应行为

"icon": { show, hidden } 如果存在此节点 可执行对应行为

具体使用可参考 test 里的 captcha 工程项目中的 Image 页面使用方式
