# Squash Feedback

Appliquez des déformations **Squash & Stretch** à n'importe quel `Transform`.

Idéal pour donner du "juice" à vos jeux : impacts, sauts, atterrissages, clics sur des boutons UI, collectibles, etc.

```csharp
transform.Squash(force: 0.4f, iterations: 3, duration: 0.5f);
```

---

## Fonctionnalités

- **API en une ligne** — méthode d'extension disponible directement sur `Transform`, sans configuration préalable.
- **Préservation du volume** — lorsque l'objet s'écrase sur un axe, il s'étire sur les deux autres.
- **Amortissement automatique** — l'oscillation s'atténue progressivement jusqu'au retour à l'échelle d'origine.
- **Choix de l'axe** — déformation sur l'axe `X`, `Y`, `Z` ou `All`.
- **Compatible avec les pauses** — l'option `unscaledTime` permet d'ignorer `Time.timeScale`.
- **Callback `onBounce`** — invoqué à chaque rebond, idéal pour déclencher un son ou des particules.
- **Retour automatique à l'échelle initiale** — même si l'objet est désactivé pendant l'effet.

---

## Installation

### Avec le Package Manager (Git URL)

1. Ouvrez **Window → Package Manager**.
2. Sélectionnez **Add package from git URL...**.
3. Collez cette URL :

```
https://github.com/Suussonic/squash-package.git?path=com.suussonic.squash
```

### Via le `manifest.json`

Ajoutez cette ligne dans `Packages/manifest.json` :

```json
{
  "dependencies": {
    "com.suussonic.squash": "https://github.com/Suussonic/squash-package.git?path=com.suussonic.squash"
  }
}
```

> **Prérequis :** Unity **2022.3** ou supérieur.

---

## Démarrage rapide

```csharp
using UnityEngine;
using Suussonic.Squash;

public class Exemple : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Écrase l'objet verticalement : 3 rebonds en 0.5 seconde.
        transform.Squash(force: 0.4f, iterations: 3, duration: 0.5f);
    }
}
```

Le composant interne `SquashRunner` est ajouté automatiquement dès le premier appel : aucune configuration supplémentaire n'est nécessaire.

---

## Référence API

### `Squash(...)`

```csharp
public static void Squash(
    this Transform target,
    float force,
    int iterations,
    float duration,
    SquashAxis axis = SquashAxis.Y,
    bool unscaledTime = false,
    Action onBounce = null
);
```

| Paramètre      | Type         | Défaut          | Description                                                                 |
|----------------|--------------|-----------------|-----------------------------------------------------------------------------|
| `force`        | `float`      | —               | Intensité de la déformation (ex. `0.4` représente une variation limitée à ±95 %). |
| `iterations`   | `int`        | —               | Nombre d'oscillations réalisées pendant l'effet.                            |
| `duration`     | `float`      | —               | Durée totale de l'effet en secondes.                                        |
| `axis`         | `SquashAxis` | `SquashAxis.Y`  | Axe principal de l'écrasement : `X`, `Y`, `Z` ou `All`.                           |
| `unscaledTime` | `bool`       | `false`         | Si `true`, utilise `Time.unscaledDeltaTime` et ignore `Time.timeScale`.      |
| `onBounce`     | `Action`     | `null`          | Callback invoqué à chaque demi-cycle de l'effet.                            |

> L'appel est ignoré si `duration <= 0` ou `force == 0`.

### `Pump(...)`
```csharp
public static void Pump(
    this Transform target, 
    float force, 
    int iterations, 
    float duration, 
    SquashAxis axis = SquashAxis.Y, 
    bool unscaledTime = false, 
    Action onBounce = null 
    );
```

Déclenche un effet d'écrasement sur l'objet ciblé avec les paramètres spécifiés. L'effet se répète autant de fois que nécessaire pour atteindre la durée totale spécifiée.

> L'appel est ignoré si `duration <= 0` ou `force == 0`.

### `StopSquash()`

```csharp
public static void StopSquash(this Transform target);
```

Interrompt immédiatement l'effet en cours et restaure l'échelle d'origine du `Transform`.

### `SquashAxis`

```csharp
public enum SquashAxis { X, Y, Z, All }
```

Axe principal de l'écrasement. Les deux autres axes s'étirent à hauteur de la moitié du facteur appliqué pour conserver une déformation équilibrée.

---

## Fonctionnement interne

1. Au premier appel de `Squash()`, un **`SquashRunner`** est ajouté au `GameObject`. Ce composant, masqué du menu *Add Component*, mémorise le `localScale` d'origine.
2. Une coroutine anime l'échelle avec une **sinusoïde amortie** :

   ```
   facteur = sin(progression × iterations × 2π) × force × (1 − progression)
   ```

3. L'axe choisi est multiplié par `(1 − facteur)` tandis que les deux autres axes compensent avec `(1 + facteur × 0.5)`, pour produire un effet *squash & stretch*.
4. À chaque demi-cycle de la sinusoïde, le callback `onBounce` est invoqué.
5. À la fin de la durée (ou sur `StopSquash()` / `OnDisable`), l'échelle d'origine est restaurée.

---

## Exemples

### Impact au sol avec un son

```csharp
void OnCollisionEnter(Collision collision)
{
    transform.Squash(
        force: 0.5f,
        iterations: 2,
        duration: 0.4f,
        axis: SquashAxis.Y,
        onBounce: () => audioSource.Play()
    );
}
```

### Feedback de bouton UI (fonctionne en pause)

```csharp
public void OnButtonClick()
{
    // unscaledTime = true → l'animation joue même si Time.timeScale = 0
    buttonTransform.Squash(0.3f, 1, 0.25f, SquashAxis.Y, unscaledTime: true);
}
```

### Écrasement latéral

```csharp
transform.Squash(force: 0.6f, iterations: 1, duration: 0.3f, axis: SquashAxis.X);
```

### Arrêter un effet en cours

```csharp
transform.StopSquash(); // restaure instantanément l'échelle d'origine
```

---

## Structure du package

```
com.suussonic.squash/
├── Runtime/
│   ├── SquashExtensions.cs   // Méthodes d'extension Squash() / StopSquash()
│   └── SquashRunner.cs       // Composant interne gérant la coroutine d'animation
├── Editor/
├── Samples~/
│   └── DemoSquash/           // Scène de démonstration (objets 3D et UI)
├── package.json
└── LICENSE.md
```

---

## Licence

Distribué sous licence **MIT**. Voir [LICENSE.md](./LICENSE.md).
