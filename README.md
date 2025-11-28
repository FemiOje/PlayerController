# Player Controller - Physics-Based Third-Person Character Controller

A custom, physics-based third-person character controller for Unity, built from the ground up using Rigidbody physics. This project implements a complete player control system following software architecture best practices and Unity development guidelines.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Requirements](#requirements)
- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Setup Instructions](#setup-instructions)
- [Controls](#controls)
- [Technical Implementation](#technical-implementation)
- [Design Patterns](#design-patterns)
- [Code Examples](#code-examples)
- [Testing Environment](#testing-environment)
- [Development Guidelines](#development-guidelines)
- [Future Improvements](#future-improvements)

## Overview

This project implements a physics-based third-person character controller system for Unity, created as part of a game development assessment. The controller emphasizes custom physics implementations over Unity's built-in Character Controller component, providing fine-grained control over movement mechanics.

### Assessment Requirements Compliance

- **Physics-Based Movement**: Uses Rigidbody physics with custom force/velocity calculations
- **Third-Person Perspective**: Full camera system with mouse look and follow behavior
- **Keyboard & Mouse Controls**: Complete input system supporting both Unity Input Managers
- **The 3 C's**: Character feel, Camera control, and responsive Controls
- **3D Platformer Features**: Jumping, ground detection, air control, and more
- **Custom Implementation**: No Character Controller component or existing character controller assets
- **Allowed Technologies**: Cinemachine for camera, Unity Input System/Legacy Input Manager

## Features

### Character Movement
- **Physics-Based Movement**: Custom Rigidbody manipulation for precise control
- **Ground Detection**: Raycast and collision-based ground detection system
- **Jump Mechanics**: Configurable jump force with coyote time support
- **Air Control**: Customizable air movement for platformer feel
- **Sprint System**: Variable movement speeds with smooth transitions
- **Slope Handling**: Proper movement on inclined surfaces

### Camera System
- **Third-Person Camera**: Smooth follow camera with configurable offset
- **Mouse Look**: Responsive camera rotation with sensitivity controls
- **Camera Collision**: Automatic camera adjustment to avoid clipping through walls
- **State-Aware Camera**: Camera behavior adapts to player state (grounded, airborne, etc.)

### Input System
- **Dual Input Support**: Works with both Unity Input System and Legacy Input Manager
- **Abstracted Input Layer**: Input system abstracted behind interfaces for flexibility
- **Normalized Input**: Clean input data structure for movement and actions

### Architecture
- **Component-Based Design**: Modular, single-responsibility components
- **Event-Driven Communication**: Loose coupling through C# events
- **Data-Driven Configuration**: ScriptableObjects for easy tuning
- **Extensible State Machine**: Easy to add new character states
- **SOLID Principles**: Professional software architecture throughout

## Requirements

### Unity Version
- **Unity 6**

### Required Packages
- **Input System** (Unity Input System package) - Optional, Legacy Input Manager also supported
- **Cinemachine** (optional, for advanced camera features)

### Project Settings
- **Physics**: Standard Unity physics settings
- **Input**: Configured for keyboard and mouse input
- **Rendering**: Compatible with Built-in, URP, and HDRP render pipelines

## Architecture Overview

The project follows a **Component-Based, Event-Driven, Data-Driven** architecture that emphasizes modularity, maintainability, and extensibility.

### System Layers

1. **Data Layer**: ScriptableObjects for configuration (movement speeds, jump forces, camera settings)
2. **Core Systems Layer**: Independent systems (Input, Movement, Camera, State Management)
3. **Unity Integration Layer**: MonoBehaviour components that bridge Unity lifecycle with core systems
4. **Utility Layer**: Helper classes for physics, math, and common operations

### Data Flow

```
Input System → State Machine → Movement System → Rigidbody Physics → Camera System
     ↓              ↓                ↓
  Events        Events           Events
```

### Key Principles

- **Separation of Concerns**: Each system has a single, well-defined responsibility
- **Dependency Inversion**: Systems depend on interfaces, not concrete implementations
- **Open/Closed Principle**: Extensible through behaviors and states without modifying core code
- **Loose Coupling**: Systems communicate through events and interfaces
- **High Cohesion**: Related functionality grouped together logically

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── PlayerController.cs          # Main orchestrator component
│   │   └── PlayerState.cs                # State machine/enum definitions
│   │
│   ├── Input/
│   │   ├── IInputProvider.cs            # Input interface abstraction
│   │   ├── UnityInputProvider.cs         # Unity Input System implementation
│   │   ├── LegacyInputProvider.cs        # Legacy Input Manager implementation
│   │   └── InputData.cs                  # Input data structure
│   │
│   ├── Movement/
│   │   ├── MovementSystem.cs            # Core movement logic
│   │   ├── GroundDetector.cs            # Ground detection system
│   │   ├── JumpHandler.cs               # Jump mechanics
│   │   ├── Behaviors/
│   │   │   ├── WalkBehavior.cs          # Walking movement behavior
│   │   │   ├── SprintBehavior.cs        # Sprinting movement behavior
│   │   │   └── AirControlBehavior.cs    # Air movement behavior
│   │   └── Data/
│   │       └── MovementData.cs          # ScriptableObject configuration
│   │
│   ├── Camera/
│   │   ├── CameraController.cs          # Camera control logic
│   │   └── Data/
│   │       └── CameraData.cs            # ScriptableObject configuration
│   │
│   ├── Physics/
│   │   └── PhysicsUtilities.cs          # Physics helper methods
│   │
│   └── Events/
│       └── PlayerEvents.cs               # Event definitions
│
├── Prefabs/
│   └── Player.prefab                     # Pre-configured player prefab
│
└── Scenes/
    └── MainScene.unity                   # Testing/demo scene
```

## Setup Instructions

### 1. Import the Project

1. Open Unity Hub
2. Add the project folder
3. Open the project in Unity

### 2. Configure Input System (Optional)

If using Unity Input System:
1. Go to `Edit > Project Settings > Player`
2. Under "Active Input Handling", select "Input System Package (New)" or "Both"
3. The Input Actions asset is located at `Assets/InputSystem_Actions.inputactions`

### 3. Scene Setup

1. Open `Assets/Scenes/MainScene.unity`
2. The scene includes a pre-configured player GameObject
3. Ensure the player has:
   - `Rigidbody` component (with appropriate mass and drag settings)
   - `CapsuleCollider` or `BoxCollider` component
   - `PlayerController` component attached
   - Camera setup (either Cinemachine or manual camera)

### 4. Player GameObject Configuration

**Required Components:**
- `Rigidbody`
  - Mass: 1
  - Drag: 0-5 (adjust for feel)
  - Angular Drag: 0
  - Freeze Rotation: X, Y, Z (to prevent tumbling)
- `Collider` (CapsuleCollider recommended)
  - Height: 2
  - Radius: 0.5
  - Center: (0, 1, 0)

**PlayerController Component:**
- Assign Movement Data ScriptableObject
- Assign Camera Data ScriptableObject (if using)
- Configure input provider (Unity or Legacy)

### 5. Create Configuration Assets

1. Create Movement Data:
   - Right-click in Project window
   - `Create > PlayerController > Movement Data`
   - Configure movement speeds, jump force, etc.

2. Create Camera Data (if using):
   - Right-click in Project window
   - `Create > PlayerController > Camera Data`
   - Configure camera offset, sensitivity, etc.

### 6. Testing

1. Press Play in the Unity Editor
2. Use WASD for movement
3. Use Mouse for camera rotation
4. Use Spacebar for jumping

## Controls

### Default Controls

| Action | Input |
|--------|-------|
| Move Forward | W / Up Arrow |
| Move Backward | S / Down Arrow |
| Move Left | A / Left Arrow |
| Move Right | D / Right Arrow |
| Jump | Spacebar |
| Sprint | Left Shift (hold) |
| Camera Rotate | Mouse Movement |
| Camera Reset | Middle Mouse Button (optional) |

### Customization

Controls can be customized through:
- Unity Input System: Edit `Assets/InputSystem_Actions.inputactions`
- Legacy Input Manager: Edit `Edit > Project Settings > Input Manager`
- Code: Modify input provider implementations

## Technical Implementation

### Physics-Based Movement

The controller uses Unity's Rigidbody component but applies forces and velocities manually, giving fine control over movement:

```csharp
// Example: Applying movement force
Vector3 moveDirection = transform.forward * inputData.moveInput.y + 
                        transform.right * inputData.moveInput.x;
Vector3 force = moveDirection * movementSpeed * Time.fixedDeltaTime;
rigidbody.AddForce(force, ForceMode.VelocityChange);
```

### Ground Detection

Multiple ground detection methods are implemented:
- **Raycast Detection**: Fast, configurable distance
- **Collision Detection**: More accurate, uses collision callbacks
- **Layer Masking**: Configurable ground layers

### State Management

Character states are managed through a state machine:
- **Grounded**: Normal movement, can jump
- **Airborne**: Limited air control, affected by gravity
- **Jumping**: Transitional state during jump initiation

### Performance Optimizations

Following Unity best practices:
- ✅ Component references cached in `Awake()`
- ✅ Ground detection uses raycasts (not in Update loop unnecessarily)
- ✅ Events used instead of polling
- ✅ Minimal allocations in hot paths
- ✅ FixedUpdate for physics, Update for input

## Design Patterns

### 1. Strategy Pattern
Movement behaviors (walk, sprint, air control) are implemented as swappable strategies, allowing runtime behavior changes.

### 2. Observer Pattern
Systems communicate through C# events:
```csharp
public static event Action OnGrounded;
public static event Action OnJump;
public static event Action OnLand;
```

### 3. State Pattern
Character state machine allows easy extension of new states without modifying existing code.

### 4. Facade Pattern
`PlayerController` acts as a facade, providing a simple interface while coordinating complex subsystems.

### 5. Dependency Injection
Systems receive dependencies through interfaces, enabling testability and flexibility:
```csharp
public interface IInputProvider
{
    InputData GetInput();
}
```

### 6. Component Pattern
Unity's component system enhanced with focused, single-responsibility components.

## Code Examples

### Basic PlayerController Setup

```csharp
using UnityEngine;
using PlayerController.Core;
using PlayerController.Input;
using PlayerController.Movement;

namespace PlayerController
{
    /// <summary>
    /// Main orchestrator for the player control system.
    /// Coordinates input, movement, camera, and state management.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Tooltip("Movement configuration data")]
        private MovementData movementData;
        
        [SerializeField] [Tooltip("Input provider implementation")]
        private IInputProvider inputProvider;
        
        private Rigidbody rb;
        private MovementSystem movementSystem;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            movementSystem = new MovementSystem(rb, movementData);
        }
        
        private void FixedUpdate()
        {
            var input = inputProvider.GetInput();
            movementSystem.ProcessMovement(input, Time.fixedDeltaTime);
        }
    }
}
```

### Movement Data ScriptableObject

```csharp
using UnityEngine;

namespace PlayerController.Movement.Data
{
    /// <summary>
    /// Configuration data for player movement parameters.
    /// Create instances via: Create > PlayerController > Movement Data
    /// </summary>
    [CreateAssetMenu(fileName = "New Movement Data", 
                     menuName = "PlayerController/Movement Data")]
    public class MovementData : ScriptableObject
    {
        [Header("Movement")]
        [Tooltip("Base walking speed")]
        public float walkSpeed = 5f;
        
        [Tooltip("Sprint speed multiplier")]
        public float sprintMultiplier = 1.5f;
        
        [Header("Jump")]
        [Tooltip("Jump force applied to rigidbody")]
        public float jumpForce = 8f;
        
        [Tooltip("Coyote time in seconds (grace period after leaving ground)")]
        public float coyoteTime = 0.2f;
    }
}
```

## Testing Environment

The project includes a test scene (`MainScene.unity`) designed to demonstrate and test all controller features:

### Environment Features
- **Various Terrain Types**: Flat ground, slopes, platforms
- **Jumping Challenges**: Platforms at different heights
- **Movement Testing**: Open areas for sprint and movement testing
- **Camera Testing**: Varied geometry to test camera collision

### Testing Checklist
- [ ] Basic movement (WASD)
- [ ] Jump mechanics
- [ ] Ground detection
- [ ] Camera rotation and follow
- [ ] Sprint functionality
- [ ] Air control
- [ ] Slope handling
- [ ] Edge cases (falling, landing, etc.)

## Development Guidelines

This project follows the Unity C# Game Development guidelines:

### Code Quality
- Clean, readable code with meaningful names
- XML documentation for public APIs
- C# naming conventions (PascalCase public, camelCase private)
- Appropriate access modifiers
- Null checking and error handling

### Unity Best Practices
- Component references cached in `Awake()`
- Coroutines and events preferred over Update() polling
- ScriptableObjects for shared data
- UnityEvents/C# events for loose coupling
- Appropriate lifecycle methods used correctly

### Code Structure
1. Using statements (only what's needed)
2. Namespace declarations
3. Class summary comments
4. Serialized fields with `[SerializeField]` and `[Tooltip]`
5. Public properties for read-only data
6. Unity lifecycle methods in standard order
7. Public methods, then private methods
8. Helper methods at the bottom

## Future Improvements

Potential enhancements for future development:

### Movement Features
- [ ] Wall-running mechanics
- [ ] Slide/crouch system
- [ ] Double jump
- [ ] Dash ability
- [ ] Ledge grabbing

### Camera Features
- [ ] Camera shake effects
- [ ] Dynamic FOV changes
- [ ] Multiple camera presets
- [ ] Camera transitions

### Technical Improvements
- [ ] Unit tests for core systems
- [ ] Performance profiling tools
- [ ] Debug visualization (Gizmos)
- [ ] Animation system integration
- [ ] Audio integration hooks

### Polish
- [ ] Landing impact effects
- [ ] Footstep audio system
- [ ] Particle effects for movement
- [ ] UI feedback systems

## License

This project is created for educational/assessment purposes.

## Acknowledgments

- Built following Unity best practices and software architecture principles
- Designed with extensibility and maintainability in mind
- Implements custom physics for fine-grained control

---

**Note**: This controller is designed as a learning project and assessment submission. It demonstrates understanding of Unity physics, software architecture, and game development best practices.

