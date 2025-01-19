namespace MovieService.Core.Entities;

public class Genre
{
    public Genre(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public ICollection<MovieGenre> MovieGenres { get; private set; } = new List<MovieGenre>();

    public void UpdateName(string name) => Name = name;
    public void UpdateDescription(string description) => Description = description;
}