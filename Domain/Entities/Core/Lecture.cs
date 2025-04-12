using Domain.Entities.Abstracts;
using Domain.Entities.Relationships;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public class Lecture : Entity
{
    public UniqueName Name { get; private set; }
    public string Tempo { get; private set; }
    public Guid ModuleId { get; private set; }
    public Module Module { get; private set; }
    public Url GithubUrl { get; private set; }
    public Video Video { get; private set; }
    public Guid? VideoId { get; private set; }
    public long Views { get; private set; }
    private readonly IList<StudentLecture> _studentLectures = new List<StudentLecture>();
    public IReadOnlyCollection<StudentLecture> StudentLectures => _studentLectures.ToList();

    private Lecture() {}
    public Lecture(UniqueName name, string tempo, Url? githubUrl, Video? video)
    {
        AddNotificationsFromValueObjects(name, githubUrl, video);
        Name = name;
        Tempo = tempo;
        Video = video;
        GithubUrl = githubUrl;
    }

    public void AddModule(Module module)
    {
        if (module is null)
        {
            AddNotification("Module", "Módulo inválido.");
            return;
        }
        ModuleId = module.Id;
        Module = module;
    }

    public void AddView() => Views++;
}

