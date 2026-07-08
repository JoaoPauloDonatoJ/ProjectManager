namespace ProjectMannager.API.DTOs
{
    public record CreateBoardDto(string Name, string? Description);

    public record BoardResponseDto(int Id, string Name, string? Description, int WorkspaceId);
}
