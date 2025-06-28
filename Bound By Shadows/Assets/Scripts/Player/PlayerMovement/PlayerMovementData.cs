using UnityEngine;

namespace EthanTheHero
{
    /**
     * @class PlayerMovementData
     * @brief ScriptableObject przechowujący dane konfiguracyjne ruchu gracza.
     *
     * Zawiera ustawienia prędkości biegu, przyspieszenia, skoku, daszu oraz interakcji ze ścianami.
     * Obliczenia siły przyspieszenia i wytracania prędkości wykonywane są automatycznie w metodzie `OnValidate()`.
     * Umożliwia łatwe dostosowanie balansu postaci bez modyfikacji kodu.
     */
    [CreateAssetMenu(menuName = "Player Movement Data")]
    public class PlayerMovementData : ScriptableObject
    {
        // ---------------- RUN ----------------

        [Header("Run")]

        /// @brief Docelowa maksymalna prędkość biegu gracza.
        public float runMaxSpeed;

        /// @brief Czas, po którym gracz powinien osiągnąć prędkość maksymalną (z pozycji spoczynkowej).
        public float runAcceleration;

        /// @brief Obliczona siła przyspieszenia (na podstawie `runAcceleration`).
        [HideInInspector] public float runAccelAmount;

        /// @brief Czas potrzebny do wytracenia prędkości (zatrzymania).
        public float runDecceleration;

        /// @brief Obliczona siła wytracenia prędkości (na podstawie `runDecceleration`).
        [HideInInspector] public float runDeccelAmount;

        [Space(10)]

        /// @brief Mnożnik przyspieszenia w powietrzu.
        [Range(0.01f, 1)] public float accelInAir;

        /// @brief Mnożnik wytracenia prędkości w powietrzu.
        [Range(0.01f, 1)] public float deccelInAir;

        /// @brief Czy gracz ma zachowywać pęd przy nagłych zmianach kierunku.
        public bool doConserveMomentum;

        [Space(20)]

        // ---------------- JUMP ----------------

        [Header("Jump")]

        /// @brief Wysokość skoku gracza.
        public float jumpHeight;

        [Space(20)]

        // ---------------- DASH ----------------

        [Header("Dash")]

        /// @brief Siła dasza (poziomy impuls).
        public float dashPower = 30f;

        /// @brief Czas oczekiwania na ponowne użycie dasza.
        public float dashingCoolDown = 1f;

        /// @brief Czas trwania dasza.
        public float dashingTime = 0.2f;

        /// @brief Koszt dasza w punktach staminy.
        public float dashCost = 20f;

        [Space(20)]

        // ---------------- WALL INTERACTION ----------------

        [Header("Wall Sliding and Wall Jumping")]

        /// @brief Odległość od ściany, przy której wykrywana jest możliwość zsuwania się.
        public float wallDistance = 0.05f;

        /// @brief Czas okna wejścia w wall jump.
        [HideInInspector] public float wallJumpTime = 0.2f;

        /// @brief Maksymalna prędkość zsuwania się po ścianie.
        public float wallSlideSpeed = 0.3f;

        /// @brief Siła skoku w pionie przy wall jumpie.
        public float wallJumpingYPower = 6.5f;

        /// @brief Siła skoku w poziomie przy wall jumpie.
        public float wallJumpingXPower = 5f;

        /// @brief Czas trwania animacji/efektu wall jumpa.
        public float WallJumpTimeInSecond = 0.1f;

        /**
         * @brief Automatycznie aktualizuje dane zależne i ogranicza wartości w edytorze Unity.
         */
        private void OnValidate()
        {
            runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
            runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

            // Ograniczenia zakresów
            #region Variable Ranges
            runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
            runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
            #endregion
        }
    }
}
