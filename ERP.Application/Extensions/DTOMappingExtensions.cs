using ERP.ApplicationDTO.branch;
using ERP.ApplicationDTO.company;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using ERP.Shared.encryptservice;

namespace ERP.Application.Extensions
{
    /// <summary>
    /// Extension methods for encoding IDs in DTOs using HashId service
    /// This eliminates repetitive ID encoding code across handlers
    /// </summary>
    public static class DTOMappingExtensions
    {
        /// <summary>
        /// Encode all ID fields in UserDTO
        /// </summary>
        public static UserDTO EncodeIds(this UserDTO dto, IHashIdService hashId, User entity)
        {
            dto.UserId = hashId.EncodeId(entity.UserId);
            dto.CompanyId = entity.CompanyId != null
                ? hashId.EncodeId(entity.CompanyId.GetValueOrDefault())
                : null;
            dto.BranchId = entity.BranchId != null
                ? hashId.EncodeId(entity.BranchId.GetValueOrDefault())
                : null;
            dto.Created_By_Id = hashId.EncodeId(entity.Created_By_Id);
            dto.Last_Update_By_Id = hashId.EncodeId(entity.Last_Update_By_Id);
            return dto;
        }

        /// <summary>
        /// Encode all ID fields in CompanyDTO
        /// </summary>
        public static CompanyDTO EncodeIds(this CompanyDTO dto, IHashIdService hashId, Company entity)
        {
            dto.CompanyId = hashId.EncodeId(entity.CompanyId);
            dto.Created_By_Id = hashId.EncodeId(entity.Created_By_Id);
            dto.Last_Update_By_Id = hashId.EncodeId(entity.Last_Update_By_Id);
            return dto;
        }

        /// <summary>
        /// Encode all ID fields in BranchDTO
        /// </summary>
        public static BranchDTO EncodeIds(this BranchDTO dto, IHashIdService hashId, Branch entity)
        {
            dto.BranchId = hashId.EncodeId(entity.BranchId);
            dto.CompanyId = hashId.EncodeId(entity.CompanyId);
            dto.Created_By_Id = hashId.EncodeId(entity.Created_By_Id);
            dto.Last_Update_By_Id = hashId.EncodeId(entity.Last_Update_By_Id);
            return dto;
        }

        /// <summary>
        /// Encode all ID fields in a list of UserDTOs
        /// </summary>
        public static List<UserDTO> EncodeIds(this List<UserDTO> dtoList, IHashIdService hashId, List<User> entities)
        {
            for (int i = 0; i < dtoList.Count && i < entities.Count; i++)
            {
                dtoList[i].EncodeIds(hashId, entities[i]);
            }
            return dtoList;
        }

        /// <summary>
        /// Encode all ID fields in a list of CompanyDTOs
        /// </summary>
        public static List<CompanyDTO> EncodeIds(this List<CompanyDTO> dtoList, IHashIdService hashId, List<Company> entities)
        {
            for (int i = 0; i < dtoList.Count && i < entities.Count; i++)
            {
                dtoList[i].EncodeIds(hashId, entities[i]);
            }
            return dtoList;
        }
    }
}
