/**
 * @interface ISaveable
 * @brief Interfejs do obs�ugi zapisu i odczytu stanu obiekt�w w grze.
 *
 * Ka�dy obiekt, kt�ry implementuje ten interfejs, mo�e by� automatycznie zapisany i odtworzony
 * przez globalny system zapisu. Przechowywany stan musi by� mo�liwy do serializacji.
 *
 * Przyk�ad u�ycia:
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
 * @author Filip Kud�a
 */
public interface ISaveable
{
    /**
     * @brief Zbiera dane reprezentuj�ce stan obiektu do zapisania.
     * @return Obiekt serializowalny reprezentuj�cy aktualny stan.
     */
    object CaptureState();

    /**
     * @brief Przywraca stan obiektu na podstawie danych z zapisu.
     * @param state Stan obiektu odczytany z pliku zapisu.
     */
    void RestoreState(object state);
}
