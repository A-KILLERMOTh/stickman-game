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
- Individually configurable damage for each combo hit.
- Individually configurable knockback strength for each combo hit.
- Damage and knockback values selected based on the current combo step.

### Player Health
- Player health system implemented.
- Damage reception implemented.
- Hurt animation triggered when receiving unblocked damage.
- Knockback applied when receiving damage.
- Player death system implemented.
- Death state prevents further damage after death.
- Death animation triggered when health reaches zero.
- Death UI activated upon player death.

### Blocking / Shield System
- Shield input implemented using Unity Input System.
- Blocking state controlled through `isBlocking`.
- Shield animation controlled through Animator parameters.
- Blocking reduces incoming knockback.
- Blocked attacks do not reduce player health.
- Block-hit audio implemented using `AudioSource.PlayOneShot()`.
- Blocking persists across multiple incoming attacks until the player releases the shield.
- Blocking logic is handled inside `PlayerHealth.TakeDamage()`.
- Configurable block duration implemented.
- Configurable block cooldown implemented.
- Block duration drains while blocking.
- Cooldown begins after voluntary shield release or guard depletion.

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
- Enemy movement and attack behaviour stop when the player dies.

### Enemy Combat
- Three-step enemy attack combo implemented.
- Combo progression controlled through Animation Events.
- Attack hit detection implemented.
- Directional attack detection implemented.
- Attack damage and knockback implemented.
- Attack cooldown implemented.
- Enemy attack interruption upon receiving damage implemented.
- Enemy attacks stop when the player is dead.

### Enemy Health and Damage
- Enemy damage reception implemented.
- Hurt animation implemented.
- Knockback implemented.
- Enemy combo interruption on receiving damage implemented.
- Enemy receives damage, attack direction and knockback values from player attacks.
- Enemy health reduced according to received damage.

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
- Death

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
- Synchronizing damage with attack impact frames.

---

## 5. Combat Feedback

### Knockback
- Player knockback implemented.
- Enemy knockback implemented.
- Knockback values vary depending on the attack.
- Blocking reduces knockback received.
- Player attacks have individually configurable knockback values per combo hit.

### Hit-Stop
- A centralized hit-stop manager implemented using a singleton-style pattern.
- Hit-stop handled through a coroutine.
- `Time.timeScale` used to freeze gameplay temporarily.
- `WaitForSecondsRealtime` used to allow the freeze duration to complete while time is paused.
- Hit-stop duration can be configured.
- Hit-stop integrated into combat damage interactions.

### Audio
- Block-hit sound implemented.
- Death sound implemented.
- Audio played through `AudioSource.PlayOneShot()`.
- Additional combat sounds still needed.

---

## 6. UI Systems

### Death UI
- Death UI implemented.
- Death text activated when the player dies.
- Death sound triggered on death.
- Restart button implemented.
- Restart button activated upon death.
- Restart button connected to the death UI script through the Button `OnClick()` event.
- Current scene reloads using `SceneManager.LoadScene()`.

---

## 7. Technical Concepts Used

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
- Scene management using `SceneManager`
- Inspector-based configuration using serialized fields
- Git version control and merge resolution

---

## 8. Current Development Status

### Completed
- Player movement
- Jumping
- Air dash
- Player attack combo
- Enemy AI
- Enemy attack combo
- Enemy damage and interruption
- Player damage
- Player death system
- Death animation integration
- Death UI
- Restart system
- Knockback
- Shield / blocking
- Block-hit audio
- Block duration and cooldown
- Hit-stop implementation and integration
- Per-attack player damage balancing
- Per-attack player knockback balancing
- Enemy behaviour stopping after player death

### Current Task
**Combat polish and balancing**

---

## 9. Upcoming Combat Improvements

- Combat sound effects.
- Hit-impact visual effects.
- Combat responsiveness tuning.
- Knockback tuning.
- Enemy aggression and combat balance.
- Improve death and game-over presentation.
- Further combat feedback and polish.