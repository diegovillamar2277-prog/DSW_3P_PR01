using Microsoft.AspNetCore.Mvc;
using DSW_3P_PR02.Models;
using DSW_3P_PR02.Services;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/eventos")]
public class EventosController(IEventoService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Evento>> ObtenerTodos() => Ok(service.ObtenerTodos());

    [HttpGet("{id:int}")]
    public ActionResult<Evento> ObtenerPorId(int id)
    {
        var evento = service.ObtenerPorId(id);
        if (evento is null) return NotFound(new ProblemDetails
        {
            Status = 404, Title = "Evento no encontrado", Detail = "El evento no existe."
        });
        return Ok(evento);
    }

    [HttpPost]
    public ActionResult<Evento> Crear([FromBody] Evento request)
    {
        var evento = service.Crear(request);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = evento.Id }, evento);
    }

    [HttpPut("{id:int}")]
    public IActionResult Actualizar(int id, [FromBody] Evento request)
    {
        service.Actualizar(id, request);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id)
    {
        service.Eliminar(id);
        return NoContent();
    }
}
