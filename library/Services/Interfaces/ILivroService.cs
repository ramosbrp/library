using library.Models.Dto;
using Microsoft.VisualBasic;

namespace library.Services.Interfaces
{
    public interface ILivroService
    {
        Task<List<LivroDto>> GetAllAsync();
        Task<LivroDto> AddAsync(LivroDto livro);
        Task<List<LivroDto>> FilterAsync(int? year, int? month);
        Task<LivroDto> Detail(int codigo);
        Task<LivroDto> UpdateAsync(LivroDto livroDto);
        Task<bool> DeleteLivroAsync(int codigo);
    }
}
