using appglobal.models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace appglobal
{
    /// <summary>
    /// Main class for initiating database content on .netcore
    /// </summary>
    public static class DbInitializer
    {

        /// <summary>
        /// Main method to populate database content
        /// </summary>
        public static void Initialize()
        {
            using (var _context = new test_hr_tect_model(AppGlobal.get_db_option()))
            {
                //run primary migration method
                _context.Database.Migrate();

                // Look for any user or feature group
                if (_context.m_employe.Any() || _context.m_feature_group.Any())
                {
                    return; // if DB has been seeded, exit the method;
                }
                else
                {
                    db_init.Initialize();
                }
            }
        }
    }
}