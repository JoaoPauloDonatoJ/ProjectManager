namespace ProjectMannager.API.DTOs
{
    public record CreateColumnDto(string Name);

    public record ColumnResponseDto(int Id, string Name, int Position, int BoardId);

}
