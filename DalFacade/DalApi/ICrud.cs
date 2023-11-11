namespace DO;

public interface ICrud<T> where T : class
{
    //all commants are for type dependency, need to be changed.

    int Create(T item);

    T? Read(int id);

    List<T> ReadAll();

    void Update(T item);
    void Delete(int id);
}
