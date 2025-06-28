/**
 * @interface ISaveable
 * @brief Interfejs do obs³ugi zapisu i odczytu stanu obiektów w grze.
 *
 * Ka¿dy obiekt, który implementuje ten interfejs, mo¿e byæ automatycznie zapisany i odtworzony
 * przez globalny system zapisu. Przechowywany stan musi byæ mo¿liwy do serializacji.
 *
 * Przyk³ad u¿ycia:
 * @code
 * public class MyObject : MonoBehaviour, ISaveable
 * {
 *     public object CaptureState()
 *     {
 *         return myData;
 *     }
 *
 *     public void RestoreState(object state)
 *     {
 *         myData = (int)state;
 *     }
 * }
 * @endcode
 * 
 * @author Filip Kud³a
 */
public interface ISaveable
{
    /**
     * @brief Zbiera dane reprezentuj¹ce stan obiektu do zapisania.
     * @return Obiekt serializowalny reprezentuj¹cy aktualny stan.
     */
    object CaptureState();

    /**
     * @brief Przywraca stan obiektu na podstawie danych z zapisu.
     * @param state Stan obiektu odczytany z pliku zapisu.
     */
    void RestoreState(object state);
}
