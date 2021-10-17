using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Enums;
using Business.Models;
using DataAccess.EntityFramework.Repositories;
using Entities.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public interface IUserService : IServices<UserModel>
    {

    }
    public class UserService : IUserService
    {
        private readonly UserRepositoryBase _userRepository;

        public UserService(UserRepositoryBase userRepository)
        {
            _userRepository = userRepository;
        }
        public Result Add(UserModel model)
        {
            try
            {
                if (_userRepository.GetEntityQuery().Any(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim()))
                    return new ErrorResult("Bu kullanıcı adı daha önceden alınmış!");
                if (_userRepository.GetEntityQuery("UserDetail").Any(u => u.UserDetail.EMail.ToUpper() == model.UserDetail.EMail.ToUpper().Trim()))
                    return new ErrorResult("Mail zaten kullanılıyor!");
                var entity = new User()
                {
                    active = true,
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    RoleId = (int)Roles.User,
                    UserDetail = new UserDetail()
                    {
                        Address = model.UserDetail.Address.Trim(),
                        CountryId = model.UserDetail.CountryId,
                        CityId = model.UserDetail.CityId,
                        EMail = model.UserDetail.EMail.Trim()
                    }
                };
                _userRepository.Add(entity);
                return new SuccesResult();
               

            }
            catch (Exception e)
            {

                return new ExceptionResult(e);
            }
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

            
            _userRepository?.Dispose();
        }

        public IQueryable<UserModel> GetQuery()
        {
            return _userRepository.GetEntityQuery("UserDetail", "Role").Select(u => new UserModel()

            {
                active = u.active,
                Guid = u.Guid,
                Id = u.Id,
                Password = u.Password,
                Role = new RoleModel()
                {
                    Guid = u.Guid,
                    Id = u.Role.Id,
                    Name = u.Role.Name
                },
                RoleId = u.RoleId,
                UserDetail = new UserDetailModel()
                {
                    Address =u.UserDetail.Address,
                    Id = u.UserDetail.Id,
                    Guid = u.UserDetail.Guid,
                    CountryId = u.UserDetail.CountryId,
                    CityId = u.UserDetail.CityId,
                    EMail = u.UserDetail.EMail,
                },
                UserDetailId = u.UserDetailId,
                UserName = u.UserName
                
            }) ;
        }

        public Result Update(UserModel model)
        {
            try
            {
                var entitiy = _userRepository.GetEntityQuery(u => u.UserName.ToUpper() == model.UserName.ToUpper()).SingleOrDefault();          
                entitiy.Password = model.Password;
                _userRepository.Update(entitiy);
               
                return new SuccesResult();
            }
            catch (Exception e)
            {

               return new ExceptionResult(e);
            }
        }
    }
}
