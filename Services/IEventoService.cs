using DSW_3P_PR02.Models;
namespace DSW_3P_PR02.Services;

public interface IEventoService
{
    List<Evento> ObtenerTodos();
    Evento? ObtenerPorId(int id);
    Evento Crear(Evento nuevo);
    void Actualizar(int id, Evento actualizado);
    void Eliminar(int id);
}
