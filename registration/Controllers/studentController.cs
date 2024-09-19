using registration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace registration.Controllers
{
    public class studentController : Controller
    {
        // GET: student/registration
        public ActionResult registration()
        {
            return View();
        }

        // POST: student/registration
        [HttpPost]
        public ActionResult registration(User user)
        {
            using (StudentdataEntities DB = new StudentdataEntities())
            {
                if (ModelState.IsValid)
                {
                    // Check if the username already exists
                    bool usernameExists = DB.Users.Any(u => u.Username == user.Username);
                    if (usernameExists)
                    {
                        // Add a model error if the username is already taken
                        ModelState.AddModelError("Username", "Username is already registered.");
                        return View(user); // Return the view with the existing user data to show the error
                    }

                    // Hash the password
                    user.Password = HashPassword(user.Password);

                    // Add user to the database
                    DB.Users.Add(user);
                    DB.SaveChanges();

                    // Redirect to the login page after successful registration
                    return RedirectToAction("login", "student");
                }

                // If model state is not valid, return the view with existing user data
                return View(user);
            }
        }

        // GET: student/login
        public ActionResult login()
        {
            return View();
        }

        // POST: student/login
        [HttpPost]
        public ActionResult login(string username, string password)
        {
            // Hash the password first
            string hashedPassword = HashPassword(password);

            using (StudentdataEntities DB = new StudentdataEntities())
            {
                // Check if the user exists and the password is correct
                var user = DB.Users.SingleOrDefault(u => u.Username == username && u.Password == hashedPassword);

                if (user != null)
                {
                    // User is authenticated, redirect to index view
                    return RedirectToAction("index", "student"); // Assuming "Home" is your main controller and "Index" is the action
                }
                else
                {
                    // Invalid login, add model error
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay the form
            return View();
        }
        public ActionResult index()
        {
            return View();
        }

        // Password hashing method (placeholder for actual implementation)
        private string HashPassword(string password)
        {
            // Use a real hashing algorithm (e.g., BCrypt) in production
            return password; // Replace with actual hashing logic
        }
    }
}