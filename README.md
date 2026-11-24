# Squash Package

Dépôt du package Unity **Squash Feedback** (`com.suussonic.squash`) : un effet **Squash & Stretch**, applicable sur n'importe quel `Transform`.

```csharp
using Suussonic.Squash;

transform.Squash(force: 0.4f, iterations: 3, duration: 0.5f);
```

**Documentation complète du package : [com.suussonic.squash/README.md](./com.suussonic.squash/README.md)**

---

## Contenu du dépôt

| Dossier                                            | Description                                             |
|----------------------------------------------------|---------------------------------------------------------|
| [`com.suussonic.squash/`](./com.suussonic.squash)  | Le package UPM (code source, documentation, licence).   |
| `My project/`                                      | Projet Unity de test / développement du package.        |

---

## Installation dans votre projet

Dans Unity : **Window → Package Manager → Add package from git URL...** puis collez :

```
https://github.com/Suussonic/squash-package.git?path=com.suussonic.squash
```

Ou directement dans `Packages/manifest.json` :

```json
{
  "dependencies": {
    "com.suussonic.squash": "https://github.com/Suussonic/squash-package.git?path=com.suussonic.squash"
  }
}
```

> **Prérequis :** Unity **2022.3** ou supérieur.

---

## Aperçu de l'API

```csharp
// Écrase l'objet sur l'axe Y (par défaut) : 3 rebonds amortis en 0.5 s.
transform.Squash(force: 0.4f, iterations: 3, duration: 0.5f);

// Avec toutes les options : axe, temps non-scalé, callback de rebond.
transform.Squash(0.5f, 2, 0.4f, SquashAxis.X, unscaledTime: true, onBounce: () => audioSource.Play());

// Interrompt l'effet et restaure l'échelle d'origine.
transform.StopSquash();
```

Le composant interne `SquashRunner` est ajouté automatiquement.

Consultez la [documentation détaillée](./com.suussonic.squash/README.md) pour la référence API complète, le fonctionnement interne et plus d'exemples.

---

## Licence

Distribué sous licence **MIT** — voir [com.suussonic.squash/LICENSE.md](./com.suussonic.squash/LICENSE.md).