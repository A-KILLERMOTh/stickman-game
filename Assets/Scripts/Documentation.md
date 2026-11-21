# Stickman Game – Development Notes

## 1. Project Overview

A fast-paced 2D combat game built in Unity, focused on responsive movement, aggressive combat, combo attacks, blocking, knockback and impactful combat feedback.

---

## 2. Player Systems

### Player Movement
- Horizontal movement implemented.
- Facing direction tracked and used by combat systems.
- Jumping implemented.
- Air dash implemented.
- Movement restrictions during attack combos implemented.
- Knockback system implemented.

### Player Combat
- Three-step attack combo system implemented.
- Animator controlled using `AttackIndex`.
- Attack inputs can be queued during designated combo windows.
- Combo progression controlled through Animation Events.
- Attack hit detection implemented using `Physics2D.OverlapCircleAll()`.
- Attack point adjusts based on player facing direction.
- Attack cooldown implemented.

### Player Health
- Player health system implemented.
- Damage reception implemented.
- Hurt animation triggered when receiving unblocked damage.
- Knockback applied when receiving damage.
- Death function currently logs a message and requires further implementation.

### Blocking / Shield System
- Shield input implemented using Unity Input System.
- Blocking state controlled through `isBlocking`.
- Shield animation controlled through Animator parameters.
- Blocking reduces incoming knockback.
- Blocked attacks do not reduce player health.
- Block-hit audio implemented using `AudioSource.PlayOneShot()`.
- Blocking persists across multiple incoming attacks until the player releases the shield.
- Blocking logic is handled inside `PlayerHealth.TakeDamage()`.

---

## 3. Enemy Systems

### Enemy Movement and AI
- Player detection implemented.
- Enemy follows the player.
- Enemy stopping distance implemented.
- Enemy facing direction implemented.
- Obstacle detection implemented.
- Jumping over obstacles implemented.
- Ground detection implemented.

### Enemy Combat
- Three-step enemy attack combo implemented.
- Combo progression controlled through Animation Events.
- Attack hit detection implemented.
- Directional attack detection implemented.
- Attack damage and knockback implemented.
- Attack cooldown implemented.
- Enemy attack interruption upon receiving damage implemented.

### Enemy Health and Damage
- Enemy damage reception implemented.
- Hurt animation implemented.
- Knockback implemented.
- Enemy combo interruption on receiving damage implemented.

---

## 4. Animation Systems

### Player Animations
- Idle
- Movement
- Jump
- Fall
- Dash
- Attack 1
- Attack 2
- Attack 3
- Hurt
- Shield / Blocking

### Enemy Animations
- Locomotion
- Attack 1
- Attack 2
- Attack 3
- Hurt

### Animation Events
Animation Events are used to synchronize gameplay actions with animation frames.

Examples:
- Attack hit detection.
- Combo progression.
- Combo completion.

---

## 5. Combat Feedback

### Knockback
- Player knockback implemented.
- Enemy knockback implemented.
- Knockback values vary depending on the attack.
- Blocking reduces knockback received.

### Hit-Stop
- A centralized hit-stop manager implemented using a singleton-style pattern.
- Hit-stop handled through a coroutine.
- `Time.timeScale` used to freeze gameplay temporarily.
- `WaitForSecondsRealtime` used to allow the freeze duration to complete while time is paused.
- Hit-stop duration can be configured.
- Hit-stop integrated into combat damage interactions.

### Audio
- Block-hit sound implemented.
- Audio played through `AudioSource.PlayOneShot()`.
- Additional combat sounds still needed.

---

## 6. Technical Concepts Used

- Unity Animator Controllers
- Animator Parameters
- Animation Events
- Rigidbody2D
- Collider2D
- Physics2D overlap detection
- LayerMasks
- Unity Input System
- Coroutines
- Time.timeScale
- WaitForSecondsRealtime
- Singleton-style access
- Encapsulation using properties
- Git version control and merge resolution

---

## 7. Current Development Status

### Completed
- Player movement
- Jumping
- Air dash
- Player attack combo
- Enemy AI
- Enemy attack combo
- Enemy damage and interruption
- Player damage
- Knockback
- Shield / blocking
- Block-hit audio
- Hit-stop implementation and integration

### Current Task
**Block Duration / Guard Management**

Planned:
- Limited continuous guard duration.
- Guard depletion.
- Guard break.
- Recovery period.
- Recharging guard after recovery.
- Voluntary shield release without guard break.

---

## 8. Upcoming Combat Improvements

- Guard duration and guard break.
- Combat sound effects.
- Hit-impact visual effects.
- Combat responsiveness tuning.
- Knockback tuning.
- Enemy aggression and combat balance.
- Additional death and game-over behavior.
