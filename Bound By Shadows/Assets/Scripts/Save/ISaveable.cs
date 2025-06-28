/**
 * @interface ISaveable
 * @brief Interfejs do obsługi zapisu i odczytu stanu obiektów w grze.
 *
 * Każdy obiekt, który implementuje ten interfejs, może być automatycznie zapisany i odtworzony
 * przez globalny system zapisu. Przechowywany stan musi być możliwy do serializacji.
 *
 * Przykład użycia:
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
 * @author Filip Kudła
 */
public interface ISaveable
{
    /**
     * @brief Zbiera dane reprezentujące stan obiektu do zapisania.
     * @return Obiekt serializowalny reprezentujący aktualny stan.
     */
    object CaptureState();

    /**
     * @brief Przywraca stan obiektu na podstawie danych z zapisu.
     * @param state Stan obiektu odczytany z pliku zapisu.
     */
    void RestoreState(object state);
}

