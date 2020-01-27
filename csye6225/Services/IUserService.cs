using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using csye6225.Helpers;
using csye6225.Models;

namespace csye6225.Services
{
    public interface IUserService
    {
        Task<IEnumerable<AccountResponse>> GetAll();
        Task<AccountResponse> Authenticate(string email, string password);
        Task<AccountResponse> Self(string id);
        Task<AccountResponse> Create(AccountCreateRequest req);
        Task<AccountResponse> Update(String id, AccountUpdateRequest req);
        Task<Boolean> CheckIfUserExists(string email);
    }

    public class UserService : IUserService
    {
        private readonly dbContext _context;
        private readonly IMapper _mapper;

        public UserService(dbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountResponse> Authenticate(string email, string password)
        {
            var user = await Task.Run(() => _context.Account.FirstOrDefault(x => x.email_address == email));

            if (user == null)
                return null;

            if(!BCrypt.Net.BCrypt.Verify(password, user.password)) {
                return null;
            }

            return _mapper.Map<AccountResponse>(user.WithoutPassword());
        }

        public async Task<AccountResponse> Self(string id)
        {
            var user = await Task.Run(() => _context.Account.FirstOrDefault(x => x.id.ToString() == id));

            if (user == null)
                return null;

            return _mapper.Map<AccountResponse>(user.WithoutPassword());
        }

        public async Task<IEnumerable<AccountResponse>> GetAll()
        {
            var users = await Task.Run(() => _context.Account.WithoutPasswords());
            return _mapper.Map<List<AccountResponse>>(users);
        }

        public async Task<AccountResponse> Create(AccountCreateRequest req)
        {
            var user = new AccountModel() 
            { 
                id = Guid.NewGuid(),
                first_name = req.first_name,
                last_name = req.last_name,
                password = BCrypt.Net.BCrypt.HashPassword(req.password),
                email_address = req.email_address,
                account_created = DateTime.Now,
                account_updated = DateTime.Now
            };

            _context.Account.Add(user); 
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountResponse>(user.WithoutPassword());
        }

        public async Task<AccountResponse> Update(string id, AccountUpdateRequest req)
        {
            var user = await Task.Run(() => _context.Account.FirstOrDefault(x => x.id.ToString() == id));

            if (user == null)
                return null;

            user.first_name = req.first_name;
            user.last_name = req.last_name;
            user.password = BCrypt.Net.BCrypt.HashPassword(req.password);
            user.account_updated = DateTime.Now;
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountResponse>(user.WithoutPassword());
        }

        public async Task<Boolean> CheckIfUserExists(string email)
        {
            return await Task.Run(() => _context.Account.Any(x => x.email_address.ToLower() == email.ToLower()));
        }
    }
}