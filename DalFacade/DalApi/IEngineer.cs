namespace DO;
public interface IEngineer : ICrud<Engineer>
{
    public void Deconstruct(Engineer? e, out int id, out string? name, out string? email, out int? level, out double? cost);
}

