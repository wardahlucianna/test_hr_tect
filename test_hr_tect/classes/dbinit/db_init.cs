using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations;
using appglobal.models;
using appglobal;
using System.IO;
using System.Text.RegularExpressions;

namespace appglobal
{
    public static class db_init
    {
        public static void Initialize()
        {
            using (var _context = new test_hr_tect_model(AppGlobal.get_db_option()))
            {

                #region Akses
                #region m_feature_group
                List<m_feature_group> list_m_feature_group = new List<m_feature_group>
                {
                    new m_feature_group{m_application_id=1,feature_group_name="Akses", feature_group_url="akses", feature_group_squance=1,feature_group_status="Aktif", feature_group_create_at=DateTime.Now, feature_group_icon = "fa fa-key"},
                };
                _context.m_feature_group.AddRange(list_m_feature_group);
                _context.SaveChanges();
                AppGlobal.console_log("m_feature_group", "");
                #endregion

                #region m_feature
                var m_feature_group_id = list_m_feature_group.SingleOrDefault(e => e.feature_group_url == "akses").m_feature_group_id;
                List<m_feature> list_m_feature = new List<m_feature>
                {
                    new m_feature{m_feature_group_id=m_feature_group_id, feature_name="Company", feature_url="m_company", feature_icon = "fa fa-qq", feature_visible=true, feature_sequence=1, feature_status="Aktif", feature_create_at=DateTime.Now},
                    new m_feature{m_feature_group_id=m_feature_group_id, feature_name="Employe", feature_url="m_employe", feature_icon = "fa fa-qq", feature_visible=true, feature_sequence=2, feature_status="Aktif", feature_create_at=DateTime.Now},
                };
                _context.m_feature.AddRange(list_m_feature);
                _context.SaveChanges();
                AppGlobal.console_log("m_feature", "");
                #endregion

                #region m_company
                List<m_company> list_m_company = new List<m_company>
                {
                    new m_company{m_company_name="Admin Company", m_company_logo="-", m_company_website = "http://www.google.com/",m_company_email="www.admin_company@admin.com"},
                };
                _context.m_company.AddRange(list_m_company);
                _context.SaveChanges();
                AppGlobal.console_log("m_company", "");
                #endregion

                #region m_employe
                var m_company_id = list_m_company.SingleOrDefault(e => e.m_company_name == "Admin Company").m_company_id;
                List<m_employe> list_m_employe = new List<m_employe>
                {
                    new m_employe{m_employe_first_name="Admin", m_employe_last_name="Admin", m_employe_phone = "081228180812",m_employe_email = "www.admin@admin.com",m_company_id=1,m_employe_password="5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8"},

                };
                _context.m_employe.AddRange(list_m_employe);
                _context.SaveChanges();
                AppGlobal.console_log("m_company", "");
                #endregion

                #region m_parameter
                List<m_parameter> m_parameter = new List<m_parameter> {
                  new m_parameter {
                    parameter_group = "Base Setting", parameter_key = "Session Timeout", parameter_value = "3600"
                  },
                  new m_parameter {
                    parameter_group = "Base Setting", parameter_key = "MaxArchiveFiles", parameter_value = "730"
                  },
                };
                foreach (m_parameter m_parameter_data in m_parameter)
                {
                    _context.m_parameter.Add(m_parameter_data);
                    AppGlobal.console_log("m_parameter", JsonConvert.SerializeObject(m_parameter_data));
                }
                _context.SaveChanges();
                #endregion
                #endregion

            }
        }
    }

    public class list_m_jabatan
    {
        public string jabatan { set; get; }
        public string namaJabatan { set; get; }
    }

    public class list_code
    {
        public string nama { set; get; }
    }

    public class list_m_unit_kerja
    {
        public string unitKerja { set; get; }
        public string namaUnitKerja { set; get; }
        public string parentUnitKerja { set; get; }
        public string orgLevelName { set; get; }
    }

    public class list_m_posisi
    {
        public string posCode { set; get; }
        public string posTitle { set; get; }
        public string directSuperior { set; get; }
        public string jobId { set; get; }
        public string unitKerjaId { set; get; }
    }

    public class list_m_karyawan
    {
        public string nik { set; get; }
        public string nikSap { set; get; }
        public string nama { set; get; }
        public string noHp { set; get; }
        public string statusKaryawan { set; get; }
        public string unitKerjaId { set; get; }
        public string pGrade { set; get; }
        public string jobId { set; get; }
        public string jobTitle { set; get; }
        public string posCode { set; get; }
        public string tempatLahir { set; get; }
        public string tglLahir { set; get; }
        public string tanggalMasuk { set; get; }
        public string tanggalPensiun { set; get; }
        public string alamat { set; get; }
        public string agama { set; get; }
        public string jenisKelamin { set; get; }
        public string pendidikanAkhir { set; get; }
        public string pendidikanDiterima { set; get; }
        public string email { set; get; }
        public string foto { set; get; }
    }

}