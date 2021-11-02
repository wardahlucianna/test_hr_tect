using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using appglobal.models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace appglobal
{
    public class loginModel : PageModel
    {
        test_hr_tect_model _context = new test_hr_tect_model(AppGlobal.get_db_option()); //simplifying context initializer by override

        public void OnGet()
        {
        }

        /// <summary>
        /// Handle onPost Login AJAX based dengan return value JsonResult
        /// </summary>
        /// <param name="request_parameter"></param>
        /// <param name="returnURL"></param>
        /// <returns></returns>
        public JsonResult OnPost(string request_parameter, string returnURL = null)
        {
            dynamic login_object = JsonConvert.DeserializeObject(request_parameter);
            string user_name = login_object["username"];
            string password = login_object["password"];
            //string application_code = login_object["application_code"];
            //Console.WriteLine("user_name>> " + user_name);
            //Console.WriteLine("password >> " + password);
            AppResponseMessage arm = new AppResponseMessage();

            if (!string.IsNullOrWhiteSpace(user_name) && !string.IsNullOrWhiteSpace(password))
            {
                //jika masukan username & password valid
                if (IsValidLogin(user_name, password))
                {
                    ////jika username & password dikenali
                    var user_id = _context.m_employe.Where(f => f.m_employe_email == user_name).FirstOrDefault().m_employe_id;


                    //jika user valid & aktif
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, user_name),
                        //new Claim(ClaimTypes.Role, Role),

                        new Claim("user_id", user_id.ToString()),
                        //new Claim("m_user_group_id", m_user_group_id.ToString()),
                        new Claim("user_name", user_name),
                        //new Claim("application_code", application_code),
                    };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    AuthenticationHttpContextExtensions.SignInAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    arm.success();
                    arm.message = "login berhasil";
                }
                else
                {
                    arm.fail();
                    arm.message = "login gagal";
                }
            }
            else
            {
                arm.fail();
                arm.message = "login gagal";
            }
            return new JsonResult(arm);
        }

        /// <summary>
        /// Handle pencocokan identity login di database
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal bool IsValidLogin(string user_name, string password)
        {
            bool hasil = false;
            try
            {
                var user_exsis = _context.m_employe.Any(f => f.m_employe_email == user_name);

                if (user_exsis)
                {
                    string passInDB = _context.m_employe.Where(u => u.m_employe_email == user_name).FirstOrDefault().m_employe_password;
                    password = create_password_SHA256(password);
                    hasil = password == passInDB ? true : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return hasil;
        }

        /// <summary>
        /// Get all feature user has access to
        /// </summary>
        /// <param name="m_user_group_id"></param>
        /// <returns></returns>
        private string getFeatureMap(int m_user_group_id)
        {
            string result = "";
            //List<map_feature> feature_map_list;
            //feature_map_list = _context.map_feature.Where(a => a.m_user_group_id == m_user_group_id).ToList();
            //result = Newtonsoft.Json.JsonConvert.SerializeObject(feature_map_list);
            //Console.WriteLine(result);
            return result;
        }

        public static string create_password_SHA256(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}