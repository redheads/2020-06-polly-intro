using System;

namespace Demo
{
    public class Demo0
    {
        private readonly IUserService _userService;

        public Demo0(IUserService userService) => _userService = userService;

        public bool IsAdult(int id)
        {
            var age = _userService.GetAge(id); // <- this method is unreliable!

            if (age <= 0)
            {
                throw new ArgumentException("this does not make any sense!");
            }
            
            return age >= 18;
        }
    }

    /// <summary>
    /// We know that the UserService is not very reliable...
    /// </summary>
    public interface IUserService
    {
        // method is called synchronously...
        int GetAge(int id);
    }
}