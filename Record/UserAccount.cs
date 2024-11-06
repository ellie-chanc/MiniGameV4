using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MiniGameV4.Data;
using MiniGameV4.Model;
using MiniGameV4.Exceptions;

namespace MiniGameV4.Record
{
    internal class UserAccount
    {
        private User user;
        private GameContext gameContext;

        public UserAccount(GameContext context)
        {
            gameContext = context;

            string username = SetName(NameType.Username);

            // get user if already exist in database
            User? u = gameContext.User.Where(x => x.Username == username).FirstOrDefault();

            if (u == null)
            {
                // create and insert new user to database if not already existed
                user = new User();
                user.Username = username;
                user.FirstName = SetName(NameType.FirstName);
                user.LastName = SetName(NameType.LastName);
                gameContext.Add(user);
                gameContext.SaveChanges();
            }
            else
            {
                // make u as current user if already exist in database
                user = u;
            }
        }

        public int GetUserId()
        {
            return user.UserId;
        }

        public string GetUsername()
        {
            return user.Username;
        }

        private string SetName(NameType nameType)
        {
            string? name;
            string pattern = @"^[a-zA-Z0-9]*$";
            Regex re = new Regex(pattern);
            string question = "";

            switch (nameType)
            {
                case NameType.Username:
                    question = "Please enter your username or register a new username: ";
                    break;
                case NameType.FirstName:
                    question = "Please enter your first name: ";
                    break;
                case NameType.LastName:
                    question = "Please enter your last name: ";
                    break;
            }

            while (true)
            {
                Console.Write(question);
                try
                {
                    name = Console.ReadLine();

                    if (string.IsNullOrEmpty(name))
                    {
                        throw new ArgumentException("Input cannot be null or empty.");
                    }

                    if (name.Length >= 15)
                    {
                        throw new ArgumentException("Input cannot be longer than 15 characters.");
                    }

                    if (!re.IsMatch(name))
                    {
                        throw new SpecialCharacterException("Input cannot contain special characters.");
                    }

                    // when no exceptions are thrown
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Invalid input: " + ex.Message);
                }
                catch (SpecialCharacterException ex)
                {
                    Console.WriteLine("Invalid input: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input: " + ex.Message);
                }
            }

            return name;
        }
    }
}
