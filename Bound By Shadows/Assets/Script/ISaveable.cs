public interface ISaveable
{
    object CaptureState();     // zwraca dowoln� struktur� danych opisuj�c� stan
    void RestoreState(object state);  // przywraca stan z�tej struktury
}
