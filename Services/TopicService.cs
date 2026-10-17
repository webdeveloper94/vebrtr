using WebLab.RTR.Models;

namespace WebLab.RTR.Services;

public interface ITopicService
{
    Task<List<Topic>> GetTopicsAsync();
    Task<Topic?> GetTopicByIdAsync(int id);
    Task InitializeDatabaseAsync();
}

public class TopicService : ITopicService
{
    private readonly ITopicRepository _repository;

    public TopicService() : this(new TopicRepository())
    {
    }

    public TopicService(ITopicRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Topic>> GetTopicsAsync()
    {
        return await _repository.GetAllTopicsAsync();
    }

    public async Task<Topic?> GetTopicByIdAsync(int id)
    {
        return await _repository.GetTopicByIdAsync(id);
    }

    public async Task InitializeDatabaseAsync()
    {
        await _repository.SeedInitialTopicsAsync();
    }
}
