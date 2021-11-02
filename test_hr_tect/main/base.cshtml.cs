using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using appglobal.models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Web;

namespace appglobal
{
    public class baseModel : PageModel
    {
        static test_hr_tect_model _context = new test_hr_tect_model(AppGlobal.get_db_option()); //simplifying context initializer by override

        public JsonResult OnPost()
        {
            test_hr_tect_model _context = new test_hr_tect_model(AppGlobal.get_db_option()); //simplifying context initializer by override
            AppResponseMessage arm = new AppResponseMessage(); //inisialisasi ARM sebagai standarisasi respon balik
            //var user_group = AppGlobal.get_user_group_login_name();
            //try
            //{
            //    using (var transaction = _context.Database.BeginTransaction())
            //    {
            //        try
            //        {
            //            var user_id = Convert.ToInt32(User.FindFirst("user_id").Value);
            //            string path_folder_upload = AppGlobal.get_profil_dept();
            //            var app_code = System.Web.HttpContext.Current.User.FindFirst("application_code").Value;

            //            //handle kiriman parameter sesuai f >> function, dihandle filternya di ScopePageModel
            //            if (Request.Query["f"] == "get_table_dept")
            //            {
            //                List<dynamic> list_data = new List<dynamic>();

            //                string filter = String.IsNullOrEmpty(Request.Form["search"]) ? "" : Convert.ToString(Request.Form["search"]);
            //                Int32 requestStart = String.IsNullOrEmpty(Request.Form["start"]) ? 0 : Convert.ToInt32(Request.Form["start"]);
            //                Int32 requestLength = String.IsNullOrEmpty(Request.Form["length"]) ? 0 : Convert.ToInt32(Request.Form["length"]);
            //                string order_name = String.IsNullOrEmpty(Request.Form["order_name"]) ? "" : Convert.ToString(Request.Form["order_name"]);
            //                string order_by = String.IsNullOrEmpty(Request.Form["order_by"]) ? "" : Convert.ToString(Request.Form["order_by"]);

            //                var filteredData = (from a in _context.m_unit_kerja
            //                                    where a.m_unit_kerja_status == "Aktif"
            //                                    select a).AsNoTracking();

            //                var totalFilter = filteredData.Count();
            //                var totalRow = filteredData.Count();

            //                if (order_name == "m_unit_kerja_nama" && order_by == "asc")
            //                {
            //                    filteredData = filteredData.OrderBy(p => (p.m_unit_kerja_nama));
            //                }
            //                else if (order_name == "m_unit_kerja_nama" && order_by == "desc")
            //                {
            //                    filteredData = filteredData.OrderByDescending(p => (p.m_unit_kerja_nama));
            //                }

            //                var filteredData_new = filteredData.Select(p => new {
            //                    p.m_unit_kerja_id,
            //                    p.m_unit_kerja_nama,
            //                });

            //                if (!String.IsNullOrWhiteSpace(filter))
            //                {
            //                    filteredData_new = filteredData_new.Where(p =>
            //                        (
            //                            p.m_unit_kerja_nama.ToLower().Contains(filter.ToLower())
            //                        )
            //                    );
            //                    totalFilter = filteredData_new.Count();
            //                }

            //                if (requestLength > 0)
            //                {
            //                    filteredData_new = filteredData_new.Skip(requestStart).Take(requestLength);
            //                }

            //                var data = filteredData_new.ToList();

            //                list_data.Add(new
            //                {
            //                    data = data,
            //                    totalFilter = totalFilter,
            //                    totalRow = totalRow,
            //                });

            //                arm.data = list_data;
            //                arm.success();
            //            }

            //            else if (Request.Query["f"] == "change_notif")
            //            {
            //                var t_notifikasi_id = String.IsNullOrEmpty(Request.Form["t_notifikasi_id"]) ? 0 : Convert.ToInt32(Request.Form["t_notifikasi_id"]);
            //                var data_t_notifikasi = _context.t_notifikasi.Where(e => e.t_notifikasi_id == t_notifikasi_id).SingleOrDefault();

            //                data_t_notifikasi.t_notifikasi_status = "Read";
            //                data_t_notifikasi.t_notifikasi_update_at = DateTime.Now;
            //                data_t_notifikasi.t_notifikasi_update_by = user_id;
            //                _context.t_notifikasi.Update(data_t_notifikasi); //insert m_feature yg diconstruct
            //                _context.SaveChanges(); //save changes to database

            //                arm.success(); //set success status
            //                arm.message = "Data Telah Disetujui"; //set success message
            //            }

            //            else if (Request.Query["f"] == "create_notif")
            //            {
            //                if (@app_code == "MR") // notif MR
            //                {
            //                    var notif_html = create_notif_mr(user_group);
            //                    arm.data = notif_html;
            //                    arm.success(); //set success status
            //                    arm.message = "Data Telah Disetujui"; //set success message
            //                }

            //                else if (@app_code == "FR") // notif Fraud
            //                {
            //                    var notif_html = create_notif_fr(user_group);
            //                    arm.data = notif_html;
            //                    arm.success(); //set success status
            //                    arm.message = "Data Telah Disetujui"; //set success message
            //                }

            //                else if (@app_code == "RP") // notif Fraud
            //                {
            //                    var notif_html = create_notif_pr(user_group);
            //                    arm.data = notif_html;
            //                    arm.success(); //set success status
            //                    arm.message = "Data Telah Disetujui"; //set success message
            //                }
            //            }
            //            transaction.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            arm.fail();
            //            arm.message = ex.Message;
            //            transaction.Rollback();
            //            AppGlobal.console_log("Error Save: ", ex.ToString());
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    arm.fail();
            //    arm.message = ex.Message;
            //    AppGlobal.console_log("Error Save: ", ex.ToString());
            //}
            return new JsonResult(arm);
        }
        public void OnGet()
        {
        }

        /// <summary>
        /// Create primary menu structure based on user authorization
        /// </summary>
        /// <returns></returns>
        public static string create_menu_structure()
        {

            //rfc : edit model & add hidden attribute
            //don't display hidden feature, but grant access to user when mapped
            string string_menu = "";

            Claim user_group_id = System.Web.HttpContext.Current.User.FindFirst("m_user_group_id");
            Claim application_code = System.Web.HttpContext.Current.User.FindFirst("application_code");
            //int m_user_group_id = Convert.ToInt32(user_group_id.Value);
            //string m_application_code =application_code.Value;
            //var authorized_feature_list = _context.map_feature
            //  .Where(e => e.m_user_group_id == m_user_group_id)
            //  .Select(e => e.m_feature_id)
            //  .ToList();

            //var m_application_id = _context.m_application
            //  .SingleOrDefault(e => e.application_code == m_application_code)
            //  .m_application_id;

            var menu_list = _context.m_feature
              .Include(e => e.m_feature_group)
              //.Where(e => authorized_feature_list.Contains(e.m_feature_id) && e.m_feature_group.m_application_id == m_application_id && e.m_feature_group.feature_group_status == "Aktif" && e.feature_status == "Aktif" && e.feature_visible == true)
              .Select(e => new { e.m_feature_id, e.feature_name, e.feature_url, e.feature_icon, e.m_feature_group_id, e.m_feature_group.feature_group_name, e.feature_sequence, e.m_feature_group.feature_group_url, e.m_feature_group.feature_group_squance, e.m_feature_group.feature_group_icon })
              .OrderBy(e => e.m_feature_group_id.ToString("D2") + "-" + e.feature_sequence.ToString("D2"))
              .ToList();

            if (menu_list.Count() > 0)
            {
                var list_feature_group = menu_list.Select(e => new { e.m_feature_group_id, e.feature_group_name, e.feature_group_url, e.feature_group_squance, e.feature_group_icon }).OrderBy(e => e.feature_group_squance).Distinct().ToList();
                foreach (var item in list_feature_group)
                {
                    var list_feature = menu_list.Where(e => e.m_feature_group_id == item.m_feature_group_id).ToList();
                    var string_feature = "";
                    string feature_group_url = item.feature_group_url.ToLower().Replace(" ", "_");
                    string feature_group_icon = item.feature_group_icon;

                    foreach (var menu_list_data in list_feature)
                    {
                        string feature_url = menu_list_data.feature_url;
                        string_feature += @"<li class='nav-item' data-feature_group_url='" + feature_group_url + @"' data-feature_url='" + feature_url + @"'><a class='nav-link menu-item' href='" + feature_group_url + "-" + feature_url + "'>" + menu_list_data.feature_name + @"</a></li>";
                    }

                    string_menu += @"<li class='nav-item'>
                        <a class='nav-link' data-toggle='collapse' href='#" + feature_group_url + @"' aria-expanded='true' aria-controls='ui-basic' style='margin-left:16px;'>
                            <i class='" + feature_group_icon + @" menu-icon'></i>
                            <span class='menu-title'>" + item.feature_group_name + @"</span>
                            <i class=''></i>
                        </a>
                        <div class='collapse' id='" + feature_group_url + @"'>
                            <ul class='nav flex-column sub-menu'>
                                " + string_feature + @"
                            </ul>
                        </div>
                    </li>";

                }
            }
            return string_menu;
        }
    }
}