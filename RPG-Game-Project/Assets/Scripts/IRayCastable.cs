namespace RPG.Control
{
    public interface IRayCastable
    {
        CursorType GetCursorType();
        bool HandleRayCast(PlayerController callingController);
    }
}