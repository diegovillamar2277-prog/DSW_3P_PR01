using System.ComponentModel.DataAnnotations;
using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

public class EventoService : IEventoService
{
    private const int DiasMinimosAnticipacion = 15;

    private readonly IEventoRepository _repository;

    public EventoService(IEventoRepository repository)
    {
        _repository = repository;
    }

    public List<Evento> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Evento? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Evento Crear(Evento nuevo)
    {
        Validar(nuevo);
        ValidarFecha(nuevo.Fecha);

        return _repository.Agregar(new Evento
        {
            Nombre = nuevo.Nombre.Trim(),
            Categoria = nuevo.Categoria.Trim(),
            Fecha = nuevo.Fecha,
            Lugar = nuevo.Lugar.Trim(),
            CupoMaximo = nuevo.CupoMaximo,
            Activo = nuevo.Activo
        });
    }

    public void Actualizar(int id, Evento actualizado)
    {
        var actual = _repository.ObtenerPorId(id)
            ?? throw new KeyNotFoundException("El evento no existe.");

        Validar(actualizado);

        // La regla de anticipación solo aplica si se cambia la fecha del evento.
        if (actualizado.Fecha != actual.Fecha)
            ValidarFecha(actualizado.Fecha);

        var datos = new Evento
        {
            Nombre = actualizado.Nombre.Trim(),
            Categoria = actualizado.Categoria.Trim(),
            Fecha = actualizado.Fecha,
            Lugar = actualizado.Lugar.Trim(),
            CupoMaximo = actualizado.CupoMaximo,
            Activo = actualizado.Activo
        };

        if (!_repository.Actualizar(id, datos))
            throw new KeyNotFoundException("El evento no existe.");
    }

    public void Eliminar(int id)
    {
        if (!_repository.Eliminar(id))
            throw new KeyNotFoundException("El evento no existe.");
    }

    // Reglas de negocio
    private static void Validar(Evento evento)
    {
        if (string.IsNullOrWhiteSpace(evento.Nombre))
            throw new ValidationException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(evento.Categoria))
            throw new ValidationException("La categoría es obligatoria.");
        if (string.IsNullOrWhiteSpace(evento.Lugar))
            throw new ValidationException("El lugar es obligatorio.");
        if (evento.CupoMaximo <= 0)
            throw new ValidationException("El cupo máximo debe ser mayor a 0.");
    }

    private static void ValidarFecha(DateTime fecha)
    {
        DateTime hoy = DateTime.Today;

        if (fecha.Date <= hoy)
            throw new ValidationException("No se puede registrar una fecha que ya pasó.");
        if (fecha.Date < hoy.AddDays(DiasMinimosAnticipacion))
            throw new ValidationException(
                $"El evento debe registrarse con al menos {DiasMinimosAnticipacion} días de anticipación.");
    }
}
