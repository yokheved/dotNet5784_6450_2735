namespace DO;

public interface ICrud<T> where T : class
{
    //all commants are for type dependency, need to be changed.

    int Create(T item);
    T? Read(int id);
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null); //stage 2
    void Update(T item);
    void Delete(int id);
    T? Read(Func<T, bool> filter); // stage 2

}
