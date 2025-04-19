using Domain.Entities.Abstracts;
using Domain.Entities.Core;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public class Course : Entity
{
    public UniqueName Name { get; private set; }
    public Description Description { get; private set; }
    public BigString AboutDescription { get; private set; }
    public decimal Price { get; private set; }
    public decimal TotalHours { get; private set; }
    public Url NotionUrl { get; private set; }
    public Url? GitHubUrl { get; private set; }
    public IA IA { get; private set; }
    public Guid IAid {get; private set;}
    public Video Trailer { get; private set; }
    public  Guid? TrailerId { get; private set; }
    public Parameter? Parameters { get; private set; }
    public Guid? ParameterId { get; private set; }
    public Category Category { get; private set; }
    public Guid CategoryId { get; set; }
    public Picture Image { get; private set; }
    public Guid PictureId {get; private set;}
    public Teacher Teacher { get; private set; }
    public Guid TeacherId { get; private set; }
    public long Subscribes { get; private set; }

    private readonly IList<Module> _modules = new List<Module>();
    public IReadOnlyCollection<Module> Modules => _modules.ToList();
    
    private Course() {}
    public Course(UniqueName name, Description description,
        BigString about, decimal price, decimal totalHours, Url? notionUrl, IA ia,
        Video trailer, Category category, Teacher teacher, Picture? image)
    {
        AddNotificationsFromValueObjects(name, description, about, notionUrl, trailer);
        Name = name;
        Description = description;
        AboutDescription = about;
        Price = price;
        TotalHours = totalHours;
        NotionUrl = notionUrl;
        IA = ia;
        Trailer = trailer;
        Category = category;
        Teacher = teacher;
        Image = image;
    }

    public void AddModule(Module module)
    {
        if (module is null)
        {
            AddNotification("Module", "Módulo inválido.");
            return;
        }

        _modules.Add(module);
    }
    public void AddSubscribe() => Subscribes++;
}
