﻿namespace GroupProject.Application.Users;

public record UserResponse(
    string Login,
    string Role,
    string Status);