using UserManagementService.Entities;
using UserManagementService.Models.User;

namespace UserManagementService.Mappers
{
    public static class UserMapper
    {
        public static UserEntity ToEntity(UserModel model)
        {
            return new UserEntity
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                Salt = model.Salt,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                IsActive = model.IsActive
            };
        }

        public static UserModel ToModel(UserEntity entity)
        {
            var model = new UserModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };

            model.SetPasswordFromHash(entity.PasswordHash, entity.Salt);

            return model;
        }
    }
}