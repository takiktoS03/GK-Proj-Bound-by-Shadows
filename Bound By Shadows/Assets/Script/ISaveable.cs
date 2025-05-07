public interface ISaveable
{
    object CaptureState();     // zwraca dowoln¹ strukturê danych opisuj¹c¹ stan
    void RestoreState(object state);  // przywraca stan z tej struktury
}
