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
using System.Net;
using System.Net.Mail;

namespace appglobal
{
    public class m_employeModel : ScopePageModel
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

                            var filteredData = (from a in _context.m_employe
                                                from b in _context.m_company
                                                where a.m_company_id == b.m_company_id
                                                select new
                                                {
                                                    a.m_employe_first_name,
                                                    a.m_employe_last_name,
                                                    a.m_employe_phone,
                                                    a.m_employe_email,
                                                    b.m_company_id,
                                                    a.m_employe_id,
                                                    b.m_company_name
                                                }).AsNoTracking();

                            var totalFilter = filteredData.Count();
                            var totalRow = filteredData.Count();

                            if (order_name == "m_employe_first_name" && order_by == "asc")
                            {
                                filteredData = filteredData.OrderBy(p => (p.m_employe_first_name));
                            }
                            else if (order_name == "m_employe_first_name" && order_by == "desc")
                            {
                                filteredData = filteredData.OrderByDescending(p => (p.m_employe_first_name));
                            }

                            else if (order_name == "m_employe_last_name" && order_by == "asc")
                            {
                                filteredData = filteredData.OrderBy(p => (p.m_employe_last_name));
                            }
                            else if (order_name == "m_employe_last_name" && order_by == "desc")
                            {
                                filteredData = filteredData.OrderByDescending(p => (p.m_employe_last_name));
                            }

                            else if (order_name == "m_employe_phone" && order_by == "asc")
                            {
                                filteredData = filteredData.OrderBy(p => (p.m_employe_phone));
                            }
                            else if (order_name == "m_employe_phone" && order_by == "desc")
                            {
                                filteredData = filteredData.OrderByDescending(p => (p.m_employe_phone));
                            }

                            else if (order_name == "m_employe_email" && order_by == "asc")
                            {
                                filteredData = filteredData.OrderBy(p => (p.m_employe_email));
                            }
                            else if (order_name == "m_employe_email" && order_by == "desc")
                            {
                                filteredData = filteredData.OrderByDescending(p => (p.m_employe_email));
                            }

                            else if (order_name == "m_company_name" && order_by == "asc")
                            {
                                filteredData = filteredData.OrderBy(p => (p.m_company_name));
                            }
                            else if (order_name == "m_company_name" && order_by == "desc")
                            {
                                filteredData = filteredData.OrderByDescending(p => (p.m_company_name));
                            }

                            var filteredData_new = filteredData.Select(p => new
                            {
                                p.m_company_id,
                                p.m_company_name,
                                p.m_employe_first_name,
                                p.m_employe_last_name,
                                p.m_employe_email,
                                p.m_employe_phone,
                                p.m_employe_id
                            });

                            if (!String.IsNullOrWhiteSpace(filter))
                            {
                                filteredData_new = filteredData_new.Where(p =>
                                    (
                                        p.m_company_name.ToLower().Contains(filter.ToLower()) ||
                                        p.m_employe_first_name.ToLower().Contains(filter.ToLower()) ||
                                        p.m_employe_last_name.ToLower().Contains(filter.ToLower()) ||
                                        p.m_employe_email.ToLower().Contains(filter.ToLower()) ||
                                        p.m_employe_phone.ToLower().Contains(filter.ToLower())
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
                            var m_employe_first_name = String.IsNullOrEmpty(Request.Form["m_employe_first_name"]) ? "" : Request.Form["m_employe_first_name"].ToString();
                            var m_employe_last_name = String.IsNullOrEmpty(Request.Form["m_employe_last_name"]) ? "" : Request.Form["m_employe_last_name"].ToString();
                            var m_employe_email = String.IsNullOrEmpty(Request.Form["m_employe_email"]) ? "" : Request.Form["m_employe_email"].ToString();
                            var m_employe_phone = String.IsNullOrEmpty(Request.Form["m_employe_phone"]) ? "" : Request.Form["m_employe_phone"].ToString();
                            var m_company_id = String.IsNullOrEmpty(Request.Form["m_company_id"]) ? 0 : Convert.ToInt32(Request.Form["m_company_id"]);

                            var exsis_email = _context.m_employe.Where(e => e.m_employe_email == m_employe_email).Any();
                            var exsis_phone = _context.m_employe.Where(e => e.m_employe_phone == m_employe_phone).Any();

                            if (exsis_email)
                            {
                                arm.fail(); //set success status
                                arm.message = "Duplicate Email"; //set success message
                            }
                            else if (exsis_phone)
                            {
                                arm.fail(); //set success status
                                arm.message = "Duplicate Phone"; //set success message
                            }
                            else
                            {
                                m_employe data_m_employe = new m_employe
                                {
                                    m_employe_first_name = m_employe_first_name,
                                    m_employe_last_name = m_employe_last_name,
                                    m_employe_email = m_employe_email,
                                    m_employe_phone = m_employe_phone,
                                    m_company_id = m_company_id,
                                    m_employe_password = loginModel.create_password_SHA256("password"),
                                };

                                _context.m_employe.Add(data_m_employe); //insert m_feature yg diconstruct
                                _context.SaveChanges(); //save changes to database

                                #region email
                                var m_company_data = _context.m_company.Where(e => e.m_company_id == m_company_id).SingleOrDefault();

                                var fromAddress = new MailAddress("wardah.rose12345@gmail.com", "GR Tech");
                                var toAddress = new MailAddress(m_company_data.m_company_email, m_company_data.m_company_name);
                                const string fromPassword = "Wa21121988";
                                const string subject = "Notification new employee";
                                string body = @"<html xmlns='http://www.w3.org/1999/xhtml'>
                                                        <head>
                                                            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
                                                            <title>Demystifying Email Design</title>
                                                            <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                                                            <body style='margin: 0; padding: 0;'>
                                                                <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border: #ffffff;background:#d0d2d5;padding-bottom:20px'>
                                                                    <tr>
                                                                        <td colspan='3' style='border: #ffffff;'>
                                                                            <img style='height: auto;text-align: center;display: block;margin-left: auto;margin-right: auto;width: 50%;padding-top:10px' src='freepikpsd.com/file/2019/10/your-logo-png-7-Transparent-Images.png'> <br>
                                                                            <h3 style='text-align:center'>Information New Employee</h3>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width='20%' style='border: #ffffff;'></td>
                                                                        <td width='60%' style='border: #ffffff;'>
                                                                            <table border='1' cellpadding='0' cellspacing='0' width='100%' style='border: #ffffff;'>
                                                                                <tr>
                                                                                    <td width='200px' style='border: #ffffff;'>
                                                                                        First Name
                                                                                    </td>
                                                                                    <td width='3%' style='border: #ffffff;'>
                                                                                        :
                                                                                    </td>
                                                                                    <td style='border: #ffffff;'>" + m_employe_first_name + @"</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width='200px' style='border: #ffffff;'>
                                                                                        Last Name
                                                                                    </td>
                                                                                    <td width='5%' style='border: #ffffff;'>
                                                                                        :
                                                                                    </td>
                                                                                    <td style='border: #ffffff;'>" + m_employe_last_name + @"</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width='200px' style='border: #ffffff;'>
                                                                                        Email
                                                                                    </td>
                                                                                    <td width='5%' style='border: #ffffff;'>
                                                                                        :
                                                                                    </td>
                                                                                    <td style='border: #ffffff;'>" + m_employe_email + @"</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width='200px' style='border: #ffffff;'>
                                                                                        Phone
                                                                                    </td>
                                                                                    <td width='5%' style='border: #ffffff;'>
                                                                                        :
                                                                                    </td>
                                                                                    <td style='border: #ffffff;'>" + m_employe_phone + @"</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td width='20%' style='border: #ffffff;'></td>
                                                                    </tr>
                                                                </table>
                                                            </body>
                                                        </head>
                                                        </html>";

                                var smtp = new SmtpClient
                                {
                                    Host = "smtp.gmail.com",
                                    Port = 587,
                                    EnableSsl = true,
                                    DeliveryMethod = SmtpDeliveryMethod.Network,
                                    UseDefaultCredentials = false,
                                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                                };
                                using (var message = new MailMessage(fromAddress, toAddress)
                                {
                                    Subject = subject,
                                    Body = body,
                                    IsBodyHtml = true,
                                })
                                {
                                    smtp.Send(message);
                                }
                                #endregion

                                arm.success(); //set success status
                                arm.message = "Data Has Been Saved"; //set success message
                            }
                        }
                        else if (Request.Query["f"] == "edit_handler")
                        {
                            var m_company_id = String.IsNullOrEmpty(Request.Form["m_company_id"]) ? 0 : Convert.ToInt32(Request.Form["m_company_id"]);
                            var m_employe_id = String.IsNullOrEmpty(Request.Form["m_employe_id"]) ? 0 : Convert.ToInt32(Request.Form["m_employe_id"]);
                            var m_employe_first_name = String.IsNullOrEmpty(Request.Form["m_employe_first_name"]) ? "" : Request.Form["m_employe_first_name"].ToString();
                            var m_employe_last_name = String.IsNullOrEmpty(Request.Form["m_employe_last_name"]) ? "" : Request.Form["m_employe_last_name"].ToString();
                            var m_employe_email = String.IsNullOrEmpty(Request.Form["m_employe_email"]) ? "" : Request.Form["m_employe_email"].ToString();
                            var m_employe_phone = String.IsNullOrEmpty(Request.Form["m_employe_phone"]) ? "" : Request.Form["m_employe_phone"].ToString();
                            var m_employe_data = _context.m_employe.Where(e => e.m_employe_id == m_employe_id).SingleOrDefault();

                            var exsis = _context.m_employe.Where(e => e.m_employe_id == m_employe_id).Any();
                            if (!exsis)
                            {
                                arm.fail(); //set success status
                                arm.message = "Data is not Found"; //set success message
                            }
                            else
                            {


                                m_employe_data.m_company_id = m_company_id;
                                m_employe_data.m_employe_first_name = m_employe_first_name;
                                m_employe_data.m_employe_last_name = m_employe_last_name;
                                m_employe_data.m_employe_email = m_employe_email;
                                m_employe_data.m_employe_phone = m_employe_phone;

                                _context.m_employe.Update(m_employe_data); //insert m_feature yg diconstruct
                                _context.SaveChanges(); //save changes to database

                                arm.success(); //set success status
                                arm.message = "Data Has Been Saved"; //set success message
                            }
                        }
                        else if (Request.Query["f"] == "delete_handler")
                        {
                            var m_employe_id = String.IsNullOrEmpty(Request.Query["id"]) ? 0 : Convert.ToInt32(Request.Query["id"].ToString());
                            var m_employe_data = _context.m_employe.SingleOrDefault(e => e.m_employe_id == m_employe_id);

                            if (m_employe_data == null)
                            {
                                arm.fail(); //set success status
                                arm.message = "Data not found"; //set success message
                            }
                            else
                            {
                                _context.m_employe.Remove(m_employe_data); //insert m_feature yg diconstruct
                                _context.SaveChanges(); //save changes to database
                                arm.success(); //set success status
                                arm.message = "Data Deleted Successfully"; //set success message
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