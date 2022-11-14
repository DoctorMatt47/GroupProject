﻿using GroupProject.Application.Common.Requests;

namespace GroupProject.Application.Commentaries;

public record CreateCommentaryRequest(
    string Description,
    CompileOptionsRequest? CompileOptions,
    Guid TopicId,
    Guid UserId);

public record GetCommentariesRequest(
    PageRequest Page,
    CommentariesOrderedBy OrderBy,
    Guid? TopicId = null,
    Guid? UserId = null);

public enum CommentariesOrderedBy
{
    CreationTime,
    ComplaintCount,
    VerifyBefore,
}
