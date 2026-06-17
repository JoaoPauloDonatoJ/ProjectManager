namespace ProjectMannager.API.DTOs
{
    public record CreateWorkspaceDto(string Name, string? Description);

    public record WorkspaceResponseDto(int Id, string Name, string? Description, int UserId);
}
