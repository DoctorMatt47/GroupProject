using GroupProject.Domain.ValueObjects;

namespace GroupProject.WebApi.Requests;

public record CreateCommentaryBody(string Description, CompileOptions CompileOptions);
