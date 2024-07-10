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

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<LivroDto>>> Filter([FromQuery] int? year, [FromQuery] int? month)
    {
        try
        {
            var livros = await _livroService.FilterAsync(year, month);

            return CreatedAtAction(nameof(Filter), new ApiResponse<List<LivroDto>>(true, "Livros encontrados", livros));

        }
        catch (Exception ex)
        {

            return StatusCode(500, new ApiResponse<string>(false, "Erro ao buscar livros", ex.Message));
        }
    }

    [HttpGet("detail")]
    public async Task<ActionResult<IEnumerable<LivroDto>>> Detail([FromQuery] int codigo)
    {
        try
        {
            var livro = await _livroService.Detail(codigo);
            return CreatedAtAction(nameof(Detail), new ApiResponse<LivroDto>(true, "Livro editado com sucesso.", livro));
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    [HttpPost("edit")]
    public async Task<ActionResult<IEnumerable<LivroDto>>> Edit([FromBody] LivroDto livroDto)
    {
        try
        {

            var livro = await _livroService.UpdateAsync(livroDto);
            return CreatedAtAction(nameof(Detail), new ApiResponse<LivroDto>(true, "Livro editado com sucesso.", livro));
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    // Método DELETE: api/livros/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLivro(int id)
    {
        try
        {
            var success = await _livroService.DeleteLivroAsync(id);
            if (!success)
            {
                return NotFound(new { Message = "Livro não encontrado." });
            }

            return Ok(new { Message = "Livro deletado com sucesso." });
        }
        catch (Exception ex)
        {
            // Log the exception details here using your preferred logging approach
            return StatusCode(500, new { Message = $"Erro interno do servidor: {ex.Message}" });
        }
    }
}
