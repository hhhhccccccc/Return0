<!-- TRELLIS:START -->
# Trellis Instructions

These instructions are for AI assistants working in this project.

Use the `/trellis:start` command when starting a new session to:
- Initialize your developer identity
- Understand current project context
- Read relevant guidelines

Use `@/.trellis/` to learn:
- Development workflow (`workflow.md`)
- Project structure guidelines (`spec/`)
- Developer workspace (`workspace/`)

Keep this managed block so 'trellis update' can refresh the instructions.

<!-- TRELLIS:END -->

# 项目架构大纲

## 项目概述

Return0 是一个基于 Unity + Zenject 的武侠策略游戏项目，采用模块化架构，包含完整的战斗系统、UI 系统、存档系统、资源管理系统等。

## 核心技术栈

- **Unity** - 游戏引擎
- **Zenject** - 依赖注入框架
- **YooAsset** - 资源管理
- **OdinInspector** - 编辑器工具
- **Luban** - 配置表生成

## 架构分层

### 1. 入口层 (AppManager / GameManager)

- `AppManager.cs` - 应用管理器基类，处理依赖注入、Manager 初始化、更新循环
- `GameManager.cs` - 游戏入口，初始化 YooAsset、资源管理器、各类 Manager

```
初始化流程:
OnPreWork() → InitAppManager() → InitCustomManagerBefore() → InitManagers → InitCustomManagerAfter() → OnGameReady()
```

### 2. Manager 层 (Manager/)

负责核心系统管理，采用依赖注入:

| Manager | 职责 |
|---------|------|
| **MessageManager** | 消息系统，事件分发与订阅 |
| **ControllerManager** | 控制器管理，自动注册 IController |
| **ModelManager** | 模型管理，SingleModel / ISingleArchiveModel 生命周期 |
| **ViewManager** | 视图管理，创建 UI 根节点、相机、灯光 |
| **UIManager** | UI 面板管理，PanelLayer 分层显示 |
| **ResourceManager** | 资源加载，封装 YooAsset |
| **PoolManager** | 对象池管理 |
| **ArchiveManager** | 存档管理 |
| **LogManager** | 日志管理 |
| **InputManager** | 输入管理 |
| **JobManager** | 任务/协程管理 |
| **ConditionManager** | 条件系统 |

**基类:**
- `ManagerBase` - 所有 Manager 基类，实现 `IManager` 接口，`Init()` 协程初始化
- `Singleton<T>` - 泛型单例 (非 Mono)
- `MonoSingleton<T>` - MonoBehaviour 单例，跨场景持久化

### 3. Controller 层 (Controller/)

处理业务逻辑，通过消息驱动:

- 继承 `ControllerBase<TMsg>` 实现 `Handle(TMsg msg)` 方法
- 自动注册到 `ControllerManager`，监听对应消息类型
- 通过 `[Inject]` 获取依赖

**示例:**
```csharp
public class BattleStartController : ControllerBase<BattleStartEventModel>
{
    [Inject] private BattleManager BattleManager;
    public override void Handle(BattleStartEventModel model) { ... }
}
```

### 4. Model 层 (Model/)

数据层:

| 类型 | 说明 |
|------|------|
| **SingleModel** | 单例模型，通过消息驱动生命周期 |
| **SingleArchiveModel** | 存档模型，自动保存/加载 |
| **BattleUnit** | 战斗单位，属性、技能、Buff、键管理 |
| **BattleField** | 战场，双方阵地 |

### 5. View 层 (View/)

UI 表现层:

- `View` - 视图基类，自动注入组件 (`[AutoFind]` 属性)、消息订阅、资源加载
- `Panel` - 面板基类，继承自 View，支持显示/隐藏回调

### 6. Message 层 (Message/)

事件系统:

- `MessageModel` - 消息基类，实现 `IRecycle` 对象池接口
- `MessageManager` - 消息分发，使用 `Register<T>/DispatchMsg<T>`

### 7. Config 层 (Config/)

配置表系统:

- `ConfigManager` - 配置表管理器，使用 Luban 生成的 `Tables` 类
- 配置数据存储在 `StreamingAssets/Luban/*.json`

**支持的配置表:**
- 战斗 Buff / 技能 / 心法 / 宝物配置
- 战斗时机 (Moment) 条件与效果配置
- 场景/地图/区域配置
- 天气/季节/时间配置
- 角色/物品/事件配置

### 8. Interface 层 (Interface/)

接口定义:
- `IManager` - Manager 接口
- `IController<T>` - 控制器接口
- `IModel` - 模型接口
- `IInitRootBefore/IInitRootAfter` - Manager 初始化顺序标记

## 目录结构

```
Assets/Scripts/
├── AppManager.cs           # 应用入口基类
├── GameManager.cs          # 游戏入口
│
├── Config/                 # 配置表系统
│   ├── ConfigManager.cs
│   └── Config/             # Luban 生成的配置类
│
├── Const/                  # 常量定义
│   ├── GameConst.cs
│   ├── GameEnumArray.cs
│   └── GameResource.cs
│
├── Controller/             # 控制器
│   └── Impl/
│       ├── Battle/         # 战斗相关控制器
│       ├── DebugController.cs
│       └── GameStartController.cs
│
├── Debug/                  # 调试系统
│
├── Interface/              # 接口定义
│   ├── Base/               # 基础接口
│   └── Mono/               # Mono 相关接口
│
├── Manager/                # 核心管理器
│   ├── ArchiveManager/
│   ├── ConditionManager/
│   ├── ControllerManager/
│   ├── InputManager/
│   ├── JobManager/
│   ├── LogManager/
│   ├── MessageManager/
│   ├── ModelManager/
│   ├── PoolManager/
│   ├── ResourceManager/
│   ├── UIManager/
│   └── ViewManager/
│
├── Message/                # 消息系统
│   ├── Base/
│   └── Impl/
│       ├── Battle/         # 战斗事件
│       ├── Business/       # 业务事件
│       └── InputEventModel.cs
│
├── Model/                  # 数据模型
│   ├── Archive/            # 存档系统
│   ├── Battle/             # 战斗核心
│   │   ├── Logic/          # 战斗逻辑
│   │   │   ├── BattleUnit.cs
│   │   │   ├── BattleProperty.cs
│   │   │   ├── BattleKey.cs
│   │   │   ├── BattleSkill/   # 技能实现
│   │   │   ├── BattleMomentCondition/  # 时机条件
│   │   │   └── BattleTreasure/ # 宝物
│   │   └── Manager/       # 战斗管理器
│   └── Util/
│
├── MonoEx/                 # Mono 扩展
│
└── View/                   # UI 视图
    ├── Battle/             # 战斗表现
    ├── Panel/
    │   ├── Gen/            # 自动生成的面板
    │   └── UILogic/       # 面板逻辑
    └── Scene/             # 场景表现
```

## 核心概念

### 依赖注入 (Zenject)
- 使用 `[Inject]` 属性注入依赖
- 通过 `DiContainer` 手动绑定或自动扫描

### 消息驱动
- Controller 通过消息驱动业务逻辑
- View 通过消息响应数据变化
- Model 通过消息触发数据更新

### 对象池
- 实现 `IRecycle` 接口的对象可复用
- `PoolManager.GetClass<T>() / RecycleClass<T>()`

### 战斗系统核心概念
- **ActionWheel (息)** - 战斗时间单位
- **BattleUnit** - 战斗单位 (角色)
- **BattleKey** - 键 (上/下/左/右)
- **BattleProperty** - 属性 (气血/刚气/玄气等)
- **BattleSkill** - 技能 (主动/被动)
- **Buff** - 增益/减益状态
- **HeartMethod** - 心法 (被动技)
- **Treasure** - 宝物 (装备)
- **BattleMoment** - 战斗时机 (触发条件与效果)

## 开发规范

1. **新增 Manager**: 继承 `ManagerBase`，实现对应接口
2. **新增 Controller**: 继承 `ControllerBase<TMsg>`，实现 `Handle` 方法
3. **新增 Model**: 根据需要选择 `SingleModel` / `SingleArchiveModel` / 普通类
4. **新增 UI Panel**: 继承 `Panel`，使用 `[AutoFind]` 自动注入组件
5. **新增消息**: 继承 `MessageModel`
6. **配置表**: 使用 Luban 工具生成，存放在 `StreamingAssets/Luban/`
