namespace BlApi;

internal static class Factory
{
    public static IBl Get => new BlImplementation.Bl();
}
