using ProjectMannager.API.Common;
using ProjectMannager.API.DTOs;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Repositories.Interfaces;

namespace ProjectMannager.API.Services
{
    public class WorkspaceService(IWorkspaceRepository workspaceRepository) : IWorkspaceService
    {
        public async Task<ServiceResult<WorkspaceResponseDto>> CreateAsync(CreateWorkspaceDto dto, int userId)
        {
            var workspace = new Workspace
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId
            };

            await workspaceRepository.AddAsync(workspace);
            await workspaceRepository.SaveChangesAsync();

            var response = new WorkspaceResponseDto(workspace.Id, workspace.Name, workspace.Description, workspace.UserId);
            return ServiceResult<WorkspaceResponseDto>.Ok(response, "Workspace criado com sucesso.");
        }

        public async Task<ServiceResult<IEnumerable<WorkspaceResponseDto>>> GetUserWorkspacesAsync(int userId)
        {
            var workspaces = await workspaceRepository.GetByUserIdAsync(userId);

            var response = workspaces.Select(w => new WorkspaceResponseDto(w.Id, w.Name, w.Description, w.UserId));

            return ServiceResult<IEnumerable<WorkspaceResponseDto>>.Ok(response);
        }
    }
}