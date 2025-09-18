using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xim.Domain.Entities;
using Xim.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Xim.Library.Constants;
using System.Globalization;

namespace Xim.Application.Services
{
    public abstract class BaseService
    {
        protected readonly IServiceProvider _serviceProvider;
        public BaseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public string ProcessTenLink(string input)
        {
            // Loại bỏ dấu tiếng Việt
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            // Thay khoảng cách bằng dấu gạch ngang
            result = result.Replace(' ', '-');

            return result.ToLower();
        }
        public string ProcessMa(string input)
        {
            // Loại bỏ dấu tiếng Việt
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            // Thay khoảng cách bằng dấu gạch ngang
            result = result.Replace(" ", "");

            return result.ToLower();
        }
        /// <summary>
        /// Xử lý 1 số thông tin trước khi thêm mới
        /// </summary>
        protected virtual void ProcessInsertData(object entity)
        {
            if (entity is IEntityKey<Guid> entityKeyGuid
                && entityKeyGuid.id == Guid.Empty)
            {
                entityKeyGuid.id = Guid.NewGuid();
            }

            if (entity is IEntityCreated entityCreated
                && (entityCreated.created == null || entityCreated.created == default(DateTime)))
            {
                
                    entityCreated.created = DateTime.Now;
              
            }

            if (entity is IEntityModifed entityModifed
                && (entityModifed.modified == null || entityModifed.modified == default(DateTime)))
            {
                entityModifed.modified = DateTime.Now;
            }
        }

        /// <summary>
        /// Xử lý 1 số thông tin trước khi update
        /// </summary>
        protected virtual void ProcessUpdateData(object entity)
        {
            if (entity is IEntityModifed entityModifed
                && (entityModifed.modified == null || entityModifed.modified == default(DateTime)))
            {
                entityModifed.modified = DateTime.Now;
            }
        }
    }

    public abstract class BaseService<TRepo> : BaseService
    {
        protected readonly TRepo _repo;
        public BaseService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _repo = serviceProvider.GetService<TRepo>();
        }

        ///// <summary>
        ///// Ghi nhật ký người dùng
        ///// </summary>
        ///// <param name="userId">user name</param>
        ///// <param name="type">hành động gì</param>
        ///// <param name="data">dữ liệu đi kèm</param>
        //protected async Task WriteUserLogAsync(Guid userId, UserLogType type, object data = null)
        //{
        //    if (_repo is IUserLogRepo)
        //    {
        //        //Không ghi log cho service của repo này
        //        return;
        //    }

        //    var entity = new UserLogEntity
        //    {
        //        id = Guid.NewGuid(),
        //        created = DateTime.Now,
        //        user_id = userId,
        //        type = type.ToString(),
        //    };

        //    if (data != null)
        //    {
        //        if (data is string stringData)
        //        {
        //            entity.data = stringData;
        //        }
        //        else
        //        {
        //            entity.data = JsonConvert.SerializeObject(data);
        //        }
        //    }

        //    var repo = _serviceProvider.GetService<IUserLogRepo>();
        //    await repo.InsertAsync(entity);
        //}
    }
}
