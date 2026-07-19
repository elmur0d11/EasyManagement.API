using AutoMapper;
using EasyManagement.API.Data;
using EasyManagement.API.Dto;
using EasyManagement.API.Exceptions;
using Microsoft.EntityFrameworkCore;
using KeyNotFoundException = EasyManagement.API.Exceptions.KeyNotFoundException;
using UnauthorizedAccessException = EasyManagement.API.Exceptions.UnauthorizedAccessException;

namespace EasyManagement.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AccountService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AccountReadDto> UpdateAccount(int userId, AccountUpdateDto request)
        {
            //Check if the user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new UnauthorizedAccessException("Token not found! Login before using our services!");

            // Updating data
            user.Username = request.Username;
            user.FullName = request.FullName;
            user.Email = request.Email;
            user.Role = request.Role;

            await _context.SaveChangesAsync();

            return _mapper.Map<AccountReadDto>(user);
        }
        public async Task<AccountReadDto> UpdatePassword(int userId, PasswordUpdateDto request)
        {
            // Check if the user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null) throw new UnauthorizedAccessException("Token not found! Login before using our services!");
            // Check if the old password is correct
            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                throw new BadRequestException("Invalid password!");
            // Updating the password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            await _context.SaveChangesAsync();

            return _mapper.Map<AccountReadDto>(user);
        }
    }
}
