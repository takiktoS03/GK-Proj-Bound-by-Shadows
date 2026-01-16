/**
 * Interfejs definiujący metody zapisu i odtwarzania stanu obiektów w systemie zapisu gry.
 *
 * @author Julia Bigaj
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

