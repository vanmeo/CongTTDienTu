using Xim.Application.Contracts.Users;
using Xim.Domain.Entities;
using Xim.Domain.Repos;
using Xim.Library.Exceptions;
using Xim.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xim.Library.Constants;
using Xim.Domain.Pagings;
using Microsoft.Extensions.DependencyInjection;

namespace Xim.Application.Services
{
    public class UserService : BaseService<IUserRepo>, IUserService
    {
        public UserService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<List<UserDtoView>> GetByDonviAsync(Guid iddonvi)
        {
            var entitys = await _repo.GetUserByDonViAsync<UserEntity>(iddonvi);
            var data = ClassExtension.Map<UserDtoView>(entitys);
            return data;
        }
        public async Task<UserDtoView> GetAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            var data = ClassExtension.Map<UserDtoView>(entity);
            return data;
        }

        public async Task<List<UserDtoView>> GetAsync()
        {
            var entitys = await _repo.GetsAsync();
            var data = ClassExtension.Map<UserDtoView>(entitys);
            return data;
        }

        public async Task<UserDtoLoggedIn> GetLoginAsync(UserDtoLogin model)
        {
            if (string.IsNullOrWhiteSpace(model?.username))
            {
                throw new BusinessException("ParamInvalid");
            }

            var entity = await _repo.GetLoginAsync(model.username);
            if (entity == null)
            {
                throw new BusinessException("ParamInvalid");
            }

            var password = EncodePassword(model.password);
            if (password != entity.password)
            {
                throw new BusinessException("ParamInvalid");
            }

            if (entity.is_locked == true)
            {
                throw new BusinessException("UserInactive");
            }

            //log tạm thời chưa ghi log đoạn này
            //await this.WriteUserLogAsync(entity.id, UserLogType.Login, new { model.username, model.type });

            var data = ClassExtension.Map<UserDtoLoggedIn>(entity);
            return data;
        }

        public async Task<UserDtoView> CreateAsync(UserDtoCreate model)
        {
            var entity = await _repo.GetAsync<UserEntity>(new Dictionary<string, object>
            {
                { "username", model.username },
                { "fullname", model.fullname },
            });
            if (entity != null)
            {
                throw new BusinessException($"Đã tồn tại user");
            }
            this.ValidatePassword(model.password);

            entity = ClassExtension.Map<UserEntity>(model);
            this.ProcessInsertData(entity);
            entity.password = EncodePassword(entity.password);
            entity.id = Guid.NewGuid();
            await _repo.InsertAsync(entity);
            var result = ClassExtension.Map<UserDtoView>(entity);

            //log
            //await this.WriteUserLogAsync(model.created_userid.Value, UserLogType.UserCreate, entity);

            return result;
        }

        public async Task<UserDtoView> UpdateAsync(UserDtoUpdate model)
        {           
            var entity = await _repo.GetAsync(model.id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }
            var password = entity.password;
            
            ClassExtension.Map(model, entity);
            this.ProcessUpdateData(entity);
            if (string.IsNullOrWhiteSpace(model.password))
            {
                entity.password = password;
            }
            else
            {
                entity.password = EncodePassword(model.password);
                model.password = entity.password;   //update to write log
            }

            await _repo.UpdateAsync(entity);

            ////log
            //await this.WriteUserLogAsync(entity.id, UserLogType.UserUpdate, model);

            var result = ClassExtension.Map<UserDtoView>(entity);
            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repo.GetAsync(id);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }
            
            this.ProcessUpdateData(entity);
            await _repo.DeleteAsync(id);                       
            //await this.WriteUserLogAsync(model.created_userid.Value, UserLogType.UserDelete, new
            //{
            //    model.id
            //});
        }

        /// <summary>
        /// Mã hóa mật khẩu
        /// </summary>
        string EncodePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }

            // byte array representation of that string
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            string encoded = BitConverter.ToString(hash)
               // without dashes
               .Replace("-", string.Empty);
            return encoded;
        }

        /// <summary>
        /// Kiểm tra mật khẩu hợp lệ không
        /// </summary>
        void ValidatePassword(string password)
        {
            if (password?.Length < 6)
            {
                throw new BusinessException("Password must be equal or logner than 6 characters");
            }
        }

        public async Task ChangePasswordAsync(UserDtoChangePassword model)
        {
            var entity = await _repo.GetAsync(model.userid.Value);
            if (entity == null)
            {
                throw new BusinessException("Notfound");
            }

            var encodeOldPassword = this.EncodePassword(model.old_password);
            if (encodeOldPassword != entity.password)
            {
                throw new BusinessException("OldPasswordInvalid");
            }

            this.ValidatePassword(model.new_password);

            entity.password = this.EncodePassword(model.old_password);
            this.ProcessUpdateData(entity);
            await _repo.UpdateAsync(entity.id, entity);

            //pvduy tạm thời comment lại bảng log
            //await this.WriteUserLogAsync(entity.id, UserLogType.UserDelete, new { old_password = encodeOldPassword, password = entity.password });
        }
        public async Task<PagingData> GetListAsync(PagingParam param)
        {
            var entity = await _repo.GetPagingAsync(param);
            var sum = await _repo.GetPagingSummaryAsync(new PagingSummaryParam()
            {
                filter = param.filter
            });
            return new PagingData
            {
                data = entity,
                sumData = sum
            };
        }
    }
}
