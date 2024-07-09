using library.Models.Dto;

namespace library.Services.Interfaces
{
    public interface ILivroService
    {
        Task<List<LivroDto>> GetAllAsync();
        Task<LivroDto> AddAsync(LivroDto livro);
    }
}
