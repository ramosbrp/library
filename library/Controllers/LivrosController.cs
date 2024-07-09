using library.Models;
using Microsoft.AspNetCore.Mvc;
using library.Services;
using library.Services.Interfaces;
using library.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;

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
        try
        {
            var livros = await _livroService.GetAllAsync();

            return CreatedAtAction(nameof(GetLivros), new ApiResponse<List<LivroDto>>(true, "Livro encontrados", livros));

        }
        catch (Exception ex)
        {

            return StatusCode(500, new ApiResponse<string>(false, "Erro ao buscar livros", ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<LivroDto>>> PostLivro([FromBody] LivroDto livroDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var livro = await _livroService.AddAsync(livroDto);
            return CreatedAtAction(nameof(PostLivro), new ApiResponse<LivroDto>(true, "Livro cadastrado com sucesso", livro));

        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>(false, "Erro ao cadastrar livros", ex.Message));
        }
    }

}
