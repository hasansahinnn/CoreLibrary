using Core.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core
{
	public class UserEntity:BaseEntity
	{

		[Encrypted]
		public string Username { get; set; }

		[Encrypted]
		public string Password { get; set; }

		public int Age { get; set; }

        public UserEntity(string username, string password, int age)
        {
            Username = username;
            Password = password;
            Age = age;
        }
    }
	
}
