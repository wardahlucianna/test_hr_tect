using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using appglobal.models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Web;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

namespace appglobal
{
    public class m_companyModel : ScopePageModel
    {

        public JsonResult OnPost()
        {
            AppResponseMessage arm = new AppResponseMessage(); //inisialisasi ARM sebagai standarisasi respon balik
            test_hr_tect_model _context = new test_hr_tect_model(AppGlobal.get_db_option()); //simplifying context initializer by override
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var user_id = Convert.ToInt32(User.FindFirst("user_id").Value);
                        string path_folder_logo = AppGlobal.get_logo();

                        //handle kiriman parameter sesuai f >> function, dihandle filternya di ScopePageModel
                        if (Request.Query["f"] == "get_table_admin")
                        {
                            List<dynamic> list_data = new List<dynamic>();

                            string filter = String.IsNullOrEmpty(Request.Form["search"]) ? "" : Convert.ToString(Request.Form["search"]);
                            Int32 requestStart = String.IsNullOrEmpty(Request.Form["start"]) ? 0 : Convert.ToInt32(Request.Form["start"]);
                            Int32 requestLength = String.IsNullOrEmpty(Request.Form["length"]) ? 0 : Convert.ToInt32(Request.Form["length"]);
                            string order_name = String.IsNullOrEmpty(Request.Form["order_name"]) ? "" : Convert.ToString(Request.Form["order_name"]);
                            string order_by = String.IsNullOrEmpty(Request.Form["order_by"]) ? "" : Convert.ToString(Request.Form["order_by"]);

                            var filteredData = (from a in _context.m_company
                                                select a).AsNoTracking();

                            var totalFilter = filteredData.Count();
                            var totalRow = filteredData.Count();

                            if (order_name == "m_company_name" && order_by == "asc")
                            {
                                filteredData = filteredData.OrderBy(p => (p.m_company_name));
                            }
                            else if (order_name == "m_company_name" && order_by == "desc")
                            {
                                filteredData = filteredData.OrderByDescending(p => (p.m_company_name));
                            }

                            else if (order_name == "m_company_email" && order_by == "asc")
                            {
                                filteredData = filteredData.OrderBy(p => (p.m_company_email));
                            }
                            else if (order_name == "m_company_email" && order_by == "desc")
                            {
                                filteredData = filteredData.OrderByDescending(p => (p.m_company_email));
                            }

                            else if (order_name == "m_company_website" && order_by == "asc")
                            {
                                filteredData = filteredData.OrderBy(p => (p.m_company_website));
                            }
                            else if (order_name == "m_company_website" && order_by == "desc")
                            {
                                filteredData = filteredData.OrderByDescending(p => (p.m_company_website));
                            }



                            var filteredData_new = filteredData.Select(p => new
                            {
                                p.m_company_id,
                                p.m_company_name,
                                p.m_company_email,
                                p.m_company_website,
                                m_company_logo= p.m_company_logo,
                            });

                            if (!String.IsNullOrWhiteSpace(filter))
                            {
                                filteredData_new = filteredData_new.Where(p =>
                                    (
                                        p.m_company_name.ToLower().Contains(filter.ToLower()) ||
                                        p.m_company_email.ToLower().Contains(filter.ToLower()) ||
                                        p.m_company_website.ToLower().Contains(filter.ToLower())
                                    )
                                );
                                totalFilter = filteredData_new.Count();
                            }

                            if (requestLength > 0)
                            {
                                filteredData_new = filteredData_new.Skip(requestStart).Take(requestLength);
                            }

                            var data = filteredData_new.ToList();

                            list_data.Add(new
                            {
                                data = data,
                                totalFilter = totalFilter,
                                totalRow = totalRow,
                            });

                            arm.data = list_data;
                            arm.success();
                        }
                        else if (Request.Query["f"] == "insert_handler")
                        {
                            var m_company_name = String.IsNullOrEmpty(Request.Form["m_company_name"]) ? "" : Request.Form["m_company_name"].ToString();
                            var m_company_email = String.IsNullOrEmpty(Request.Form["m_company_email"]) ? "" : Request.Form["m_company_email"].ToString();
                            var m_company_website = String.IsNullOrEmpty(Request.Form["m_company_website"]) ? "" : Request.Form["m_company_website"].ToString();
                            var name_m_company_logo = String.IsNullOrEmpty(Request.Form["name_m_company_logo"]) ? "" : Request.Form["name_m_company_logo"].ToString();
                            var hidden_m_company_logo = String.IsNullOrEmpty(Request.Form["hidden_m_company_logo"]) ? "" : Request.Form["hidden_m_company_logo"].ToString();
                            string ext = System.IO.Path.GetExtension(name_m_company_logo).ToLower();
                            var file_name_storage = Guid.NewGuid().ToString("N").ToUpper() + ext;
                            
                            if (ext.ToLower() != ".jpeg" && ext.ToLower() != ".png" && ext.ToLower() != ".jpg")
                            {
                                arm.fail();
                                arm.message = "Only (.JPEG | .PNG | .JPG)";
                                throw new System.ArgumentException(arm.message);
                            }

                            var exsis = _context.m_company.Where(e => e.m_company_name == m_company_name).Any();
                            if (exsis)
                            {
                                arm.fail(); //set success status
                                arm.message = "Data Duplicate"; //set success message
                            }
                            else
                            {
                                m_company data_m_company = new m_company
                                {
                                    m_company_name = m_company_name,
                                    m_company_email = m_company_email,
                                    m_company_website = m_company_website,
                                    m_company_logo = file_name_storage,
                                };

                                _context.m_company.Add(data_m_company); //insert m_feature yg diconstruct
                                _context.SaveChanges(); //save changes to database


                                string file_path = path_folder_logo + "/" + file_name_storage;

                                if (!Directory.Exists(path_folder_logo))
                                {
                                    Directory.CreateDirectory(path_folder_logo);
                                }

                                string get_fresh_byte1 = hidden_m_company_logo.Split("base64,")[0];
                                string get_fresh_byte = hidden_m_company_logo.Split("base64,")[1];

                                byte[] convert_to_byte = Convert.FromBase64String(get_fresh_byte);
                                using (var stream_file_bukti_realisasi = new System.IO.MemoryStream(convert_to_byte))
                                    System.IO.File.WriteAllBytes(file_path, convert_to_byte);

                                arm.success(); //set success status
                                arm.message = "Data Has Been Saved"; //set success message
                            }
                        }
                        else if (Request.Query["f"] == "edit_handler")
                        {
                            var m_company_id = String.IsNullOrEmpty(Request.Form["m_company_id"]) ? 0 : Convert.ToInt32(Request.Form["m_company_id"]);
                            var m_company_name = String.IsNullOrEmpty(Request.Form["m_company_name"]) ? "" : Request.Form["m_company_name"].ToString();
                            var m_company_email = String.IsNullOrEmpty(Request.Form["m_company_email"]) ? "" : Request.Form["m_company_email"].ToString();
                            var m_company_website = String.IsNullOrEmpty(Request.Form["m_company_website"]) ? "" : Request.Form["m_company_website"].ToString();
                            var m_company_logo = Request.Form.Files["m_company_logo"];
                            string ext = m_company_logo != null && m_company_logo.Length > 0?System.IO.Path.GetExtension(m_company_logo.FileName).ToLower():"";
                            var file_name_storage = m_company_logo != null && m_company_logo.Length > 0?Guid.NewGuid().ToString("N").ToUpper() + ext:"";
                            var m_company_data = _context.m_company.Where(e => e.m_company_id == m_company_id).SingleOrDefault();

                            if (m_company_logo != null && m_company_logo.Length > 0)
                            {
                                if (ext.ToLower() != ".jpeg" && ext.ToLower() != ".png" && ext.ToLower() != ".jpg")
                                {
                                    arm.fail();
                                    arm.message = "Only (.JPEG | .PNG | .JPG)";
                                    throw new System.ArgumentException(arm.message);
                                }

                                //remove logo
                                string file_path = path_folder_logo + "/" + file_name_storage;

                                if (!Directory.Exists(path_folder_logo))
                                {
                                    Directory.CreateDirectory(path_folder_logo);
                                }

                                var files = Directory.GetFiles(path_folder_logo);

                                foreach (string new_file in files)
                                {
                                    FileInfo fi = new FileInfo(new_file);
                                    if (fi.Name == m_company_data.m_company_logo)
                                    {
                                        fi.Delete();
                                    }
                                }

                                //Save the File.
                                string filePath = path_folder_logo + "/" + file_name_storage;
                                using (var ms = new MemoryStream())
                                {
                                    m_company_logo.CopyTo(ms);
                                    var fileBytes = ms.ToArray();
                                    string s = Convert.ToBase64String(fileBytes);
                                    byte[] convert_to_byte = Convert.FromBase64String(s);

                                    using (var stream_file_bukti_realisasi = new System.IO.MemoryStream(convert_to_byte))
                                        System.IO.File.WriteAllBytes(path_folder_logo + "/" + file_name_storage, convert_to_byte);
                                    // act on the Base64 data
                                }
                            }

                            var exsis = _context.m_company.Where(e => e.m_company_id == m_company_id).Any();
                            if (!exsis)
                            {
                                arm.fail(); //set success status
                                arm.message = "Data is not Found"; //set success message
                            }
                            else
                            {
                                m_company_data.m_company_name = m_company_name;
                                m_company_data.m_company_email = m_company_email;
                                m_company_data.m_company_website = m_company_website;

                                if (m_company_logo != null && m_company_logo.Length > 0)
                                    m_company_data.m_company_logo = file_name_storage;

                                _context.m_company.Update(m_company_data); //insert m_feature yg diconstruct
                                _context.SaveChanges(); //save changes to database

                                arm.success(); //set success status
                                arm.message = "Data Has Been Saved"; //set success message
                            }
                        }
                        else if (Request.Query["f"] == "delete_handler")
                        {
                            var m_company_id = String.IsNullOrEmpty(Request.Query["id"]) ? 0 : Convert.ToInt32(Request.Query["id"].ToString());
                            var m_company_data = _context.m_company.SingleOrDefault(e => e.m_company_id == m_company_id);

                            if (m_company_data == null)
                            {
                                arm.fail(); //set success status
                                arm.message = "Data not found"; //set success message
                            }
                            else
                            {
                                var m_employe_data = _context.m_employe.SingleOrDefault(e => e.m_company_id == m_company_id);

                                if (m_employe_data == null)
                                {
                                    string file_path = path_folder_logo + "/" + m_company_data.m_company_logo;
                                    if (!Directory.Exists(path_folder_logo))
                                    {
                                        Directory.CreateDirectory(path_folder_logo);
                                    }

                                    var files = Directory.GetFiles(path_folder_logo);
                                    foreach (string new_file in files)
                                    {
                                        FileInfo fi = new FileInfo(new_file);
                                        if (fi.Name== m_company_data.m_company_logo)
                                        {
                                            fi.Delete();
                                        }
                                    }

                                    _context.m_company.Remove(m_company_data); //insert m_feature yg diconstruct
                                    _context.SaveChanges(); //save changes to database
                                    arm.success(); //set success status
                                    arm.message = "Data Deleted Successfully"; //set success message
                                }
                                else
                                {
                                    arm.fail(); //set success status
                                    arm.message = "Data Failed to Delete. The Data Has Been Used in Transaction."; //set success message
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        arm.fail();
                        arm.message = ex.Message;
                        transaction.Rollback();
                        AppGlobal.console_log("Error Save: ", ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                arm.fail();
                arm.message = ex.Message;
                AppGlobal.console_log("Error Save: ", ex.ToString());
            }
            return new JsonResult(arm);
        }
    }
}