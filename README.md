# 2.27-Simulation-Visualization

## Scripts

### mesh

| 文件名            | 描述                                   |
| ----------------- | -------------------------------------- |
| mesh.cs           | 初始                                   |
| meshStandard.cs   | 动力学计算 mesh（Slun\_）→ 改为最终版 |
| mesh2part.cs      | 疲劳寿命预测 mesh（danlun\_）          |
| meshseparate.cs   | 流固耦合仿真 mesh（lun\_）             |
| meshtxt.cs        | 流固耦合仿真 mesh 流场（无前缀文件）   |
| meshsolid.cs      | 流固耦合仿真 mesh 轮对（pingban\_）    |
| ColorValueMesh.cs | 颜色条 mesh（meshbiaochi.cs 改）       |

### 全局

| 文件名                        | 描述                                                                          |
| ----------------------------- | ----------------------------------------------------------------------------- |
|                               |                                                                               |
| ReadFile.cs                   | 读取文件内容                                                                  |
| SwitchToColor.cs              | 将数据值转化为颜色值，输出Color[]                                             |
| ColorValueController.cs       | 控制颜色条的显隐                                                              |
| SpatialAwarenessController.cs | 提供空间网格三种模式转换                                                      |
| SetPosition.cs                | 把merge中结构强度物体的当前位置赋给疲劳寿命物体                               |
| PositionReader.cs             | 读取QR码、merge、IF、IW的当前位置，获取轮对、构架与QR码的相对位置定位         |
| PositionSaver.cs              | 创建本地txt文件，将frame、wheelset的transform数据进行持久性存储，以及启动更新 |

### QR识别

from [https://github.com/yl-msft/QRTracking](https://github.com/yl-msft/QRTracking)

| 文件名                           | 描述                                                                                         |
| -------------------------------- | -------------------------------------------------------------------------------------------- |
| QRCode.cs                        | 设置获取QR码基本信息                                                                         |
| SpatialGraphNodeTracker.cs（改） | 获取QR码实物位置信息并将QRPrefab贴在实物上（改：添加SetPosition()将merge物体定位到QR码位置） |
| QRCodesManager.cs                | QR识别的启停，QR码的添加、更新、删除的调用                                                   |
| QRCodesVisualizer.cs             | QR码的添加、更新、删除，包括QRPrefab的实例化（改：添加传递位置信息的public对象）             |

### 流场

| 文件名            | 描述                                          |
| ----------------- | --------------------------------------------- |
| FlowField.cs      | 流固耦合仿真gizmos流场（无前缀文件）          |
| DrawLine.cs       | 调用linerenderer组件绘制流线                  |
| DoTweenPath.cs    | 调用DoTween插件的提供方法使小球沿流线轨迹运动 |
| SetLine.cs        | 查找获取可连成流线的点集并绘制                |
| CatmulRomCurve.cs | 将点集转变为样条曲线插值后点集                |
| BezierCurve.cs    | 将点集转变为贝塞尔曲线插值后点集              |
