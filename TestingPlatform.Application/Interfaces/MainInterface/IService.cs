namespace TestingPlatform.Application.Interfaces.MainInterface;

public interface IService<TEntityDTO> 
    where TEntityDTO : class
{
    Task<bool> AddAsync(TEntityDTO dTO);

    Task<List<TEntityDTO>> GetAllAsync(int pageNumber, int pageSize, string attribute = "", string order = "asc");

    Task<TEntityDTO> GetAsync(int id);

    Task UpdateAsync(int id, TEntityDTO dto);

    Task<bool> DeleteAsync(int id);
}