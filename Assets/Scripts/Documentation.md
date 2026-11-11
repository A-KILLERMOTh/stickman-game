
# STICKMAN GAME
## Development Notebook

**Engine:** Unity 2D  
**Language:** C#  
**Project:** Stickman Game  
**Repository:** https://github.com/A-KILLERMOTh/stickman-game

---

# 1. PROJECT OVERVIEW

A 2D action platformer featuring a stickman character with movement, aerial abilities, melee combat, shielding and enemy AI.

The project is being developed incrementally using Unity and C#.

The main systems we have worked on are:

- Player movement and physics.
- Jumping and air dashing.
- Attack combos.
- Animation state management.
- Player health.
- Shield and blocking mechanics.
- Enemy movement and chasing.
- Enemy obstacle jumping.

---

# 2. PLAYER MOVEMENT

**Script:** `PlayerMovement.cs`

### Horizontal Movement

Implemented Rigidbody2D-based horizontal movement.

We use velocity to control the player's movement while preserving the vertical component.

```csharp
rb.linearVelocity = new Vector2(
    horizontalInput * moveSpeed,
    rb.linearVelocity.y
);
```

### Jumping

Implemented jumping by controlling the Rigidbody2D's vertical velocity.

```csharp
rb.linearVelocity = new Vector2(
    rb.linearVelocity.x,
    jumpForce
);
```

Important concepts covered:

- Initial jump velocity.
- Gravity and vertical acceleration.
- Ground detection.
- Controlling movement while airborne.

### Maximum Fall Speed

Implemented/discussed limiting the player's downward velocity.

```csharp
if (rb.linearVelocity.y < -maxFallSpeed)
{
    rb.linearVelocity = new Vector2(
        rb.linearVelocity.x,
        -maxFallSpeed
    );
}
```

Important understanding:

- Increasing maximum fall speed allows faster falling.
- It does not increase the initial jump velocity.
- It limits how fast the character can fall.

### Air Dash

Implemented an air dash.

Additional restrictions were added:

- The player cannot jump or ascend again after air dashing.
- Normal movement must not interfere with dash movement.
- Movement logic needs to respect the current action state.

### Movement During Attack Combos

Implemented restrictions to minimize player movement while performing attack combos.

This prevents normal movement input from interfering with the attack sequence.

---

# 3. PLAYER ATTACK SYSTEM

**Script:** `PlayerAttack.cs`

### Attack Combo

Implemented a multi-stage attack combo using Unity's Animator.

The attack system uses animation states to represent the different stages of the combo.

### Animator Integration

Worked with Animator parameters to control attack states and transitions.

Important distinction:

`Animator` and `Animation` are different Unity components.

Animator parameters are accessed through the Animator component.

```csharp
animator.SetInteger("Attack", 1);
animator.SetBool("IsAttacking", true);
animator.SetTrigger("Attack");
```

These are examples of the API, not a claim that all three parameters exist in the current project.

### Problems Encountered

- Attack 2 failing to transition back to Idle.
- Attack sequence becoming stuck.
- Player no longer accepting additional attack input.
- Player not starting in the Idle animation.
- Incorrect use of `Animation.SetInteger()` instead of the Animator API.

We worked on debugging the Animator transitions and parameter handling.

---

# 4. PLAYER HEALTH SYSTEM

**Script:** `PlayerHealth.cs`

Started implementing the player's health and damage-handling system.

The script includes serialized references for components such as:

- Maximum health.
- Animator.
- Collider-related components.

### Health System Concepts

The health system is responsible for tracking the player's health and handling damage interactions.

Basic health logic:

```csharp
currentHealth -= damage;
currentHealth = Mathf.Max(0, currentHealth);
```

We also worked on connecting the health system with shield/block interactions.

### Compiler Error Encountered

```text
CS0246: The type or namespace name
'BlockHitSound' could not be found.
```

This indicated that Unity could not resolve the referenced class.

The issue was still being investigated in our latest discussion.

---

# 5. SHIELD / BLOCK SYSTEM

Worked on implementing the player's shield mechanics.

### Animation Work

- Created/worked on the shield-hit animation.
- Worked on the individual shield hit frames.
- Investigated an electricity symbol appearing in front of the character and obscuring the animation.

### Blocking Interaction

Worked on connecting the shield mechanic with player damage handling.

The intended interaction within the implemented system is to distinguish between a blocked hit and an ordinary damaging hit.

The shield's animation and damage-handling integration were both part of our development work.

---

# 6. ENEMY AI SYSTEM

**Script:** `Enemy.cs`

One of the major systems developed so far.

The enemy implementation includes health-related fields, movement, player chasing and obstacle jumping.

### Enemy Chasing

Implemented enemy movement towards the player's position.

The basic direction calculation is:

```csharp
Vector2 direction =
    (player.position - transform.position).normalized;
```

For horizontal movement, we use the horizontal component of the direction.

The enemy uses the player's position to determine where it needs to move.

### Obstacle Jumping

Implemented enemy jumping over obstacles while chasing the player.

The general behavior:

1. Enemy follows the player.
2. Obstacle detection identifies an obstacle.
3. Enemy checks whether it can jump.
4. Enemy initiates a jump.
5. Enemy continues chasing.

### Jump and Falling Behavior

Worked on calibrating the enemy's jump and falling behavior.

Concepts discussed:

- Jump velocity.
- Gravity.
- Maximum falling speed.
- Ground detection.
- Obstacle detection.
- Movement while airborne.

### Wall-Sticking Bug

Encountered an issue where the enemy sticks against a wall while falling.

This issue was still unresolved in our latest discussion.

We identified that collision behavior and movement logic need to be investigated.

---

# 7. UNITY ANIMATION SYSTEM

Worked extensively with Unity's Animator system.

### Concepts Covered

**Animator Parameters**

Used to communicate gameplay states to animations.

**Animator Transitions**

Control how the character moves between animation states.

**Has Exit Time**

Determines whether a transition waits for the animation to reach a specified exit time.

**Default State**

Determines which animation plays when the Animator starts.

### Animation Debugging

We investigated:

- Incorrect default animation.
- Attack states not transitioning correctly.
- Animations getting stuck.
- Incorrect Animator component references.
- Animation visibility and sprite layering issues.

---

# 8. PHYSICS AND C# CONCEPTS COVERED

These are concepts we discussed while implementing the systems.

| Concept | What we learned |
|---|---|
| Rigidbody2D | Physics-based movement |
| linearVelocity | Controls velocity |
| Vector2 | Represents 2D vectors |
| Normalized vector | Direction with unit magnitude |
| Gravity | Changes vertical velocity over time |
| Maximum fall speed | Limits downward velocity |
| Update() | Frame-based logic |
| FixedUpdate() | Fixed-timestep physics logic |
| Animator | Controls animation states |
| SetInteger() | Sets an Animator integer parameter |
| SetBool() | Sets an Animator boolean parameter |
| SetTrigger() | Activates an Animator trigger |
| SerializeField | Exposes private fields in Inspector |
| Collider2D | Handles 2D collision detection |

### Important Movement Distinction

We discussed the difference between:

**Setting Velocity Directly**

Changes velocity immediately.

**Applying Force**

Changes velocity over time according to the physics simulation.

We also discussed why continuously setting velocity while holding movement input does not work the same way as continuously applying acceleration.

---

# 9. DEVELOPMENT AND VERSION CONTROL

We use Git and GitHub to maintain the project's development history.

Repository:

https://github.com/A-KILLERMOTh/stickman-game

Our workflow includes:

1. Implementing a feature.
2. Testing it in Unity.
3. Debugging issues.
4. Refining the implementation.
5. Committing completed changes.

We have also discussed writing meaningful commit descriptions that explain what a feature actually changes.

---

# 10. DEVELOPMENT STATUS

| System | Recorded Status |
|---|---|
| Player horizontal movement | Implemented |
| Player jumping | Implemented |
| Air movement | Implemented |
| Air dash | Implemented |
| Jump restriction after air dash | Implemented |
| Attack combo | Implemented |
| Movement restriction during attack | Implemented |
| Player health | Worked on |
| Shield hit animation | Worked on |
| Shield and damage integration | Worked on |
| Enemy chasing | Implemented |
| Enemy obstacle jumping | Implemented |
| Enemy jump calibration | Worked on |
| Enemy wall-sticking issue | Unresolved |

---

# 11. DEVELOPMENT PRINCIPLES

These are the important lessons that have come up repeatedly during development.

**1. Understand the Physics.**

Know the difference between position, velocity, acceleration and force before changing movement behavior.

**2. Avoid Conflicting Movement Logic.**

Normal movement, air dash and attack movement must not continuously overwrite each other.

**3. Understand the Animator.**

Animation transitions and parameters are part of gameplay logic, not just visual presentation.

**4. Debug Systematically.**

Identify the reason behind a bug instead of randomly changing values until something works.

**5. Understand the Code Being Written.**

The objective is to become capable of implementing and debugging the systems independently.

**6. Build Incrementally.**

Complete a system, test it, understand it and then move forward.

---

# PROJECT DEVELOPMENT RECORD

This document records the systems and concepts covered during development.

It intentionally excludes features that have only been proposed or discussed but not actually worked on.

**Development philosophy: Build it brick by brick. Understand every brick.**
