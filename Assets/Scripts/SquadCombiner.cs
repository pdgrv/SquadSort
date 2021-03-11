public static class SquadCombiner
{
    public static bool TryCombineSquads(Squad fromSquad, Squad toSquad)
    {
        if (fromSquad.UnitsType == toSquad.UnitsType)
        {
            CombineSquads(fromSquad, toSquad);
            fromSquad.ClearSquad();
            return true;
        }
        else
            return false;
    }

    private static void CombineSquads(Squad fromSquad, Squad toSquad)
    {
        toSquad.PushSquadFrom(fromSquad);
    }
}
