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
    public Lecture(UniqueName name, string tempo, Module module, Url? githubUrl, Video? video)
    {
        AddNotificationsFromValueObjects(name, module, githubUrl, video);
        Name = name;
        Tempo = tempo;
        Module = module;
        Video = video;
        GithubUrl = githubUrl;
    }

    public void AddView() => Views++;

    public void AddVideo(Video video)
    {
        if (video is null)
        {
            AddNotification("Video", "Vídeo inválido.");
            return;
        }
        Video = video;
    }
}

