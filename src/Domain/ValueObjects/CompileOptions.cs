using GroupProject.Domain.Enums;

namespace GroupProject.Domain.ValueObjects;

public record CompileOptions(string Code, ProgrammingLanguage Language);
