[System.Serializable]
public enum CharactorClassType
{
    N,
    A,
    S,
    End
}
[System.Serializable]
[System.Flags]
public enum CharactorClassTypeMask
{
    N = 1 << CharactorClassType.N,
    A = 1 << CharactorClassType.A,
    S = 1 << CharactorClassType.S,
}