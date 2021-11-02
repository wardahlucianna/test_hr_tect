using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace appglobal
{
    public class crud_base_profil_dept
    {

        public static string get_group_summernote(string label, string name, string value, bool cannot_null = false, bool script = true)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
            string script_text = script == false ? "" : "<script> $('#" + name + @"').summernote({
                 height: 300,
                 minheight: null,
                 maxheight: null,
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['para', ['ul', 'ol']],
                  ],
                callbacks: {
                    onPaste: function (e) {
                        var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('text/html');
                        e.preventDefault();
                        var div_new = $('<div />');

                        div_new.append(bufferText);
                        console.log(div_new.html());
                        div_new.find('*').removeAttr('style');
                        div_new.find('*').removeAttr('size');

                        var div = $('<div />');
                        div.append(div_new.html());
                        setTimeout(function () {
                            document.execCommand('insertHtml', false, div_new.html());
                        }, 10);
                    }
                }
               }); //ckeditor.replace('" + name + @"'); </script>";

            string input_group = @"
               <div class='form-group'><label >" + label + @"</label><textarea rows='3' class='form-control " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"' placeholder='" + label + @"'>" + value + "</textarea></div>" + script_text;
            return input_group;
        }

        public static string get_summernote(string label, string name, string value, bool cannot_null = false, bool script = true)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
            string script_text = script == false ? "" : "<script> $('#" + name + @"').summernote({
                 height: 300,
                 minheight: null,
                 maxheight: null,
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['para', ['ul', 'ol']],
                  ],
                callbacks: {
                    onPaste: function (e) {
                        var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('text/html');
                        e.preventDefault();
                        var div_new = $('<div />');

                        div_new.append(bufferText);
                        console.log(div_new.html());
                        div_new.find('*').removeAttr('style');
                        div_new.find('*').removeAttr('size');

                        var div = $('<div />');
                        div.append(div_new.html());
                        setTimeout(function () {
                            document.execCommand('insertHtml', false, div_new.html());
                        }, 10);
                    }
                }
               }); //ckeditor.replace('" + name + @"'); </script>";

            string input_group = @"<div class='form-group'><textarea rows='3' class='form-control " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"' placeholder='" + label + @"'>" + value + "</textarea></div>"+script_text;
            return input_group;
        }

        public static string get_input_group(string label, string name, string value, string type = "text", bool cannot_null = false, bool read_only=false, string max_input= "100")
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_value = read_only == false ? "" : "readonly";

            string input_group = @"<div class='form-group'>
                                <label>" + label + @"</label>
                                <input type='" + type + "' class='form-control " + cannot_null_value + @"' id='" + name + @"' placeholder='" + label + @"' maxlength='"+ max_input + @"' name = '" + name + @"' value = '" + value + @"' " + read_only_value + @">
                            </div>";
            return input_group;
        }

        public static string get_input_date_range_group(string label, string name, string value, string type = "text", bool cannot_null = false, bool read_only = false, string value_start = "", string value_end = "")
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_value = read_only == false ? "" : "readonly";
            string start_value = value_start != "" ? Convert.ToDateTime(value_start).ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy");
            string end_value = value_end != "" ? Convert.ToDateTime(value_end).ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy");

            string input_group = @"<div class='form-group'>
                                <label>" + label + @"</label>
                                <input type='" + type + "' class='form-control " + cannot_null_value + @"' id='" + name + @"' placeholder='" + label + @"' name = '" + name + @"' value = '" + value + @"' " + read_only_value + @">
                                <input type='hidden' class='form-control' id='" + name + @"_start' name = '" + name + @"_start' value='"+ value_start + @"'>
                                <input type='hidden' class='form-control' id='" + name + @"_end' name = '" + name + @"_end' value='" + value_end + @"'>
                            </div>
                            <script>
                            $('#" + name + @"').daterangepicker({
                                autoUpdateInput: false,
                                showDropdowns: true,
                                startDate: moment('"+ value_start + @"'),
                                endDate: moment('" + value_end + @"'),
                                locale: {
                                  format: 'MM/YYYY'
                                }
                            }, function (start, end, label) {
                                var endDate = moment(end).endOf('month');
                                var startDate = moment(start).startOf('month');
                                $('#" + name + @"').data('daterangepicker').setEndDate(endDate);
                                $('#" + name + @"').val(start.format('MM/YYYY')+' - '+end.format('MM/YYYY'));
                                $('#" + name + @"_start').val(startDate.format('YYYY/MM/DD'));
                                $('#" + name + @"_end').val(endDate.format('YYYY/MM/DD'));
                            });
                            </script>";
            return input_group;
        }

        public static string get_input_group_right(string label, string name, string value, string type = "text", bool cannot_null = false, bool read_only=false, string max_input= "100", string label_group="")
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_value = read_only == false ? "" : "readonly";

            string input_group = @"<div class='form-group'>
                                <label>" + label + @"</label>
                                <div class='input-group'>
                                    <input type='" + type + @"' name='" + name + @"' id='" + name + @"' class='form-control " + cannot_null_value + @"' placeholder='" + label + @"' value='" + value + @"' maxlength='" + max_input + @"' data-target='input_button'>
                                    <div class='input-group-prepend'>
                                        <span class='input-group-text' id='basic-addon1'>" + label_group + @"</span>
                                    </div>
                                </div>
                            </div>";
            return input_group;
        }

        public static string get_input_left(string label, string name, string value, string type = "text", bool cannot_null = false, bool read_only = false, string max_input = "100", string label_group = "")
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_value = read_only == false ? "" : "readonly";

            string input_group = @"<div class='form-group'>
                                <div class='input-group'>
                                    <div class='input-group-prepend'>
                                        <span class='input-group-text' id='basic-addon1'>" + label_group + @"</span>
                                    </div>
                                    <input type='" + type + @"' name='" + name + @"' id='" + name + @"' class='form-control " + cannot_null_value + @"' placeholder='" + label + @"' value='" + value + @"' maxlength='" + max_input + @"' data-target='input_button'>
                                </div>
                            </div>";
            return input_group;
        }

        public static string get_toggle_group(string label, string name, bool _checked = false, string on_text = "Ya", string off_text = "Tidak")
        {
            string checked_string = _checked == false ? "" : "checked";

            string input_group = @"<div class='form-group'>
                                    <label>" + label + @"</label>
                                    <input type='checkbox' name='" + name + @"_area' id='" + name + @"_area' class='form-control' " + checked_string + @" data-on-text='" + on_text + @"' data-off-text='" + off_text + @"' onchange='toggle_update(""" + name + @""")'>
                                    <input type='hidden' id='" + name + @"' class='form-control' name='" + name + @"' value='" + _checked.ToString().ToLower() + @"'>
                                </div>
                                 <script>
                                    $('#" + name + @"_area').bootstrapSwitch();
                                  </script>";
            return input_group;
        }

        public static string get_input_textarea_group(string label, string name, string value,bool cannot_null = false, bool read_only = false, string row = "3")
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_value = read_only == false ? "" : "readonly";

            string input_group = @"
                <div class='form-group'>
                        <label>" + label + @"</label>
                        <textarea class='form-control " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"' rows='" + row + @"' placeholder='" + label + @"'>" + value + @"</textarea>
                    </div>";

            return input_group;
        }

        public static string get_input_textarea(string label, string name, string value, bool cannot_null = false, bool read_only = false, string row="3")
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_value = read_only == false ? "" : "readonly";

            string input_group = @"
                <div class='form-group'>
                        <textarea class='form-control " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"' rows='"+ row + @"' placeholder='" + label + @"'>" + value + @"</textarea>
                    </div>";

            return input_group;
        }

        public static string get_select2_group(string label, string name, bool cannot_null = true, dynamic select_list = null, string default_value = "-1", bool disabled = false, bool multiple = false, bool script = true)
        {

            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
            string multiple_select = multiple == false ? "" : "multiple='multiple'";
            string name_input = multiple == false ? name : name + "[]";
            string scripts = script == true ? "<script type='text/javascript'>$(document).ready(function() {$('#" + name + @"').select2({placeholder: 'Pilih " + label + @"',allowClear: true, width: '100%'});$('#" + name + @"').on('change', function () {$('#" + name + @"_area').val($(this).val())})});</script>" : "";
            string disabled_string = disabled == false ? "" : "disabled";
            string select = "<select  name='" + name_input + "' id='" + name + "' placeholder='" + label + @"' class='form-control " + cannot_null_value + "' " + disabled_string + " " + multiple_select + @">";
            select += "<option value = ''>Pilih " + label + "</option>";
            if (select_list != null)
            {
                foreach (var select_list_data in select_list)
                {
                    //logic to parsing anonymous list with propertyInfo looping
                    Type t = select_list_data.GetType();
                    PropertyInfo[] pi = t.GetProperties();

                    //define what property to be assigned
                    string select_value = pi[0].GetValue(select_list_data) + "";
                    string select_display = pi[1].GetValue(select_list_data);

                    //create select options
                    select = select + "<option value='" + select_value + "' ";
                    // AppGlobal.console_log("select_value", select_value);
                    //AppGlobal.console_log("default_value", default_value);
                    if (select_value + "" == default_value)
                    {
                        select = select + "selected";
                    }
                    //AppGlobal.console_log("select", select);
                    select = select + ">" + select_display + "</option>";
                }
            }
            select = select + "</select>";

            string input_group = @"
                <div class='form-group'>
                        <label>" + label + @"</label>
                        " + select + @"
                        <input type='hidden' class='form-control' id='" + name + @"_area' name='" + name + @"_area'  value='" + default_value + @"'>
                    </div>" + scripts;
            return input_group;
        }

        public static string get_select2_group_value(string label, string name, bool cannot_null = true, dynamic select_list = null, string default_value = "-1", bool disabled = false, bool multiple = false, bool script = true)
        {

            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string red_symbol = cannot_null == false ? "" : "<span style='color:red'>*</span>";
            string multiple_select = multiple == false ? "" : "multiple='multiple'";
            string name_input = multiple == false ? name : name + "[]";
            string scripts = script == true ? "<script type='text/javascript'>$(document).ready(function() {$('#" + name + @"').select2({placeholder: 'Pilih " + label + @"',allowClear: true, width: '100%'});$('#" + name + @"').on('change', function () {$('#" + name + @"_area').val($(this).val())})});</script>" : "";
            string disabled_string = disabled == false ? "" : "disabled";
            string select = "<select  name='" + name_input + "' id='" + name + "' placeholder='" + label + @"' class='form-control " + cannot_null_value + "' " + disabled_string + " " + multiple_select + @">";
            select += "<option value = ''>Pilih " + label + "</option>";
            if (select_list != null)
            {
                foreach (var select_list_data in select_list)
                {
                    //logic to parsing anonymous list with propertyInfo looping
                    Type t = select_list_data.GetType();
                    PropertyInfo[] pi = t.GetProperties();

                    //define what property to be assigned
                    string select_value = pi[0].GetValue(select_list_data) + "";

                    //create select options
                    select = select + "<option value='" + select_value + "' ";
                    // AppGlobal.console_log("select_value", select_value);
                    //AppGlobal.console_log("default_value", default_value);
                    if (select_value + "" == default_value)
                    {
                        select = select + "selected";
                    }
                    //AppGlobal.console_log("select", select);
                    select = select + ">" + select_value + "</option>";
                }
            }
            select = select + "</select>";

            string input_group = @"
                <div class='form-group'>
                        <label>" + label + @"</label>
                        " + select + @"
                        <input type='hidden' class='form-control' id='" + name + @"_area' name='" + name + @"_area'  value='" + default_value + @"'>
                    </div>" + scripts;
            return input_group;
        }

        public static string get_custom_button(string label, string name, string classes = "btn btn-primary", string events = "", string icon = "", string style = "", string target_data = "", bool disabled = false)
        {
            string disabled_value = disabled == false ? "" : "disabled";
            string targer_data_value = target_data == "" ? "" : "data-target='#"+ target_data + "'";
            string toggle_data_value = target_data == "" ? "" : "data-toggle='modal'";
            string edit_button = @"<button type='button' id='" + name + @"' class='btn-fw " + classes + @"' onclick='"+ events + @"' style='"+ style + @"' "+ targer_data_value + @"  " + toggle_data_value + @" "+ disabled_value + @">"+ icon + label + @"</button> ";
            return edit_button;
        }

        public static string get_custom_button_small(string label, string name, string classes = "btn btn-primary", string events = "", string icon = "", string style = "", string target_data = "", bool disabled = false)
        {
            string disabled_value = disabled == false ? "" : "disabled";
            string targer_data_value = target_data == "" ? "" : "data-target='#" + target_data + "'";
            string toggle_data_value = target_data == "" ? "" : "data-toggle='modal'";
            string edit_button = @"<button type='button' id='" + name + @"' class='" + classes + @"' onclick='" + events + @"' style='" + style + @"' " + targer_data_value + @"  " + toggle_data_value + @" " + disabled_value + @">" + icon + label + @"</button> ";
            return edit_button;
        }

        public static string get_file_uraian_pekerjaan(string label, string name, bool cannot_null = false)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string input_group = @"
                    <div class='custom-file'>
                        <input type='file' class='custom-file-input " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"' placeholder='" + label + @"'>
        			    <input type='hidden' id='hidden_uraian_" + name + @"' name='hidden_uraian_" + name + @"'>
        			    <input type='hidden' id='name_uraian_" + name + @"' name='name_uraian_" + name + @"'>
                        <label class='custom-file-label' for=''" + name + @"' id='label_uraian_" + name + @"'>Choose file</label>
                    </div>
            <script>
        	    $(function () {
                    $('#" + name + @"').on('change', function () {
                        var fileName = $(this).val().split('\\').pop();
                        $(this).siblings('#label_uraian_" + name + @"').addClass('selected').html(fileName);
        		        $('#name_uraian_" + name + @"').val(fileName);

                        if (this.files && this.files[0]) {
        				    var reader = new FileReader();
        				    reader.onload = image_uraian;
        				    reader.readAsDataURL(this.files[0]);
        			    }
                    });
        	    });

        	    function image_uraian(e) {
        		    $('#hidden_uraian_" + name + @"').val(e.target.result);
        	    };
            </script>";
            return input_group;
        }

        public static string get_file_upload(string label, string name, bool cannot_null = false)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string input_group = @"<div class='form-group'>
                        <label>" + label + @"</label>
                       <div class='custom-file'>
                        <input type='file' class='custom-file-input " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"' placeholder='" + label + @"'>
        			    <input type='hidden' id='hidden_" + name + @"' name='hidden_" + name + @"'>
        			    <input type='hidden' id='name_" + name + @"' name='name_" + name + @"'>
                        <label class='custom-file-label' for=''" + name + @"' id='label_" + name + @"'>Choose file</label>
                    </div>
                    </div>
                    
            <script>
        	    $(function () {
                    $('#" + name + @"').on('change', function () {
                        var fileName = $(this).val().split('\\').pop();
                        $(this).siblings('#label_" + name + @"').addClass('selected').html(fileName);
        		        $('#name_" + name + @"').val(fileName);

                        if (this.files && this.files[0]) {
        				    var reader = new FileReader();
        				    reader.onload = image;
        				    reader.readAsDataURL(this.files[0]);
        			    }
                    });
        	    });

        	    function image(e) {
        		    $('#hidden_" + name + @"').val(e.target.result);
        	    };
            </script>";
            return input_group;
        }

        public static string get_input(string name, string type = "text", string value = "", string label = "", bool read_only = false, bool cannot_null = false, string style="")
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_string = read_only == false ? "" : "readonly";

            string input = @"<div class='form-group' "+style+@">
                        <input type='" + type + @"' id='" + name + @"' class='form-control "+ cannot_null_value + @"' placeholder='" + label + @"' name='" + name + @"' value='" + value + @"' " + read_only_string + @">
                    </div>"; 
            return input;
        }

        public static string get_input_kriteria(string type, string name, string placeholder, string value,bool cannot_null,bool read_only)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_string = read_only == false ? "" : "readonly";

            string input = @"
                            <input type='"+ type + @"' name = '"+name+@"' id = '"+name+ @"' class='form-control " + cannot_null_value + @"' placeholder='" + placeholder + @"' value='" + value + @"'  " + read_only_string + @">
                   ";
            return input;
        }

        public static string get_group_input_button_modal(string label, string name, string value, string type = "text", string id_button="", string name_button="", string data_target = "", bool read_only = false, bool cannot_null = false)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_value = read_only == false ? "" : "readonly";

            string input = @"<div class='form-group'>
                                <label>"+ label + @"</label>
                                <div class='input-group'>
                                    <input type='"+ type + @"' class='form-control "+ cannot_null_value + @"' placeholder='"+ label + @"' aria-label='' aria-describedby='basic-addon2' style='height: 43px;' value='"+ value + @"' id='"+ name + @"' nama='" + name + @"' " + read_only_value + @" data-target='input_button'>
                                    <div class='input-group-append'>
                                        <button class='btn btn-success' type='button' style='height: 43px;' data-toggle='modal' data-target='#" + data_target + @"' id='"+ id_button + @"'> <i class='fa fa-search'></i>" + name_button + @"</button>
                                    </div>
                                </div>
                            </div>";
            return input;
        }

        public static string get_modal_content(string label, string name)
        {
            string html = @"<div class='modal fade' id='" + name + @"' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'>
                        <div class='modal-dialog modal-lg' role='document'>
                            <div class='modal-content'>
                                <div class='modal-header'>
                                    <h5 class='modal-title' id='" + name + @"_label'>" + label + @"</h5>
                                    <button type='button' class='close' data-dismiss='modal' aria-label='Close'>
                                        <span aria-hidden='true'>&times;</span>
                                    </button>
                                </div>
                                <div class='modal-body' id='modal_content_" + name + @"'>


                                </div>
                            </div>
                        </div>
                    </div>";
            return html;
        }

        public static string get_input_programkerja(string label,string type, string name, string placeholder, string value, bool cannot_null, bool read_only)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string read_only_string = read_only == false ? "" : "readonly";

            string input = @"
                            <label>" + label + @"</label>
                            <input type='" + type + @"' name = '" + name + @"' id = '" + name + @"' class='form-control " + cannot_null_value + @"' placeholder='" + placeholder + @"' value='" + value + @"'  " + read_only_string + @">
                   ";
            return input;
        }

        public static string get_input_programkerja_textarea(string label, string name, string placeholder, string value, bool cannot_null, bool disabled)
        {
            string cannot_null_value = cannot_null == false ? "" : "cannot-null";
            string disabled_string = disabled == false ? "" : "disabled";

            string input = @"
                            <label>" + label + @"</label>
                            <textarea class='form-control " + cannot_null_value + @"' id='" + name + @"' name='" + name + @"' rows='6' placeholder='" + placeholder + @"'"+ disabled_string + ">" + value + @"</textarea>
                   ";
            return input;
        }

    }
}