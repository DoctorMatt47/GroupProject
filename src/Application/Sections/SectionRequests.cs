namespace GroupProject.Application.Sections;

public record CreateSectionRequest(
    string Header,
    string Description);

public record PutSectionRequest(
    int Id,
    string Header,
    string Description);
