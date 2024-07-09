using library.Models;
using Microsoft.AspNetCore.Mvc;
using library.Services;
using library.Services.Interfaces;
using library.Models.Dto;

[ApiController]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivroDto>>> GetLivros()
    {
        var livros = await _livroService.GetAllAsync();
        return Ok(livros);
    }

}
