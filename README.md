# Informe de Decisiones y Modificaciones: Shooter 3D estilo Doom

**Proyecto:** Ampliación de Shooter 3D
**Alumno:** Jorge Rosales Jimenez / 221075135

## 1. Introducción
Como parte de la evaluación del proyecto base "Shooter 3D estilo Doom", se implementaron diversas ampliaciones para demostrar el dominio sobre los sistemas de movimiento, disparo, interfaz y estructuración lógica en Unity. El objetivo principal de las modificaciones fue escalar el sistema de disparo para soportar un arsenal dinámico, permitiendo al jugador alternar entre múltiples armas con distintas características.

## 2. Decisiones de Diseño e Implementación

### Sistema de Cambio de Armas (Inventario)
**Objetivo:** Permitir al jugador cambiar entre un arma principal y un arma secundaria en tiempo real utilizando las teclas numéricas (1 y 2).
**Decisión Técnica:** En lugar de mantener un único script `Disparar.cs` asociado al objeto raíz del jugador (lo cual limitaba las estadísticas a un solo tipo de disparo), se decidió modularizar el sistema. 
- Se trasladó el componente `Disparar` directamente a los modelos 3D (GameObjects) de cada arma hija de la cámara. 
- En el script `PrimeraPersona.cs` se implementó un arreglo `GameObject[] armas` y una función `CambiarArma(int)`. 
- **Justificación:** Esta arquitectura modular permite que cada arma gestione independientemente sus propias variables públicas (cadencia, daño, munición y efectos de destello o *muzzle*). Al presionar 1 o 2, el script simplemente activa (`SetActive(true)`) el GameObject correspondiente y desactiva el resto. Esto hace que el código sea escalable; si en el futuro se desean añadir más armas, basta con agregarlas al arreglo desde el Inspector sin modificar la lógica interna.

### Ajuste en la Lógica de Fin de Juego (GameManager)
**Objetivo:** Asegurar que el jugador no pueda seguir disparando una vez que la partida ha terminado (ya sea por Game Over o Victoria).
**Decisión Técnica:** El script base `GameManager` desactivaba el componente de disparo asumiendo que este residía en la raíz del jugador (`GetComponent<Disparar>()`). Debido al cambio de arquitectura mencionado anteriormente, esto generaba un error (o permitía seguir disparando).
- Se modificó la función `DesactivarJugador()` para utilizar `GetComponentsInChildren<Disparar>(true)`.
- **Justificación:** Al buscar en todos los hijos recursivamente, el GameManager localiza todos los scripts de disparo (incluso los de las armas que están inactivas en ese momento) y los desactiva mediante un bucle `foreach`. Esta decisión robustece el sistema y evita comportamientos indeseados (bugs) al finalizar la partida.

### Diferenciación de Armamento
**Objetivo:** Brindar una experiencia de juego más rica mediante armamento variado.
**Decisión Técnica:** Aprovechando la modularización, se configuraron dos perfiles de armas desde el Inspector de Unity:
1. **Arma Principal:** Cadencia moderada (ej. 0.5s) y daño estándar.
2. **Arma Secundaria:** Configurada como un arma de fuego rápido (ej. cadencia de 0.1s), menor daño por bala, y con su propio objeto visual de *muzzle flash*.
- **Justificación:** Demostrar cómo la exposición correcta de variables en Unity permite alterar significativamente el balance y *gameplay* (cadencia de disparo y daño) sin necesidad de programar un script único para cada tipo de arma. 

## 3. Conclusión
Las modificaciones realizadas transformaron un script monolítico de disparo en un sistema de inventario de armas flexible y escalable. Estas decisiones no solo mejoraron la jugabilidad al ofrecer variedad estratégica al jugador, sino que también aplicaron buenas prácticas de programación orientada a componentes en Unity, asegurando que los sistemas del jugador y del estado del juego (`GameManager`) se comuniquen correctamente bajo la nueva estructura.
