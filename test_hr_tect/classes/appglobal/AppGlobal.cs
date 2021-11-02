using appglobal.models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace appglobal
{
    /// <summary>
    /// Class utama untuk mengeset parameter project .netcore
    /// - Author    : Kurniawan
    /// - Modified  : 2018-09-03
    /// </summary>
    public static class AppGlobal
    {
        internal static string BASE_URL = "";
        internal static string LOG_DIR = @"./LOGS";
        internal static string MYSQL_CS = "Server = DESKTOP-G3ALH3L;Database = db_test_hr_tect; integrated security = true";
        internal static string OVERRIDE_CS = "";
        internal static string OVERRIDE_TM = "";
        internal static string PATH_LOGO = "/upload/logo/";
        //internal static string PATH_URAIAN_PEKERJAAN = "/upload/data_uraian_pekerjaan/";
        //internal static string PATH_PROFIL_DEPT_PR = "/upload/data_profil_departemen_pr/";
        //public static string PATH_EXCEL = "/upload/excel/";
        //public static string PATH_EXCEL_STAKEHOLDER = "/upload/data_stakeholder/";
        //public static string PATH_EXCEL_FR = "/upload/excel_fr/";
        //public static string PATH_CONTOH_EXCEL = "/upload/contoh_excel/";
        //public static string PATH_PEMANTAUAN_PROGRAM_KERJA_PR = "/upload/pemantauan_program_kerja_pr/";
        //public static string PATH_LANDING_PAGE_IMG = "/upload/landing_page_img/";
        internal static string OVERRIDE_OS_SERVER = "";
        internal static string DEFAULT_OS_SERVER = "windows";
        public static string Static_Token = "";

        /// <summary>
        /// Get directory path logo sekolah
        /// </summary>
        /// <returns></returns>
        public static string get_logo()
        {
            string file_server = BASE_URL;
            return file_server + PATH_LOGO;
        }

        /// <summary>
        /// Get directory path logo sekolah
        /// </summary>
        /// <returns></returns>
        public static string get_logo_FE()
        {
            string file_server = BASE_URL;
            return PATH_LOGO;
        }


        /// <summary>
        /// Get primary connection string for .netcore project
        /// </summary>
        /// <param name="db_server"></param>
        /// <returns></returns>
        static internal string get_connection_string(string db_server = "MySQL")
        {
            string connection = "";
            if (db_server == "MySQL")
            {
                string file_setting = OVERRIDE_CS;
                connection = file_setting == "" ? MYSQL_CS : file_setting;
            }
            return connection;
        }

        /// <summary>
        /// Get primary working directory for application path searching
        /// </summary>
        /// <returns></returns>
        public static string get_working_directory()
        {
            return BASE_URL; //Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        /// <summary>
        /// Get primary API Server
        /// </summary>
        /// <param name="os_name"></param>
        /// <returns></returns>
        static internal string get_os_server()
        {
            string api_server = OVERRIDE_OS_SERVER == "" ? DEFAULT_OS_SERVER : OVERRIDE_OS_SERVER;
            return api_server;
        }


        /// <summary>
        /// Used in defining which connection to be used in a db_context
        /// </summary>
        /// <returns></returns>
        public static dynamic get_db_option()
        {
            DbContextOptionsBuilder ob = new DbContextOptionsBuilder<test_hr_tect_model>();
            ob.UseSqlServer(get_connection_string());
            return ob.Options;
        }

        /// <summary>
        /// Get session timout value from db
        /// </summary>
        /// <returns></returns>
        public static int get_session_timeout()
        {
            test_hr_tect_model _context = new test_hr_tect_model(AppGlobal.get_db_option()); //simplifying context initializer by override
            int session_timeout = 0;
            try
            {
                session_timeout = Convert.ToInt32(_context.m_parameter.Where(e => e.parameter_key == "Session Timeout").Single().parameter_value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return session_timeout;
        }

        /// <summary>
        /// Standardize console logging
        /// </summary>
        public static void console_log(string name, string content)
        {
            Console.WriteLine("======================================================");
            Console.Write(name + " >> ");
            Console.WriteLine(content);
            Console.WriteLine("======================================================");
        }

        /// <summary>
        /// Get url componen from path
        /// </summary>
        /// <returns></returns>
        public static string get_url_from_path(string path, string part = "feature")
        {
            int part_index = part == "feature" ? 2 : 1;
            string result = "";
            string base_path = "/scope";
            string split_url = path.Substring(base_path.Length);
            string[] urls = split_url.Split("/");
            result = urls[part_index];
            return result;
        }

        /// <summary>
        /// Get id from current logged in user
        /// </summary>
        /// <returns></returns>
        public static int get_user_login_id()
        {
            return Convert.ToInt32(System.Web.HttpContext.Current.User.FindFirst("user_id").Value);
        }

        public static string CapitalizeWords(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Length == 0)
                return value;
            StringBuilder result = new StringBuilder(value);
            result[0] = char.ToUpper(result[0]);
            for (int i = 1; i < result.Length; ++i)
            {
                if (char.IsWhiteSpace(result[i - 1]))
                    result[i] = char.ToUpper(result[i]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Get user validity with defined user_id & feature_id derived from url
        /// </summary>
        /// <returns></returns>
        public static bool get_user_validity()
        {
            test_hr_tect_model _context = new test_hr_tect_model(AppGlobal.get_db_option()); //simplifying context initializer by override

            bool valid = false;
            string path = System.Web.HttpContext.Current.Request.Path;
            string url_feature = get_url_from_path(path, "feature");
            string url_feature_group = get_url_from_path(path, "feature_group");

            int feature_id = 0;

            try
            {
                feature_id = _context.m_feature
                  .Include(e => e.m_feature_group)
                  .Where(e => e.feature_url == url_feature && e.m_feature_group.feature_group_url.ToLower().Replace(" ", "_") == url_feature_group)
                  .Single().m_feature_id;
            }
            catch (Exception e)
            {
                console_log("error feature_id", e.ToString());
            }

            valid = feature_id >= 1 ? true : false;
            return valid;
        }

        public static string convertToRupiah(int angka)
        {
            var new_format = String.Format(CultureInfo.CreateSpecificCulture("id-id"), "{0:N}", angka);
            var index = new_format.IndexOf(",");
            new_format = new_format.Substring(0, index);
            return new_format;
        }

    }
}