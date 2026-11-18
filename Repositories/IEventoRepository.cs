using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public interface IEventoRepository
{
    List<Evento> ObtenerTodos();
    Evento? ObtenerPorId(int Id);
    Evento Agregar(Evento nuevo);
    bool Actualizar(int id, Evento evento);
    bool Eliminar(int id);
}